#nullable enable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Extensions;
using Greta.BO.BusinessLogic.Handlers.DataHandlers;
using Greta.BO.BusinessLogic.Handlers.DataHandlers.Importers;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Column;
using Greta.Sdk.Core.Models.Pager;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

//using Greta.BO.BusinessLogic.Handlers.Imports;

namespace Greta.BO.BusinessLogic.Service
{
    public interface ICSVMappingService : IGenericBaseService<CSVMapping>
    {
        Task CSVMappingImport(
            IFormFile csvFile,
            char separator,
            ModelImport modelImport,
            List<long> storesId,
            Dictionary<string, string> mapping
        );

        Task<CSVMapping> GetByName(string name, long Id = -1);
        (List<string>, List<string>, List<string>) GetColumsName(IFormFile csvFile, char separator, ModelImport modelImport);
        List<ColumnNameModel> GetColumsNameByModelExport(ModelImport modelExport);
        Task<CSVMapping?> GetById(long id);
        Task<List<CSVMapping>> GetByModelImport(ModelImport modelImport);

        Task<Pager<CSVMapping>> Filter(int currentPage, int pageSize, CSVMapping filter, string searchstring,
            string sortstring, ModelImport modelImport);

        Task<Pager<ColumnNameModel>> FilterColumnName(int currentPage, int pageSize, ColumnNameModel filter,
            string searchstring,
            string sortstring, ModelImport modelImport);
    }

    public class CSVMappingService : BaseService<ICsvMappingRepository, CSVMapping>, ICSVMappingService
    {
        private readonly IServiceProvider _provider;


        private readonly ProductImport productImport;
        private readonly ScaleProductImport scaleProductImport;
        private readonly DepartmentImport departmentImport;
        private readonly CategoryImport categoryImport;
        private readonly ScaleCategoryImport scaleCategoryImport;
        private readonly FamilyImport familyImport;

        private readonly INotifier _notifier;


        public CSVMappingService(
            IServiceProvider provider,
            ICsvMappingRepository CSVMappingRepository,
            ILogger<CSVMappingService> logger,
            ILogger<ProductImport> loggerProductImport,
            ILogger<ScaleProductImport> loggerScaleProductImport,
            ILogger<DepartmentImport> loggerDepartmentImport,
            ILogger<CategoryImport> loggerCategoryImport,
            ILogger<ScaleCategoryImport> loggerScaleCategoryImport,
            ILogger<FamilyImport> loggerFamilyImport,
            ILogger<Introspector<Category>> inCatLogger,
            ILogger<Introspector<Department>> inDepLogger,
            ILogger<Introspector<Family>> inFamLogger,
            ILogger<Introspector<Product>> inProdLogger,
            ILogger<Introspector<ScaleCategory>> inSCLogger,
            ILogger<Introspector<ScaleProduct>> inSPLogger,
            INotifier notifier
        )
            : base(CSVMappingRepository, logger)
        {
            _provider = provider;

            productImport = new ProductImport(loggerProductImport, provider, inProdLogger, notifier);
            scaleProductImport = new ScaleProductImport(loggerScaleProductImport, provider, inSPLogger, notifier);
            departmentImport = new DepartmentImport(loggerDepartmentImport, provider, inDepLogger, notifier);
            categoryImport = new CategoryImport(loggerCategoryImport, provider, inCatLogger, notifier);
            scaleCategoryImport = new ScaleCategoryImport(loggerScaleCategoryImport, provider, inSCLogger, notifier);
            familyImport = new FamilyImport(loggerFamilyImport, provider, inFamLogger, notifier);
            _notifier = notifier;
        }

