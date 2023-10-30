using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3.Model;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Entities.Events.Internal.Products;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Specifications.ScaleProductSpecs;
using Greta.Sdk.Core.Models.Pager;
using Greta.Sdk.EFCore.Extensions;
using Greta.Sdk.ExternalScale.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service
{
    public interface IProductService : IBaseService
    {
        Task<List<Product>> Get();
        Task<List<ScaleProduct>> GetScaleProduct();

        Task<Pager<ScaleProduct>> GetScaleProductsByUpcPluProduct(long cutTemplateId, int currentPage, int pageSize, ScaleProductSearchModel filter);

        Task<List<ScaleProduct>> GetScaleProductsByTemplate(long TemplateId);
        Task<List<ScaleProduct>> GetScaleProductsByUpcOrPlu(string filter);

        Task<Pager<Product>> Filter(int currentPage, int pageSize, ProductSearchModel filter, string searchstring,
            string sortstring);

        Task<Pager<Product>> FilterByStore(long storeId, int currentPage, int pageSize, Product filter,
            string searchstring, string sortstring);

        Task<Pager<Product>> FilterByBatch(long batchId, int currentPage, int pageSize, Product filter,
            string searchstring, string sortstring);

        Task<Pager<Product>> FilterByFamily(long familyId, int currentPage, int pageSize, Product filter,
            string searchstring, string sortstring);

        Task<Pager<Product>> FilterNotByFamily(int currentPage, int pageSize, Product filter,
            string searchstring, string sortstring);

        Task<Product> CreateProduct(Product product);
        Task<ScaleProduct> CreateScaleProduct(ScaleProduct product);
        Task<KitProduct> CreateKitProduct(KitProduct product);

        Task<Product> GetProductById(long id);
        Task<ScaleProduct> GetScaleProductById(long id);
        Task<KitProduct> GetKitProductById(long id);
        Task<Product> GetProductByIdWithDefaultShelfTag(long id);

        Task<Product> GetProductByPLU(int plu, long productID = -1);
        Task<Product> GetProductByUPC(string upc, long productID = -1);

        Task<Product> GetProductByUPCWithStoreAndVendor(string upc, long productID = -1);
        Task<Product> GetProductByName(string name, long productID = -1);

        Task<Product> GetProductByPLUNumber(int plu, long productID = -1);
        Task<bool> ChangeState(long id, bool state);
        Task<bool> Delete(long id);
        Task<bool> DeleteRange(List<long> ids);
        Task<bool> UpdateProduct(long productid, Product entity);
        Task<bool> UpdateScaleProduct(long productid, ScaleProduct entity);
        Task<bool> UpdateKitProduct(long productid, KitProduct entity);
        Task UpdateProductOnline(long id);
        Task<List<Product>> GetProductsByStoreId(long storeId);
        Task<List<ScaleProduct>> GetScaleProductsByStoreId(long storeId);

        Task<List<PLUModel>> GetUpdatesForScales(long storeId, DateTime last,
            List<long> deps);

        Task<List<PLUModel>> GetAllForScales(long storeId, List<long> deps);
        bool isNumber(string stringToVerify);
    }

    public class ProductService : IProductService
    {
        private readonly IKitProductRepository _kitProductRepository;
        private readonly ILogger _logger;

        private readonly IImageRepository _pimageRepository;
        private readonly ISynchroService synchroService;
        private readonly IProductRepository _productRepository;
        private readonly IScaleProductRepository _scaleProductRepository;
        private readonly IShelfTagService _shelfTagService;
        private readonly IMediator _mediator;

        public ProductService(
            ILogger<ProductService> logger,
            IProductRepository productRepository,
            IKitProductRepository kitProductRepository,
            IScaleProductRepository scaleProductRepository,
            IImageRepository pimageRepository,
            IShelfTagService shelfTagService,
            ISynchroService synchroService,
            IMediator mediator)
        {
            _logger = logger;
            _pimageRepository = pimageRepository;
            this.synchroService = synchroService;
            _productRepository = productRepository;
            _kitProductRepository = kitProductRepository;
            _shelfTagService = shelfTagService;
            _scaleProductRepository = scaleProductRepository;
            _mediator = mediator;
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
        public async Task<Pager<Product>> Filter(int currentPage, int pageSize, ProductSearchModel filter,
            string searchstring,
            string sortstring)
        {
            var splited = string.IsNullOrEmpty(sortstring) ? new[] { "", "" } : sortstring.Split("_");

            var query = _productRepository.GetEntity<Product>()
                .Include(x => x.Department)
                .Include(x => x.Category)
                .Include(x => x.Family).IgnoreAutoIncludes();

            if (!string.IsNullOrEmpty(searchstring))
                query = query.Where(c => c.Name.Contains(searchstring)
                                         || c.UPC.Contains(searchstring)
                );
            else
                query = query.WhereIf(!string.IsNullOrEmpty(filter.UPC), c => c.UPC.Contains(filter.UPC))
                    .WhereIf(!string.IsNullOrEmpty(filter.Name), c => c.Name.Contains(filter.Name))
                    .WhereIf(filter.DepartmentId > 0, c => c.DepartmentId == filter.DepartmentId)
                    .WhereIf(filter.CategoryId > 0, c => c.CategoryId == filter.CategoryId)
                    .WhereIf(filter.ProductType > 0, c => c.ProductType == filter.ProductType)
                    .WhereIf(filter.ProductTypeExcept > -1, c => c.ProductType != (ProductType)filter.ProductTypeExcept)
                    .WhereIf(filter.FamilyId > 0, c => c.FamilyId == filter.FamilyId);

            query = query.Where(e => e.State == filter.State);

            query = query
                .Switch(splited)
                .OrderByCase(e => e[0] == "upc" && e[1] == "asc", e => e.UPC)
                .OrderByDescendingCase(e => e[0] == "upc" && e[1] == "desc", e => e.UPC)
                .OrderByCase(e => e[0] == "name" && e[1] == "asc", e => e.Name)
                .OrderByDescendingCase(e => e[0] == "name" && e[1] == "desc", e => e.Name)
                .OrderByCase(e => e[0] == "departmentid" && e[1] == "asc", e => e.Department.Name)
                .OrderByDescendingCase(e => e[0] == "departmentid" && e[1] == "desc", e => e.Department.Name)
                .OrderByCase(e => e[0] == "categoryid" && e[1] == "asc", e => e.Category.Name)
                .OrderByDescendingCase(e => e[0] == "categoryid" && e[1] == "desc", e => e.Category.Name)
                .OrderByCase(e => e[0] == "familyid" && e[1] == "asc", e => e.Family.Name)
                .OrderByDescendingCase(e => e[0] == "familyid" && e[1] == "desc", e => e.Family.Name)
                .OrderByCase(e => e[0] == "productType" && e[1] == "asc", e => e.ProductType)
                .OrderByDescendingCase(e => e[0] == "productType" && e[1] == "desc", e => e.ProductType)
                .OrderByDefault(e => e.Name);

            var entities = await query.ToPageAsync(currentPage, pageSize);
            return entities;
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
        public async Task<Pager<Product>> FilterByStore(long storeId, int currentPage, int pageSize, Product filter,
            string searchstring, string sortstring)
        {
            if (currentPage < 1 || pageSize < 1)
            {
                _logger.LogError("Page parameter (currentPage or pageSize) out of bounds.");
                throw new BusinessLogicException("Page parameter out of bounds.");
            }

            var splited = string.IsNullOrEmpty(sortstring) ? new[] { "", "" } : sortstring.Split("_");

            var query = _productRepository.GetEntity<StoreProduct>()
                .Where(x => x.StoreId == storeId)
                .Include(x => x.Product.Department)
                .Include(x => x.Product.Category)
                .Include(x => x.Product.Family).IgnoreAutoIncludes()
                .Select(x => x.Product);

            if (!string.IsNullOrEmpty(searchstring))
                query = query.Where(c => c.Name.Contains(searchstring)
                                         || c.UPC.Contains(searchstring)
                );
            else
                query = query.WhereIf(!string.IsNullOrEmpty(filter.UPC), c => c.UPC.Contains(filter.UPC))
                    .WhereIf(!string.IsNullOrEmpty(filter.Name), c => c.Name.Contains(filter.Name))
                    .WhereIf(filter.DepartmentId > 0, c => c.DepartmentId == filter.DepartmentId)
                    .WhereIf(filter.CategoryId > 0, c => c.CategoryId == filter.CategoryId)
                    .WhereIf(filter.ProductType > 0, c => c.ProductType == filter.ProductType)
                    .WhereIf(filter.FamilyId > 0, c => c.FamilyId == filter.FamilyId);

            query = query.Where(e => e.State == filter.State);

            query = query
                .Switch(splited)
                .OrderByCase(e => e[0] == "upc" && e[1] == "asc", e => e.UPC)
                .OrderByDescendingCase(e => e[0] == "upc" && e[1] == "desc", e => e.UPC)
                .OrderByCase(e => e[0] == "name" && e[1] == "asc", e => e.Name)
                .OrderByDescendingCase(e => e[0] == "name" && e[1] == "desc", e => e.Name)
                .OrderByCase(e => e[0] == "departmentid" && e[1] == "asc", e => e.Department.Name)
                .OrderByDescendingCase(e => e[0] == "departmentid" && e[1] == "desc", e => e.Department.Name)
                .OrderByCase(e => e[0] == "categoryid" && e[1] == "asc", e => e.Category.Name)
                .OrderByDescendingCase(e => e[0] == "categoryid" && e[1] == "desc", e => e.Category.Name)
                .OrderByCase(e => e[0] == "familyid" && e[1] == "asc", e => e.Family.Name)
                .OrderByDescendingCase(e => e[0] == "familyid" && e[1] == "desc", e => e.Family.Name)
                .OrderByDefault(e => e.Name);

            var entities = await query.ToPageAsync(currentPage, pageSize);
            return entities;
        }

        /// <summary>
        ///     Filter and sort list of entities
        /// </summary>
        /// <param name="batchId">Batch id</param>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="filter">Filter params</param>
        /// <param name="searchstring">basic searc string</param>
        /// <param name="sortstring">sort string </param>
        /// <returns></returns>
        public async Task<Pager<Product>> FilterByBatch(long batchId, int currentPage, int pageSize, Product filter,
            string searchstring, string sortstring)
        {
            if (currentPage < 1 || pageSize < 1)
            {
                _logger.LogError("Page parameter (currentPage or pageSize) out of bounds.");
                throw new BusinessLogicException("Page parameter out of bounds.");
            }

            var splited = string.IsNullOrEmpty(sortstring) ? new[] { "", "" } : sortstring.Split("_");

            var batch = await _productRepository.GetEntity<Batch>()
                .Where(x => x.Id == batchId)
                .Include(x => x.Stores)
                .FirstOrDefaultAsync();

            var stores = batch.Stores.Select(x => x.Id).ToList();

            var query = _productRepository.GetEntity<StoreProduct>()
                .Where(x => stores.Contains(x.StoreId))
                .Include(x => x.Product.Department)
                .Include(x => x.Product.Category)
                .Include(x => x.Product.Family).IgnoreAutoIncludes()
                .Select(x => x.Product).Distinct<Product>();

            if (!string.IsNullOrEmpty(searchstring))
                query = query.Where(c => c.Name.Contains(searchstring)
                                         || c.UPC.Contains(searchstring) || ((c is ScaleProduct) &&
                                                                             ((ScaleProduct)c).PLUNumber.ToString()
                                                                             .Contains((searchstring)))
                );
            else
                query = query.WhereIf(!string.IsNullOrEmpty(filter.UPC),
                        c => c.UPC.Contains(filter.UPC) || ((c is ScaleProduct) &&
                                                            ((ScaleProduct)c).PLUNumber.ToString()
                                                            .Contains((filter.UPC))))
                    .WhereIf(!string.IsNullOrEmpty(filter.Name), c => c.Name.Contains(filter.Name))
                    .WhereIf(filter.DepartmentId > 0, c => c.DepartmentId == filter.DepartmentId)
                    .WhereIf(filter.CategoryId > 0, c => c.CategoryId == filter.CategoryId)
                    .WhereIf(filter.ProductType > 0, c => c.ProductType == filter.ProductType)
                    .WhereIf(filter.FamilyId > 0, c => c.FamilyId == filter.FamilyId);

            query = query.Where(e => e.State == filter.State);

            query = query
                .Switch(splited)
                .OrderByCase(e => e[0] == "upc" && e[1] == "asc", e => e.UPC)
                .OrderByDescendingCase(e => e[0] == "upc" && e[1] == "desc", e => e.UPC)
                .OrderByCase(e => e[0] == "name" && e[1] == "asc", e => e.Name)
                .OrderByDescendingCase(e => e[0] == "name" && e[1] == "desc", e => e.Name)
                .OrderByCase(e => e[0] == "departmentid" && e[1] == "asc", e => e.Department.Name)
                .OrderByDescendingCase(e => e[0] == "departmentid" && e[1] == "desc", e => e.Department.Name)
                .OrderByCase(e => e[0] == "categoryid" && e[1] == "asc", e => e.Category.Name)
                .OrderByDescendingCase(e => e[0] == "categoryid" && e[1] == "desc", e => e.Category.Name)
                .OrderByCase(e => e[0] == "familyid" && e[1] == "asc", e => e.Family.Name)
                .OrderByDescendingCase(e => e[0] == "familyid" && e[1] == "desc", e => e.Family.Name)
                .OrderByDefault(e => e.Name);

            var entities = await query.ToPageAsync(currentPage, pageSize);
            return entities;
        }

        /// <summary>
        ///     Filter and sort list of entities
        /// </summary>
        /// <param name="familyId">Family id</param>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="filter">Filter params</param>
        /// <param name="searchstring">basic searc string</param>
        /// <param name="sortstring">sort string </param>
        /// <returns></returns>
        public async Task<Pager<Product>> FilterByFamily(long familyId, int currentPage, int pageSize, Product filter,
            string searchstring, string sortstring)
        {
            if (currentPage < 1 || pageSize < 1)
            {
                _logger.LogError("Page parameter (currentPage or pageSize) out of bounds.");
                throw new BusinessLogicException("Page parameter out of bounds.");
            }

            var splited = string.IsNullOrEmpty(sortstring) ? new[] { "", "" } : sortstring.Split("_");

            var query = _productRepository.GetEntity<Product>()
                .Include(x => x.Family).IgnoreAutoIncludes()
                .Distinct<Product>();

            if (!string.IsNullOrEmpty(searchstring))
                query = query.Where(c => c.Name.Contains(searchstring)
                                         || c.UPC.Contains(searchstring)
                );
            else
                query = query.WhereIf(!string.IsNullOrEmpty(filter.UPC), c => c.UPC.Contains(filter.UPC))
                    .WhereIf(!string.IsNullOrEmpty(filter.Name), c => c.Name.Contains(filter.Name))
                    .WhereIf(filter.ProductType > 0, c => c.ProductType == filter.ProductType)
                    .WhereIf(filter.FamilyId > 0, c => c.FamilyId == filter.FamilyId)
                    .WhereIf(familyId > 0, c => c.FamilyId == familyId);

            query = query.Where(e => e.State == filter.State);

            query = query
                .Switch(splited)
                .OrderByCase(e => e[0] == "upc" && e[1] == "asc", e => e.UPC)
                .OrderByDescendingCase(e => e[0] == "upc" && e[1] == "desc", e => e.UPC)
                .OrderByCase(e => e[0] == "name" && e[1] == "asc", e => e.Name)
                .OrderByDescendingCase(e => e[0] == "name" && e[1] == "desc", e => e.Name)
                .OrderByCase(e => e[0] == "familyid" && e[1] == "asc", e => e.Family.Name)
                .OrderByDescendingCase(e => e[0] == "familyid" && e[1] == "desc", e => e.Family.Name)
                .OrderByDefault(e => e.Name);

            var entities = await query.ToPageAsync(currentPage, pageSize);
            return entities;
        }

        /// <summary>
        /// Filter and sort list of entities that do not belong to the selected family
        /// </summary>
        /// <param name="currentPage">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="filter">Filter params</param>
        /// <param name="searchstring">basic searc string</param>
        /// <param name="sortstring">sort string </param>
        /// <returns></returns>
        public async Task<Pager<Product>> FilterNotByFamily(int currentPage, int pageSize, Product filter,
            string searchstring, string sortstring)
        {
            if (currentPage < 1 || pageSize < 1)
            {
                _logger.LogError("Page parameter (currentPage or pageSize) out of bounds");
                throw new BusinessLogicException("Page parameter out of bounds.");
            }

            var splited = string.IsNullOrEmpty(sortstring) ? new[] { "", "" } : sortstring.Split("_");

            var query = _productRepository.GetEntity<Product>()
                //.Include(x => x.Family).IgnoreAutoIncludes()
                .Distinct<Product>();

            if (!string.IsNullOrEmpty(searchstring))
                query = query.Where(c => (c.Name.Contains(searchstring) || c.UPC.Contains(searchstring))
                                         && c.FamilyId != filter.FamilyId);
            else
                query = query.WhereIf(!string.IsNullOrEmpty(filter.UPC), c => c.UPC.Contains(filter.UPC))
                    .WhereIf(!string.IsNullOrEmpty(filter.Name), c => c.Name.Contains(filter.Name))
                    .WhereIf(filter.ProductType > 0, c => c.ProductType == filter.ProductType)
                    .WhereIf(filter.FamilyId > 0, c => c.FamilyId != filter.FamilyId);

            query = query.Where(e => e.State == filter.State);

            query = query
                .Switch(splited)
                .OrderByCase(e => e[0] == "upc" && e[1] == "asc", e => e.UPC)
                .OrderByDescendingCase(e => e[0] == "upc" && e[1] == "desc", e => e.UPC)
                .OrderByCase(e => e[0] == "name" && e[1] == "asc", e => e.Name)
                .OrderByDescendingCase(e => e[0] == "name" && e[1] == "desc", e => e.Name)
                .OrderByCase(e => e[0] == "familyid" && e[1] == "asc", e => e.Family.Name)
                .OrderByDescendingCase(e => e[0] == "familyid" && e[1] == "desc", e => e.Family.Name)
                .OrderByDefault(e => e.Name);

            var entities = await query.ToPageAsync(currentPage, pageSize);
            return entities;
        }

        /// <summary>
        ///     Get all products
        /// </summary>
        /// <returns></returns>
        public async Task<List<Product>> Get()
        {
            var entities = await _productRepository
                .GetEntity<Product>()
                .Include(x => x.Family)
                .Include(x => x.Category)
                .OrderBy(x => x.Id)
                .ToListAsync();
            return entities;
        }

        /// <summary>
        ///     Change State for entity
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="state">new State</param>
        /// <returns>Boolean success</returns>
        /// <exception cref="BusinessLogicException">If id is less to -1</exception>
        public async Task<bool> ChangeState(long id, bool state)
        {
            if (id < 1)
            {
                _logger.LogError("Id parameter out of bounds");
                throw new BusinessLogicException("Id parameter out of bounds.");
            }

            var pType = await _productRepository.GetEntity<Product>()
                .Where(x => x.Id == id)
                .Select(x => x.ProductType)
                .FirstOrDefaultAsync();

            var sp = await _productRepository.GetEntity<Product>()
                .Include(x => x.StoreProducts)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            
            foreach (var s in sp.StoreProducts)
            {
                if (sp.AddOnlineStore)
                {
                    await _mediator.Publish(new ProductChangeState(LiteProduct.Convert(sp, s), new List<long>() { s.StoreId }, s.Id, false));
                }
            }

            return pType switch
            {
                ProductType.SLP => await ProductChangeState<ScaleProduct, LiteScaleProduct>(id,
                    LiteScaleProduct.Convert1, state),
                ProductType.KPT => await ProductChangeState<KitProduct, LiteKitProduct>(id, LiteKitProduct.Convert2,
                    state),
                _ => await ProductChangeState<Product, LiteProduct>(id, LiteProduct.Convert, state)
            };
        }
        /// <summary>
        ///     Update Product Online
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        /// <exception cref="BusinessLogicException">If id is less to -1</exception>
        public async Task UpdateProductOnline(long id)
        {
            if (id < 1)
            {
                _logger.LogError("Id parameter out of bounds");
                throw new BusinessLogicException("Id parameter out of bounds.");
            }

            var product = await _productRepository.GetEntity<Product>()
                .Include(x => x.StoreProducts)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            
            product.AddOnlineStore = true;
            
            await _productRepository.UpdateAsync(id, product);
            
        }

        private async Task<bool> ProductChangeState<TEntity, TDestiny>(long id,
            Func<TEntity, StoreProduct, TDestiny> callback, bool state) where TEntity : Product
        {
            var tempP = await _productRepository.GetEntity<TEntity>()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (tempP == null) return false;
            if (!state)
            {
                tempP.PosVisible = false;
                tempP.ScaleVisible = false;
            }

            tempP.State = state;

            var success = await _productRepository.UpdateAsync<TEntity>(tempP);

            var stores = await _productRepository.GetEntity<StoreProduct>()
                .Where(x => x.ProductId == id)
                .ToListAsync();

            foreach (var s in stores)
            {
                //(List<long> stores, TDat data, SynchroType type, Func<TDat, string> converter = null);
                await synchroService.AddSynchroToStore(
                    s.StoreId,
                    callback(tempP, s),
                    SynchroType.UPDATE
                );
            }

            return success;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            if (!isNumber(product.UPC))
            {
                _logger.LogError("The upc field only allows numbers.");
                throw new BusinessLogicException("The upc field only allows numbers.");
            }

            if (product.FamilyId == -1) product.FamilyId = null;
            if (product.DefaulShelfTagId == -1) product.DefaulShelfTagId = null;
            if (product.MinimumAge == -1) product.MinimumAge = null;
            var id = await _productRepository.CreateAsync(product);
            product.Id = id;
            return product;
        }

        public async Task<ScaleProduct> CreateScaleProduct(ScaleProduct product)
        {
            if (!isNumber(product.UPC))
            {
                _logger.LogError("The upc field only allows numbers.");
                throw new BusinessLogicException("The upc field only allows numbers.");
            }

            if (product.FamilyId == -1) product.FamilyId = null;
            if (product.DefaulShelfTagId == -1) product.DefaulShelfTagId = null;
            if (product.MinimumAge == -1) product.MinimumAge = null;
            // if (product.ScaleLabelTypes != null)
            //     for (int i = 0; i < product.ScaleLabelTypes.Count; i++)
            //     {
            //         // var match = await this._repository.GetEntity<Store>().FindAsync(entity.Stores[i].Id);
            //         // elem.Add(match);
            //         this._scaleProductRepository.GetEntity<ScaleLabelType>().Attach(product.ScaleLabelTypes[i]);
            //     }
            var id = await _scaleProductRepository.CreateAsync(product);
            product.Id = id;
            return product;
        }

        public async Task<KitProduct> CreateKitProduct(KitProduct product)
        {
            if (!isNumber(product.UPC))
            {
                _logger.LogError("The upc field only allows numbers.");
                throw new BusinessLogicException("The upc field only allows numbers.");
            }

            if (product.FamilyId == -1) product.FamilyId = null;
            if (product.DefaulShelfTagId == -1) product.DefaulShelfTagId = null;
            var id = await _kitProductRepository.CreateAsync(product);
            product.Id = id;
            return product;
        }

        public async Task<Product> GetProductById(long id)
        {
            return await _productRepository.GetEntity<Product>()
                .Include(x => x.Category)
                .Include(x => x.Family)
                .Include(x => x.Department)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ScaleProduct> GetScaleProductById(long id)
        {
            return await _scaleProductRepository.GetEntity<ScaleProduct>()
                .Include(x => x.ScaleLabelDefinitions).IgnoreAutoIncludes()
                .Include(x => x.Category).IgnoreAutoIncludes()
                .Include(x => x.Family).IgnoreAutoIncludes()
                .Include(x => x.ScaleCategory).IgnoreAutoIncludes()
                .Include(x => x.Department).IgnoreAutoIncludes()
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<KitProduct> GetKitProductById(long id)
        {
            return await _kitProductRepository.GetEntity<KitProduct>()
                .Include(x => x.Category)
                .Include(x => x.Family)
                .Include(x => x.Department)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductByIdWithDefaultShelfTag(long id)
        {
            return await _productRepository.GetEntity<Product>()
                .Include(x => x.Category)
                .Include(x => x.Family)
                .Include(x => x.Department)
                .Include(x => x.DefaulShelfTag)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductByUPC(string upc, long productID = -1)
        {
            if (productID == -1)
                return await _productRepository.GetEntity<Product>().Include(e => e.StoreProducts)
                    .Where(x => x.UPC == upc).FirstOrDefaultAsync();
            return await _productRepository.GetEntity<Product>().Include(e => e.StoreProducts)
                .Where(x => x.UPC == upc && x.Id != productID).FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductByUPCWithStoreAndVendor(string upc, long productID = -1)
        {
            if (productID == -1)
                return await _productRepository.GetEntity<Product>().Include(e => e.StoreProducts).Include(v => v.VendorProducts)
                    .Where(x => x.UPC == upc).FirstOrDefaultAsync();
            return await _productRepository.GetEntity<Product>().Include(e => e.StoreProducts).Include(v => v.VendorProducts)
                .Where(x => x.UPC == upc && x.Id != productID).FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductByPLU(int plu, long productID = -1)
        {
            if (productID == -1)
                return await _productRepository.GetEntity<ScaleProduct>().Include(e => e.StoreProducts)
                    .Where(x => x.PLUNumber == plu).FirstOrDefaultAsync();
            return await _productRepository.GetEntity<ScaleProduct>().Include(e => e.StoreProducts)
                .Where(x => x.PLUNumber == plu && x.Id != productID).FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductByName(string name, long productID = -1)
        {
            if (productID == -1)
                return await _productRepository.GetEntity<Product>().Where(x => x.Name == name).FirstOrDefaultAsync();
            return await _productRepository.GetEntity<Product>().Where(x => x.Name == name && x.Id != productID)
                .FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductByPLUNumber(int plu, long productID = -1)
        {
            if (productID == -1)
                return await _productRepository.GetEntity<ScaleProduct>().Where(x => x.PLUNumber == plu)
                    .FirstOrDefaultAsync();
            return await _productRepository.GetEntity<ScaleProduct>()
                .Where(x => x.PLUNumber == plu && x.Id != productID).FirstOrDefaultAsync();
        }

        /// <summary>
        ///     Delete a entity
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Boolean success</returns>
        /// <exception cref="BusinessLogicException">If id is less to -1</exception>
        public async Task<bool> Delete(long id)
        {
            if (id < 1)
            {
                _logger.LogError("Id parameter out of bounds.");
                throw new BusinessLogicException("Id parameter out of bounds.");
            }

            //process syncronizations
            var sp = await _productRepository.GetEntity<Product>()
                .Include(x => x.StoreProducts)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            foreach (var s in sp.StoreProducts)
            {
                //(List<long> stores, TDat data, SynchroType type, Func<TDat, string> converter = null);
                await synchroService.AddSynchroToStore(
                    s.StoreId,
                    LiteProduct.Convert(sp, s),
                    SynchroType.DELETE
                );
            }

            var success = await _productRepository.DeleteAsync(id);
            return success;
        }

        /// <summary>
        ///     Delete a list
        /// </summary>
        /// <param name="ids">List of ids</param>
        /// <returns>Boolean success</returns>
        /// <exception cref="BusinessLogicException">If list is null or empty</exception>
        public async Task<bool> DeleteRange(List<long> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                _logger.LogError("List of ids is null or empty.");
                throw new BusinessLogicException("List of ids is null or empty.");
            }

            //var entitiesDelete =
            //    await _productRepository.GetEntity<Product>().Where(x => ids.Contains(x.Id)).ToListAsync();

            //var success = await _productRepository.DeleteRangeAsync(entitiesDelete);
            //return success;

            foreach (var id in ids)
            {
                await Delete(id);
            }

            return true;
        }

        public async Task<bool> UpdateProduct(long productid, Product entity)
        {
            if (productid < 1)
            {
                _logger.LogError("Id parameter out of bounds.");
                throw new BusinessLogicException("Id parameter out of bounds.");
            }

            if (!isNumber(entity.UPC))
            {
                _logger.LogError("The upc field only allows numbers.");
                throw new BusinessLogicException("The upc field only allows numbers.");
            }

            if (entity.FamilyId == -1) entity.FamilyId = null;
            if (entity.DefaulShelfTagId == -1) entity.DefaulShelfTagId = null;
            if (entity.MinimumAge == -1) entity.MinimumAge = null;

            var prevProduct = await _productRepository.GetEntity<Product>()
                .Where(x => x.Id == productid).FirstOrDefaultAsync();
            
            bool oldState = prevProduct.AddOnlineStore;
            long oldCategory = prevProduct.CategoryId;
            bool needAddShelfTag = (prevProduct.CategoryId != entity.CategoryId) ||
                                   (prevProduct.DepartmentId != entity.DepartmentId);

            var success = await _productRepository.UpdateAsync(productid, entity);
            //process syncronizations
            var sp = await _productRepository.GetEntity<Product>()
                .Include(x => x.StoreProducts)
                .Where(x => x.Id == productid)
                .FirstOrDefaultAsync();

            foreach (var s in sp.StoreProducts)
            {
                //(List<long> stores, TDat data, SynchroType type, Func<TDat, string> converter = null);
                if (entity.AddOnlineStore)
                {
                    await _mediator.Publish(new ProductUpdated(
                        LiteProduct.Convert(sp, s), 
                        new List<long>() { s.StoreId }, 
                        sp.DepartmentId, 
                        oldCategory, 
                        s.Id));
                }

                if (!entity.AddOnlineStore && oldState == false)
                {
                    await _mediator.Publish(new ProductUpdated(
                        LiteProduct.Convert(sp, s), 
                        new List<long>() { s.StoreId }, 
                        sp.DepartmentId, 
                        oldCategory, 
                        s.Id));
                }

                if (entity.AddOnlineStore == false && oldState == true)
                {
                    await _mediator.Publish(new ProductChangeState(
                        LiteProduct.Convert(sp, s), 
                        new List<long>() { s.StoreId }, 
                        s.Id, 
                        false));
                }

                await synchroService.AddSynchroToStore(
                    s.StoreId,
                    LiteProduct.Convert(sp, s),
                    SynchroType.UPDATE
                );

                if (needAddShelfTag)

                    await _shelfTagService.PostFromStoreProduct(s.Id);
            }

            return success;
        }

        public async Task<bool> UpdateScaleProduct(long productid, ScaleProduct entity)
        {
            if (productid < 1)
            {
                _logger.LogError("Id parameter out of bounds.");
                throw new BusinessLogicException("Id parameter out of bounds.");
            }

            if (!isNumber(entity.UPC))
            {
                _logger.LogError("The upc field only allows numbers.");
                throw new BusinessLogicException("The upc field only allows numbers.");
            }

            if (entity.FamilyId == -1) entity.FamilyId = null;
            if (entity.DefaulShelfTagId == -1) entity.DefaulShelfTagId = null;
            if (entity.MinimumAge == -1) entity.MinimumAge = null;


            var prevProduct = await _productRepository.GetEntity<ScaleProduct>()
                .Where(x => x.Id == productid).FirstOrDefaultAsync();

            bool oldState = prevProduct.AddOnlineStore;
            long oldCategory = prevProduct.CategoryId;
            bool needAddShelfTag = (prevProduct.CategoryId != entity.CategoryId) ||
                                   (prevProduct.DepartmentId != entity.DepartmentId);


            var success = await _scaleProductRepository.UpdateAsync(productid, entity);
            //process syncronizations
            var sp = await _productRepository.GetEntity<ScaleProduct>()
                .Include(x => x.StoreProducts)
                .Include(x => x.ScaleLabelDefinitions)
                .Where(x => x.Id == productid)
                .FirstOrDefaultAsync();

            foreach (var s in sp.StoreProducts)
            {
                //(List<long> stores, TDat data, SynchroType type, Func<TDat, string> converter = null);
                if (entity.AddOnlineStore)
                {
                    await _mediator.Publish(new ProductUpdated(
                        LiteProduct.Convert(sp, s), 
                        new List<long>() { s.StoreId }, 
                        sp.DepartmentId, 
                        oldCategory, 
                        s.Id));
                }

                if (!entity.AddOnlineStore && oldState == false)
                {
                    await _mediator.Publish(new ProductUpdated(
                        LiteProduct.Convert(sp, s), 
                        new List<long>() { s.StoreId }, 
                        sp.DepartmentId, 
                        oldCategory, 
                        s.Id));
                }

                if (entity.AddOnlineStore == false && oldState == true)
                {
                    await _mediator.Publish(new ProductChangeState(
                        LiteProduct.Convert(sp, s), 
                        new List<long>() { s.StoreId }, 
                        s.Id, 
                        false));
                }

                await synchroService.AddSynchroToStore(
                    s.StoreId,
                    LiteScaleProduct.Convert1(sp, s),
                    SynchroType.UPDATE
                );
                if (sp.ScaleLabelDefinitions != null)
                    foreach (var sd in sp.ScaleLabelDefinitions)
                    {
                        await synchroService.AddSynchroToStore(
                            s.StoreId,
                            LiteScaleLabelDefinition.Convert(sd),
                            SynchroType.UPDATE
                        );
                    }


                if (needAddShelfTag)
                    await _shelfTagService.PostFromStoreProduct(s.Id);
            }

            return success;
        }

        public async Task<bool> UpdateKitProduct(long productid, KitProduct entity)
        {
            if (productid < 1)
            {
                _logger.LogError("Id parameter out of bounds.");
                throw new BusinessLogicException("Id parameter out of bounds.");
            }

            if (!isNumber(entity.UPC))
            {
                _logger.LogError("The upc field only allows numbers.");
                throw new BusinessLogicException("The upc field only allows numbers.");
            }

            var prevProduct = await _productRepository.GetEntity<KitProduct>()
                .Where(x => x.Id == productid).FirstOrDefaultAsync();

            bool oldState = prevProduct.AddOnlineStore;
            long oldCategory = prevProduct.CategoryId;

            var success = await _kitProductRepository.UpdateAsync(productid, entity);
            //process syncronizations
            var sp = await _productRepository.GetEntity<KitProduct>()
                .Include(x => x.StoreProducts)
                .Where(x => x.Id == productid)
                .FirstOrDefaultAsync();

            foreach (var s in sp.StoreProducts)
            {
                if (entity.AddOnlineStore)
                {
                    await _mediator.Publish(new ProductUpdated(
                        LiteProduct.Convert(sp, s), 
                        new List<long>() { s.StoreId }, 
                        sp.DepartmentId, 
                        oldCategory, 
                        s.Id));
                }

                if (!entity.AddOnlineStore && oldState == false)
                {
                    await _mediator.Publish(new ProductUpdated(
                        LiteProduct.Convert(sp, s), 
                        new List<long>() { s.StoreId }, 
                        sp.DepartmentId, 
                        oldCategory, 
                        s.Id));
                }

                if (entity.AddOnlineStore == false && oldState == true)
                {
                    await _mediator.Publish(new ProductChangeState(
                        LiteProduct.Convert(sp, s), 
                        new List<long>() { s.StoreId }, 
                        s.Id, 
                        false));
                }
                //(List<long> stores, TDat data, SynchroType type, Func<TDat, string> converter = null);
                await synchroService.AddSynchroToStore(
                    s.StoreId,
                    LiteKitProduct.Convert2(sp, s),
                    SynchroType.UPDATE
                );
            }

            return success;
        }

        public async Task<List<ScaleProduct>> GetScaleProduct()
        {
            var entities = await _scaleProductRepository
                .GetEntity<ScaleProduct>()
                .Include(x => x.ScaleCategory)
                .ToListAsync();
            return entities;
        }

        public async Task<List<Product>> GetProductsByStoreId(long storeId)
        {
            var result = new List<Product>();
            var entities = await _productRepository
                .GetEntity<Product>()
                .Include(x => x.StoreProducts)
                .Where(x => x.StoreProducts.Count > 0)
                .ToListAsync();
            foreach (var item in entities)
            {
                if (item.StoreProducts.Find(x => x.StoreId == storeId) != null)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        public async Task<List<ScaleProduct>> GetScaleProductsByStoreId(long storeId)
        {
            var result = new List<ScaleProduct>();
            var entities = await _scaleProductRepository
                .GetEntity<ScaleProduct>()
                .Include(x => x.StoreProducts)
                .Where(x => x.StoreProducts.Count > 0)
                .ToListAsync();
            foreach (var item in entities)
            {
                if (item.StoreProducts.Find(x => x.StoreId == storeId) != null)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        public async Task<List<PLUModel>> GetUpdatesForScales(long storeId, DateTime last,
            List<long> deps)
        {
            if (deps == null || deps.Count == 0)
            {
                var products = await _scaleProductRepository.GetEntity<ScaleProduct>()
                    .Include(x => x.StoreProducts)
                    .Include(x => x.Department)
                    .Include(x => x.ScaleCategory)
                    .Include(x => x.ScaleLabelDefinitions).ThenInclude(s => s.ScaleLabelType1)
                    .Where(x =>
                        x.State &&
                        (
                            x.CreatedAt > last ||
                            x.UpdatedAt > last
                        )
                    )
                    .ToListAsync();
                return ToModel(products, storeId);
            }
            else
            {
                var products = await _scaleProductRepository.GetEntity<ScaleProduct>()
                    .Include(x => x.StoreProducts)
                    .Include(x => x.Department)
                    .Include(x => x.ScaleCategory)
                    .Include(x => x.ScaleLabelDefinitions).ThenInclude(s => s.ScaleLabelType1)
                    .Where(x =>
                        x.State &&
                        (
                            x.CreatedAt > last ||
                            x.UpdatedAt > last
                        ) && deps.Any(d => d == x.DepartmentId))
                    .ToListAsync();
                return ToModel(products, storeId);
            }
        }


        public async Task<List<PLUModel>> GetAllForScales(long storeId, List<long> deps)
        {
            if (deps == null || deps.Count == 0)
            {
                var products = await _scaleProductRepository.GetEntity<ScaleProduct>()
                    .Include(x => x.StoreProducts)
                    .Include(x => x.Department)
                    .Include(x => x.ScaleCategory)
                    .Include(x => x.ScaleLabelDefinitions).ThenInclude(s => s.ScaleLabelType1)
                    .Where(x => //x.Id == 44151 &&
                        x.State)
                    //.Take(1)
                    .ToListAsync();

                return ToModel(products, storeId);
            }
            else
            {
                var products = await _scaleProductRepository.GetEntity<ScaleProduct>()
                    .Include(x => x.StoreProducts)
                    .Include(x => x.Department)
                    .Include(x => x.ScaleCategory)
                    .Include(x => x.ScaleLabelDefinitions).ThenInclude(s => s.ScaleLabelType1)
                    .Where(x => //x.Id == 44151 &&
                        x.State && deps.Any(d => d == x.DepartmentId))
                    //.Take(1)
                    .ToListAsync();

                return ToModel(products, storeId);
            }
        }

        private List<PLUModel> ToModel(List<ScaleProduct> products, long storeId)
        {
            var result = new List<PLUModel>();
            foreach (var x in products)
            {
                var st = x.StoreProducts.Find(x => x.StoreId == storeId);
                if (st != null)
                {
                    result.Add(new PLUModel()
                    {
                        Id = x.PLUNumber,
                        ItemCode = x.UPC,
                        Name1 = x.Name,
                        Name2 = null,
                        Name3 = null,
                        DepartmentId = x.Department.DepartmentId,
                        GroupId = x.ScaleCategory.CategoryId,
                        Price = st.Price,
                        UnitId = x.PLUType == PluType.RandomWeight ? UnitId.LB : UnitId.PCS,

                        Text1 = x.Text1,

                        Text2 = x.Text2,
                        Text3 = x.Text3,
                        Label1ID = x.ScaleLabelDefinitions.Count() == 0
                            ? 0
                            : x.ScaleLabelDefinitions[0].ScaleLabelType1.LabelId,
                        FreshnessDate = x.ShelfLife,
                        ValidDate = x.ProductLife,
                    });
                }
            }

            return result;
        }

        public bool isNumber(string stringToVerify)
        {
            // console.log
            double numericValue;

            bool isNumber = double.TryParse(stringToVerify, out numericValue);

            if (!isNumber)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cutTemplateId"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Pager<ScaleProduct>> GetScaleProductsByUpcPluProduct(long cutTemplateId, int currentPage, int pageSize, ScaleProductSearchModel filter)
        {
            if (currentPage >= 1 && pageSize >= 1)
            {
                return await _scaleProductRepository.GetEntity<ScaleProduct>()
                                                    .WithSpecification(new CutListGetScaleProductByUpcPluProductSpec(cutTemplateId, filter))
                                                    .ToPageAsync(currentPage, pageSize);
            }

            _logger.LogError("Page parameter (currentPage or pageSize) out of bounds");
            throw new BusinessLogicException("Page parameter out of bounds.");
        }

        /// <summary>
        /// Get filter and paginate ScaleProduct by TemplateId
        /// </summary>
        /// <param name="CurrentPage"></param>
        /// <param name="PageSize"></param>
        /// <param name="TemplateId">Cut List Template Id</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <exception cref="BusinessLogicException"></exception>
        public async Task<List<ScaleProduct>> GetScaleProductsByTemplate(long TemplateId)
        {
            if (TemplateId < 1)
            {
                throw new BusinessLogicException("Id parameter out of bounds");
            }

            return await _scaleProductRepository.GetEntity<ScaleProduct>()
                                                .WithSpecification(new CutListGetScaleProductByTemplateSpec(TemplateId))
                                                .ToListAsync();
        }

        /// <summary>
        /// Get ScaleProducts by filter
        /// </summary>
        /// <param name="filter">UPC Code or PlUNumber</param>
        /// <returns></returns>
        public async Task<List<ScaleProduct>> GetScaleProductsByUpcOrPlu(string filter)
        {
            return await _scaleProductRepository.GetEntity<ScaleProduct>()
                                                .WithSpecification(new CutListGetScaleProductByUpcOrPluSpec(filter))
                                                .ToListAsync();
        }
    }
}