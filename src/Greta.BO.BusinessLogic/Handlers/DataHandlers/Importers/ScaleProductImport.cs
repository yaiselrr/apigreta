#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CsvHelper;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Greta.BO.BusinessLogic.Service;
using System.Linq;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Abstractions;
using Greta.BO.BusinessLogic.TypeConverters;
using Microsoft.EntityFrameworkCore;
using Greta.BO.BusinessLogic.Handlers.DataHandlers.Validations;
using Greta.BO.BusinessLogic.Handlers.DataHandlers.Validations.Extensions;

namespace Greta.BO.BusinessLogic.Handlers.DataHandlers.Importers
{
    public class ScaleProductImport : BaseImport<ScaleProduct>
    {
        public ScaleProductImport(ILogger<ScaleProductImport> logger, IServiceProvider provider,
            ILogger inLogger, INotifier notifier)
            : base(logger, provider, inLogger, notifier)
        {
        }

        public override async Task Process(Dictionary<string, string> mapping, CsvReader csv, List<long> storesIds)
        {
            var scaleProductRepository = Provider.GetRequiredService<IScaleProductRepository>();
            var productService = Provider.GetRequiredService<IProductService>();
            var departmentService = Provider.GetRequiredService<IDepartmentService>();
            var categoryService = Provider.GetRequiredService<ICategoryService>();
            var scaleCategoryService = Provider.GetRequiredService<IScaleCategoryService>();
            var storeProductService = Provider.GetRequiredService<IStoreProductService>();
            var labelDefinitionService = Provider.GetRequiredService<IScaleLabelDefinitionService>();
            List<string> errors = new();
            _currentRow = 0;
            _totalRows = 0;
            _insertedRows = 0;
            _updatedRows = 0;
            _failedRows = 0;
            _processedRows = 0;
            var validations = new ValidationPipeline<ScaleProduct>(_notifier)
                .Add<ScaleProductValidationName>()
                .Add<ScaleProductValidationDescription>()
                .Add<ScaleProductValidationCategoryId>()
                .Add<ScaleProductValidationDepartmentId>()
                .Add<ScaleProductValidationUPC>()
                .Add<ScaleProductValidationUPCLength>();
            var scaleProducts = Mapper(mapping, csv, storesIds);
            _totalRows = scaleProducts.Count;
            Notify("Initializing");

            var scaleCategoriesCache = new Dictionary<long, ScaleCategory>();
            var categoriesCache = new Dictionary<long, Category>();
            var labelsCache = new Dictionary<int, long>();
            var departmentCache = new Dictionary<long, long>();

            for (var i = 0; i < scaleProducts.Count; i++)
            {
                _currentRow++;
                var currentScaleProduct = scaleProducts[i];

                var validation =
                    await validations.Execute(scaleProducts[i], BuildMessage(), mapping, currentScaleProduct);
                if (!validation.IsValid)
                {
                    _failedRows++;
                    // errors.Add(((HandlerMessage)validation.Msg!).Message);
                    var errorLog = ((HandlerMessage)validation.Msg!).Message;
                    Notify($"line{_currentRow}: {errorLog}", true);
                }
                else
                {
                    var scaleProduct = await scaleProductRepository.GetByUPC(scaleProducts[i].UPC);
                    if (scaleProduct == null)
                    {
                        // await scaleProductRepository.TransactionAsync(async context =>
                        // {
                        var department = departmentCache.GetOrAddFromCache(currentScaleProduct.DepartmentId,
                            async (id) => await departmentService.GetByDepartmentId((int)id),
                            (source) =>
                            {
                                _failedRows++;
                                Notify($"line{_currentRow}: {source.ErrorMessage}", true);
                            });
                        if (department > 0)
                        {
                            var category = categoriesCache.GetOrAddFromCache(currentScaleProduct.CategoryId,
                                async (id) => await categoryService.GetByCategoryId((int)id),
                                (source) =>
                                {
                                    _failedRows++;
                                    Notify($"line{_currentRow}: {source.ErrorMessage}", true);
                                });
                            if (category != null)
                            {
                                var scaleCategory = scaleCategoriesCache.GetOrAddFromCache<ScaleCategory>(
                                    currentScaleProduct.ScaleCategoryId,
                                    async (id) => await scaleCategoryService.GetByScaleCategoryId((int)id),
                                    (source) =>
                                    {
                                        _failedRows++;
                                        Notify($"line{_currentRow}: {source.ErrorMessage}", true);
                                    });

                                if (scaleCategory != null)
                                {
                                    currentScaleProduct.PopulateIfNotMapped(mapping, category, department,
                                        scaleCategory);

                                    //process label
                                    if (currentScaleProduct.ScaleLabelDefinitions != null &&
                                        currentScaleProduct.ScaleLabelDefinitions.Count > 0)
                                    {
                                        var realLabelDefinition =
                                            await ObtainRealLabelDefinition(
                                                currentScaleProduct.ScaleLabelDefinitions, labelsCache,
                                                scaleProductRepository);
                                        // if this have data then add to object
                                        currentScaleProduct.ScaleLabelDefinitions = realLabelDefinition;
                                    }

                                    currentScaleProduct.ProductType = ProductType.SLP;

                                    var stores = currentScaleProduct.StoreProducts.ToList();
                                    currentScaleProduct.StoreProducts.Clear();

                                    var labels = currentScaleProduct.ScaleLabelDefinitions == null
                                        ? null
                                        : currentScaleProduct.ScaleLabelDefinitions.ToList();
                                    currentScaleProduct.ScaleLabelDefinitions?.Clear();

                                    var np = await productService.CreateScaleProduct(
                                        currentScaleProduct);

                                    foreach (var s in stores)
                                    {
                                        s.ProductId = np.Id;
                                        await storeProductService.PostImport(s, np);
                                    }

                                    if (labels != null)
                                    {
                                        foreach (var l in labels)
                                        {
                                            l.ScaleProductId = np.Id;
                                            await labelDefinitionService.Post(l);
                                        }
                                    }

                                    _insertedRows++;
                                    // return true;
                                }
                            }
                        }

                        // return false;
                        // }, IsolationLevel.RepeatableRead);
                    }
                    else
                    {
                        scaleProducts[i].Id = scaleProduct.Id;
                        scaleProducts[i].UserCreatorId = scaleProduct.UserCreatorId;
                        scaleProducts[i].StoreProducts.ForEach(e =>
                        {
                            //get original storeProduct
                            var storeProduct =
                                scaleProduct.StoreProducts.FirstOrDefault(r =>
                                    r.StoreId == e.StoreId);
                            if (storeProduct != null)
                            {
                                e.Id = storeProduct.Id;
                                e.ProductId = scaleProduct.Id;
                                e.UserCreatorId = storeProduct.UserCreatorId;
                            }
                        });

                        // await scaleProductRepository.TransactionAsync(async context =>
                        // {
                        var department = departmentCache.GetOrAddFromCache(currentScaleProduct.DepartmentId,
                            async (id) => await departmentService.GetByDepartmentId((int)id),
                            (source) =>
                            {
                                _failedRows++;
                                Notify($"line{_currentRow}: {source.ErrorMessage}", true);
                            });

                        if (department > 0)
                        {
                            //get department from cache if exist
                            var category = categoriesCache.GetOrAddFromCache(currentScaleProduct.CategoryId,
                                async (id) => await categoryService.GetByCategoryId((int)id),
                                (source) =>
                                {
                                    _failedRows++;
                                    Notify($"line{_currentRow}: {source.ErrorMessage}", true);
                                });

                            if (category != null)
                            {
                                var scaleCategory = scaleCategoriesCache.GetOrAddFromCache(
                                    currentScaleProduct.ScaleCategoryId,
                                    async (id) => await scaleCategoryService.GetByScaleCategoryId((int)id),
                                    (source) =>
                                    {
                                        _failedRows++;
                                        Notify($"line{_currentRow}: {source.ErrorMessage}", true);
                                    });
                                if (scaleCategory != null)
                                {
                                    currentScaleProduct.PopulateIfNotMapped(mapping, category, department,
                                        scaleCategory);

                                    //process label
                                    if (currentScaleProduct.ScaleLabelDefinitions is { Count: > 0 })
                                    {
                                        var realLabelDefinition =
                                            await ObtainRealLabelDefinition(
                                                currentScaleProduct.ScaleLabelDefinitions, labelsCache,
                                                scaleProductRepository);
                                        if (realLabelDefinition.Count > 0)
                                        {
                                            var currentDefinition = scaleProduct.ScaleLabelDefinitions
                                                .FirstOrDefault(x => x.ScaleBrandId == 1 &&
                                                                     (x.ScaleLabelType1Id ==
                                                                      realLabelDefinition[0].ScaleLabelType1Id));
                                            if (currentDefinition != null)
                                            {
                                                realLabelDefinition[0].Id = currentDefinition.Id;
                                                realLabelDefinition[0].UserCreatorId =
                                                    currentDefinition.UserCreatorId;
                                            }

                                            // if this have data then add to object
                                            currentScaleProduct.ScaleLabelDefinitions = realLabelDefinition;
                                        }
                                    }

                                    currentScaleProduct.ProductType = ProductType.SLP;

                                    var np = await productService.UpdateScaleProduct(scaleProduct.Id,
                                        currentScaleProduct);

                                    var saved = await scaleProductRepository.GetEntity<ScaleProduct>()
                                        .Include(x => x.ScaleLabelDefinitions)
                                        .Where(x => x.Id == scaleProduct.Id).FirstOrDefaultAsync();

                                    _updatedRows++;
                                    // return true;
                                }
                            }
                        }

                        // return false;
                        // }, IsolationLevel.RepeatableRead);
                    }

                    Notify();
                }
            }

            var msg = BuildMessage();
            msg.Stage = Stage.Completed;
            NotifyUpdate(msg);
        }