        /// <summary>
        ///     Get colums name list
        /// </summary>
        /// <param name="csvFile">csv file</param>
        /// <param name="separator">csv separador character</param>
        /// <param name="modelImport">model to import</param>
        /// <returns>Colums list</returns>
        public async Task CSVMappingImport(
            IFormFile csvFile,
            char separator,
            ModelImport modelImport,
            List<long> storesId,
            Dictionary<string, string> mapping)
        {
            try
            {
                // open the file "data.csv" which is a CSV file with headers
                using (var csv = new CsvReader(new StreamReader(csvFile.OpenReadStream()),
                           new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = separator.ToString() }))
                {
                    _logger.LogInformation($"CSV file {csvFile.FileName} has been read correctly.");
                    await _notifier.NotifyUpdateAsync(
                        HandlerMessage.New($"CSV file {csvFile.FileName} has been read correctly"));
                    switch (modelImport)
                    {
                        case ModelImport.FAMILY:
                            await familyImport.Process(
                                mapping,
                                csv,
                                storesId
                            );
                            break;
                        case ModelImport.PRODUCT:
                            await productImport.Process(
                                // _provider,
                                mapping,
                                csv,
                                storesId
                                //notify
                            );
                            break;
                        case ModelImport.SCALE_PRODUCT:
                            await scaleProductImport.Process(
                                mapping,
                                csv,
                                storesId
                            );
                            break;
                        case ModelImport.DEPARTMENT:
                            await departmentImport.Process(
                                mapping,
                                csv,
                                storesId
                            );
                            break;
                        case ModelImport.CATEGORY:
                            await categoryImport.Process(
                                mapping,
                                csv,
                                storesId
                            );
                            break;
                        case ModelImport.SCALE_CATEGORY:
                            await scaleCategoryImport.Process(
                                mapping,
                                csv,
                                storesId
                            );
                            break;
                    }
                }
            }
            catch (Exception error)
            {
                await _notifier.NotifyErrorAsync(HandlerMessage.New(
                    "Fail to read data from csv file. Please check the correct format and ensure thaat you select the corret separator"));
                if (error.Message.Contains("You can ignore bad data by setting BadDataFound to null"))
                    throw new BusinessLogicException(
                        "Fail to read data from csv file. Please check the correct format and ensure that you select the correct separator.");
                throw new BusinessLogicException(error.Message);
            }
        }