        private async Task<List<ScaleLabelDefinition>> ObtainRealLabelDefinition(
            IReadOnlyList<ScaleLabelDefinition> definitions, Dictionary<int, long> labelsCache,
            IScaleProductRepository scaleProductRepository)
        {
            long label1 = -1;
            long label2 = -1;

            label1 = await GetIdFromLabelId(definitions[0].ImportLabel, labelsCache, scaleProductRepository);

            if (definitions.Count > 1)
            {
                label2 = await GetIdFromLabelId(definitions[1].ImportLabel, labelsCache, scaleProductRepository);
            }

            if (label1 != -1)
            {
                return new List<ScaleLabelDefinition>()
                {
                    new ScaleLabelDefinition()
                    {
                        ScaleLabelType1Id = label1,
                        ScaleLabelType2Id = label2 == -1 ? label1 : label2,
                        ScaleBrandId = 1,
                        State = true
                    }
                };
            }

            return new List<ScaleLabelDefinition>();
        }

        private async Task<long> GetIdFromLabelId(int labelName,
            Dictionary<int, long> labelsCache,
            IScaleProductRepository scaleProductRepository)
        {
            if (labelName != -1)
            {
                if (labelsCache.TryGetValue(labelName, out long sclt))
                {
                    return sclt;
                }

                var sclt1 = await scaleProductRepository.GetEntity<ScaleLabelType>()
                    .Where(x => x.LabelId == labelName || x.Name == labelName.ToString())
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();

                if (sclt1 > 0)
                {
                    labelsCache.Add(labelName, sclt1);
                    return sclt1;
                }
            }

            return -1;
        }

        public List<ScaleProduct> Mapper(Dictionary<string, string> mapping, CsvReader csv, List<long> storesId)
        {
            var products = new List<ScaleProduct>();
            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                var scaleProduct = new ScaleProduct();
                scaleProduct.State = true;
                var propsProduct = scaleProduct.GetBaseProperties();

                foreach (var p in propsProduct)
                    if (p == "PLUType" && mapping.ContainsValue("PLUType"))
                    {
                        //process if is string or int
                        try
                        {
                            var intValue = csv.GetField(typeof(int),
                                mapping.FirstOrDefault(x => x.Value == "PLUType").Key);
                            scaleProduct.PLUType = PluType.RandomWeight;
                            switch (intValue)
                            {
                                case 1:
                                    scaleProduct.PLUType = PluType.FixedWeight;
                                    break;
                                case 2:
                                    scaleProduct.PLUType = PluType.ByCount;
                                    break;
                            }
                        }
                        catch
                        {
                            var stringValue = csv.GetField(typeof(string),
                                mapping.FirstOrDefault(x => x.Value == "PLUType").Key);
                            scaleProduct.PLUType = PluType.RandomWeight;
                            switch (stringValue)
                            {
                                case "Fixed Weight":
                                    scaleProduct.PLUType = PluType.FixedWeight;
                                    break;
                                case "By Count":
                                    scaleProduct.PLUType = PluType.ByCount;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        if (scaleProduct.GetType().GetProperty(p).PropertyType.IsAssignableFrom(typeof(bool)))
                        {
                            var defV = p == "ScaleVisible" ? true : false;
                            var value = mapping.GetCsValue<bool, BooleanConverter>(csv, p, false);
                            scaleProduct.GetType().GetProperty(p).SetValue(scaleProduct, value, null);
                        }
                        else
                        {
                            var value = mapping.GetCsValue(csv, scaleProduct.GetType().GetProperty(p).PropertyType, p,
                                null);
                            scaleProduct.GetType().GetProperty(p)?.SetValue(scaleProduct, value, null);
                        }
                    }

                if (scaleProduct.PLUNumber == 0 && scaleProduct.UPC != null && scaleProduct.UPC.Length == 5)
                {
                    try
                    {
                        scaleProduct.PLUNumber = int.Parse(scaleProduct.UPC);
                    }
                    catch
                    {
                    }
                }

                //if (scaleProduct.DepartmentId == 0) scaleProduct.DepartmentId = 1;

                if (mapping.ContainsValue("Label_1"))
                {
                    try
                    {
                        //create and asing the label definition
                        var value = mapping.GetCsValue<int>(csv, "Label_1", -1);

                        if (value != -1)
                        {
                            scaleProduct.ScaleLabelDefinitions = new List<ScaleLabelDefinition>()
                            {
                                new ScaleLabelDefinition()
                                {
                                    ImportLabel = value
                                }
                            };
                        }
                    }
                    catch //(Exception labExp)
                    {
                        // ignored
                    }
                }

                if (mapping.ContainsValue("Label_2"))
                {
                    try
                    {
                        //create and asign the label definition
                        var value = mapping.GetCsValue<int>(csv, "Label_2", -1);

                        if (value != -1)
                        {
                            if (scaleProduct.ScaleLabelDefinitions == null)
                            {
                                scaleProduct.ScaleLabelDefinitions = new List<ScaleLabelDefinition>();
                            }

                            scaleProduct.ScaleLabelDefinitions.Add(
                                new ScaleLabelDefinition()
                                {
                                    ImportLabel = value
                                }
                            );
                        }
                    }
                    catch //(Exception labExp)
                    {
                        // ignored
                    }
                }


                //storeProduct
                //adding store products to product
                storesId.ForEach(id =>
                {
                    scaleProduct.StoreProducts.Add(new StoreProduct
                    {
                        StoreId = id,
                        State = true,
                        // BinLocation = !mapping.ContainsValue(nameof(StoreProduct.BinLocation))
                        // ? null
                        // : csv.GetField<string>(mapping.FirstOrDefault(x => x.Value == nameof(StoreProduct.BinLocation))
                        //     .Key),
                        Cost = !mapping.ContainsValue(nameof(StoreProduct.Cost))
                            ? 0
                            : csv.GetField<decimal>(mapping.FirstOrDefault(x => x.Value == nameof(StoreProduct.Cost))
                                .Key),
                        GrossProfit = !mapping.ContainsValue(nameof(StoreProduct.GrossProfit))
                            ? 0
                            : csv.GetField<decimal>(mapping
                                .FirstOrDefault(x => x.Value == nameof(StoreProduct.GrossProfit))
                                .Key),
                        Price = !mapping.ContainsValue(nameof(StoreProduct.Price))
                            ? 0
                            : csv.GetField<decimal>(mapping.FirstOrDefault(x => x.Value == nameof(StoreProduct.Price))
                                .Key),
                        ProductId = !mapping.ContainsValue(nameof(StoreProduct.ProductId))
                            ? 0
                            : csv.GetField<long>(mapping.FirstOrDefault(x => x.Value == nameof(StoreProduct.ProductId))
                                .Key)
                    });
                });

                products.Add(scaleProduct);
            }

            return products;
        }
    }
}