        /// <summary>
        ///     Get colums name list
        /// </summary>
        /// <param name="csvFile">csv file</param>
        /// <param name="separator">csv separador character</param>
        /// <param name="modelImport">model to import</param>
        /// <returns>Colums list</returns>
        public (List<string>, List<string>, List<string>) GetColumsName(
            IFormFile csvFile, 
            char separator, 
            ModelImport modelImport)
        {
            try
            {
                // open the file "data.csv" which is a CSV file with headers
                using var csv = new CsvReader(new StreamReader(csvFile.OpenReadStream()),
                    new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = separator.ToString() });
                _logger.LogInformation(
                    "CSV file {FileName} has read successfully", csvFile.FileName); // get column names list from csv

                csv.Read();
                csv.ReadHeader();
                var csvHeaders = csv.HeaderRecord.OrderBy(x => x.ToString()).ToList();
                var modelHeaders = new List<string>();
                // get column names list from model
                switch (modelImport)
                {
                    case ModelImport.FAMILY:
                        modelHeaders.AddRange(new Family().GetColumnName());
                        // modelHeaders.AddRange(familyImport.GetColumnName());
                        break;
                    case ModelImport.PRODUCT:
                        var data = new Product().GetColumnName();
                        data.AddRange(new StoreProduct().GetBaseProperties());
                        modelHeaders.AddRange(data);
                        // modelHeaders.AddRange(productImport.GetColumnName());
                        // modelHeaders.AddRange(GetBaseProperties<Product>());
                        // modelHeaders.AddRange(GetBaseProperties<StoreProduct>());
                        break;
                    case ModelImport.SCALE_PRODUCT:
                        var data2 = new ScaleProduct().GetColumnName();
                        data2.AddRange(new StoreProduct().GetBaseProperties());
                        data2.Add("Label_1");
                        data2.Add("Label_2");
                        modelHeaders.AddRange(data2);
                        //modelHeaders.AddRange(scaleProductImport.GetColumnName());
                        // modelHeaders.AddRange(GetBaseProperties<ScaleProduct>());
                        // modelHeaders.AddRange(GetBaseProperties<StoreProduct>());
                        break;
                    case ModelImport.DEPARTMENT:
                        modelHeaders.AddRange(new Department().GetColumnName());
                        // modelHeaders.AddRange(departmentImport.GetColumnName());
                        // modelHeaders.AddRange(GetBaseProperties<Department>());
                        break;
                    case ModelImport.CATEGORY:
                        modelHeaders.AddRange(new Category().GetColumnName());
                        // modelHeaders.AddRange(categoryImport.GetColumnName());
                        // modelHeaders.AddRange(GetBaseProperties<Category>());
                        break;
                    case ModelImport.SCALE_CATEGORY:
                        modelHeaders.AddRange(typeof(Category).GetColumnName());
                        // modelHeaders.AddRange(scaleCategoryImport.GetColumnName());
                        // modelHeaders.AddRange(GetBaseProperties<ScaleCategory>());
                        break;
                }

                //return new List<List<string>> { csvHeaders, modelHeaders.OrderBy(x => x.ToString()).ToList() };
                var head = modelHeaders.OrderBy(x => x.ToString()).Where(x => x != "Id").ToList();
                return (
                    csvHeaders,
                    head,
                    GetSugestionMap(csvHeaders, head).Values.ToList()
                    );
            }
            catch (Exception error)
            {
                if (error.Message.Contains("You can ignore bad data by setting BadDataFound to null"))
                    throw new BusinessLogicException(
                        "Fail to read data from csv file. Please check the correct format and ensure that you select the correct separator.");
                throw new BusinessLogicException(error.Message);
            }
        }
        
        /// <summary>
        ///    Get columns sugestionsname list
        /// </summary>
        /// <param name="csv"></param>
        /// <param name="databaseFields"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetSugestionMap(List<string> csv, List<string> databaseFields)
        {
            var map = new Dictionary<string, string>();
            foreach (var header in csv)
            {
                var normalHeader = header.Normalize();
                string? bestMatch = null;
                var bestDistance = int.MaxValue;

                foreach (var campo in databaseFields)
                {
                    var normalDbField = campo.Normalize();
                    var distance = normalHeader.LevenshteinDistance(normalDbField);

                    if (distance >= bestDistance) continue;
                    bestMatch = campo;
                    bestDistance = distance;
                }

                if (bestMatch == null || !AcceptableDistance(header, bestDistance))
                {
                    map[header] = "NOT USED";
                }
                else
                {
                    map[header] = bestMatch!;
                }
            }
            return map;
        }
        
        private bool AcceptableDistance(string header, int bestDistance)
        {
            if (header.Length > 4)
            {
                return bestDistance <= 4;
            }
            else if (header.Length > 1)
            {
                return bestDistance <= header.Length - 1;
            }
            return false;
        }

        /// <summary>
        ///     Get entity by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Mapping</returns>
        public async Task<CSVMapping?> GetById(long id)
        {
            return await _repository.GetEntity<CSVMapping>().SingleOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        ///     Get entity by ModelImport
        /// </summary>
        /// <param Id="Id">CSVMapping Id</param>
        /// <returns>Mapping List by ModelImport</returns>
        public async Task<List<CSVMapping>> GetByModelImport(ModelImport modelImport)
        {
            return await _repository.GetEntity<CSVMapping>().Where(x => x.ModelImport == modelImport).ToListAsync();
        }

        /// <summary>
        ///     Get entity by name
        /// </summary>
        /// <param name="name">PriceBatch name</param>
        /// <returns>PriceBatch</returns>
        public async Task<CSVMapping> GetByName(string name, long Id = -1)
        {
            if (Id == -1)
                return await _repository.GetEntity<CSVMapping>().Where(x => x.Name == name).FirstOrDefaultAsync();
            return await _repository.GetEntity<CSVMapping>().Where(x => x.Name == name && x.Id != Id)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        ///     Filter and sort list of entities
        /// </summary>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="filter">Filter params</param>
        /// <param name="searchstring">basic searc string</param>
        /// <param name="sortstring">sort string </param>
        /// <returns></returns>
        public async Task<Pager<CSVMapping>> Filter(int currentPage, int pageSize, CSVMapping filter,
            string searchstring, string sortstring, ModelImport modelImport)
        {
            if (currentPage < 1 || pageSize < 1)
            {
                _logger.LogError("Page parameter (currentPage or pageSize) out of bounds");
                throw new BusinessLogicException("Page parameter out of bounds.");
            }

            var splited = string.IsNullOrEmpty(sortstring) ? new[] { "", "" } : sortstring.Split("_");

            var query = _repository.GetEntity<CSVMapping>();
            var query1 = FilterqueryBuilderAsync(filter, searchstring, splited, query, modelImport);

            var entities = await query1.ToPageAsync(currentPage, pageSize);
            return entities;
        }

        // public List<string> GetBaseProperties<TType>()
        // {
        //     return typeof(TType)
        //         .GetProperties()
        //         .Where(x =>
        //             x.PropertyType.IsAssignableFrom(typeof(string))
        //             || !x.PropertyType.IsClass
        //         )
        //         .Where(x => x.Name != "UpdatedAt" && x.Name != "CreatedAt" && x.Name != "UserCreatorId" &&
        //                     x.Name != "Id" && x.Name != "State")
        //         .Map(x => x.Name).ToList();
        // }

        /// <summary>
        ///     Delete mapping
        /// </summary>
        /// <param name="csvHeaderIndex">Header index from csv file</param>
        /// <param name="modelHeader">Header from model</param>
        /// <param name="modelImport">model to import</param>
        /// <returns>Colums list</returns>
        public async Task<bool> Update(string modelHeader, int csvHeaderIndex, ModelImport modelImport)
        {
            var mapp = await _repository.GetEntity<CSVMapping>().FindAsync(new { modelHeader, modelImport });
            return await _repository.UpdateAsync(mapp);
        }

        protected IQueryable<CSVMapping> FilterqueryBuilderAsync(
            CSVMapping filter,
            string searchstring,
            string[] splited,
            DbSet<CSVMapping> query,
            ModelImport modelImport)
        {
            IQueryable<CSVMapping> query1 = null;

            if (!string.IsNullOrEmpty(searchstring))
                query1 = query.Where(c => c.Name.Contains(searchstring) && c.ModelImport == modelImport);
            else
                query1 = query.WhereIf(!string.IsNullOrEmpty(filter.Name),
                    c => c.Name.Contains(filter.Name) && c.ModelImport == modelImport);

            query1 = query1
                .Switch(splited)
                .OrderByCase(e => e[0] == "name" && e[1] == "asc", e => e.Name)
                .OrderByDescendingCase(e => e[0] == "name" && e[1] == "desc", e => e.Name)
                .OrderByDefault(e => e.Name);

            return query1;
        }

        public List<ColumnNameModel> GetColumsNameByModelExport(ModelImport modelImport)
        {
            try
            {
                var result = new List<ColumnNameModel>();
                var columns = new List<string>();
                // get column names list from model
                switch (modelImport)
                {
                    case ModelImport.FAMILY:
                        // columns.AddRange(familyImport.GetAllColumnName());
                        columns.AddRange(new FamilyExportModel().GetAllColumnName());
                        break;
                    case ModelImport.PRODUCT:
                        columns.AddRange(new ProductExportModel().GetAllColumnName());
                        // columns.AddRange(productImport.GetAllColumnName());
                        break;
                    case ModelImport.SCALE_PRODUCT:
                        columns.AddRange(new ScaleProductExportModel().GetAllColumnName());
                        // columns.AddRange(scaleProductImport.GetAllColumnName());
                        break;
                    case ModelImport.DEPARTMENT:
                        columns.AddRange(new DepartmentExportModel().GetAllColumnName());
                        // columns.AddRange(departmentImport.GetAllColumnName());
                        break;
                    case ModelImport.CATEGORY:
                        columns.AddRange(new CategoryExportModel().GetAllColumnName());
                        // columns.AddRange(categoryImport.GetAllColumnName());
                        break;
                    case ModelImport.SCALE_CATEGORY:
                        columns.AddRange(new ScaleCategoryExportModel().GetAllColumnName());
                        // columns.AddRange(scaleCategoryImport.GetAllColumnName());
                        break;
                }

                foreach (var item in columns)
                {
                    if (item == "Id")
                    {
                        result.Insert(0, new ColumnNameModel() { Exported = true, Name = item });
                    }
                    else
                    {
                        result.Add(new ColumnNameModel() { Exported = true, Name = item });
                    }
                }

                return result;
            }
            catch (Exception error)
            {
                throw new BusinessLogicException(error.Message);
            }
        }

        public async Task<Pager<ColumnNameModel>> FilterColumnName(int currentPage, int pageSize,
            ColumnNameModel filter, string searchstring, string sortstring, ModelImport modelImport)
        {
            if (currentPage < 1 || pageSize < 1)
            {
                _logger.LogError("Page parameter (currentPage or pageSize) out of bounds.");
                throw new BusinessLogicException("Page parameter out of bounds.");
            }

            var splited = string.IsNullOrEmpty(sortstring) ? new[] { "", "" } : sortstring.Split("_");
            var columns = new List<string>();

            IQueryable<ColumnNameModel> query1 = null;
            //Get PropertyList By Model
            switch (modelImport)
            {
                case ModelImport.FAMILY:
                    // columns.AddRange(familyImport.GetAllColumnName());
                    columns.AddRange(new FamilyExportModel().GetAllColumnName());
                    query1 = familyImport.FilterqueryBuilderAsync(filter, searchstring, splited,
                        GetResultColumnNameModel(columns));
                    break;
                case ModelImport.PRODUCT:
                    //columns.AddRange(productImport.GetAllColumnName());
                    columns.AddRange(new ProductExportModel().GetAllColumnName());
                    query1 = productImport.FilterqueryBuilderAsync(filter, searchstring, splited,
                        GetResultColumnNameModel(columns));
                    break;
                case ModelImport.SCALE_PRODUCT:
                    //columns.AddRange(scaleProductImport.GetAllColumnName());
                    columns.AddRange(new ScaleProductExportModel().GetAllColumnName());
                    query1 = scaleProductImport.FilterqueryBuilderAsync(filter, searchstring, splited,
                        GetResultColumnNameModel(columns));

                    break;
                case ModelImport.DEPARTMENT:
                    columns.AddRange(new DepartmentExportModel().GetAllColumnName());
                    // columns.AddRange(departmentImport.GetAllColumnName());
                    query1 = departmentImport.FilterqueryBuilderAsync(filter, searchstring, splited,
                        GetResultColumnNameModel(columns));

                    break;
                case ModelImport.CATEGORY:
                    columns.AddRange(new CategoryExportModel().GetAllColumnName());
                    //columns.AddRange(categoryImport.GetAllColumnName());
                    query1 = categoryImport.FilterqueryBuilderAsync(filter, searchstring, splited,
                        GetResultColumnNameModel(columns));

                    break;
                case ModelImport.SCALE_CATEGORY:
                    columns.AddRange(new ScaleProductExportModel().GetAllColumnName());
                    //columns.AddRange(scaleCategoryImport.GetAllColumnName());
                    query1 = scaleCategoryImport.FilterqueryBuilderAsync(filter, searchstring, splited,
                        GetResultColumnNameModel(columns));

                    break;
            }

            var result = query1.Skip(pageSize * (currentPage - 1)).Take(pageSize);
            var pager = new Pager<ColumnNameModel>(query1.ToList().Count, result.ToList(), currentPage, pageSize, 10);


            return await Task.FromResult(pager);
        }

        private DbSet<ColumnNameModel> GetResultColumnNameModel(List<string> columns)
        {
            var result = new List<ColumnNameModel>();
            foreach (var item in columns)
            {
                result.Add(new ColumnNameModel() { Exported = true, Name = item });
            }


            var queryable = result.AsQueryable();

            var dbSet = new Mock<DbSet<ColumnNameModel>>();
            dbSet.As<IQueryable<ColumnNameModel>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<ColumnNameModel>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<ColumnNameModel>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<ColumnNameModel>>().Setup(m => m.GetEnumerator())
                .Returns(() => queryable.GetEnumerator());

            return dbSet.Object;
        }
    }
}