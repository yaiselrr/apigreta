using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.BusinessLogic.Handlers.DataHandlers;
using Greta.BO.BusinessLogic.Handlers.DataHandlers.Importers;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.Core.Models.Pager;
using Greta.Sdk.EFCore.Middleware;
using Greta.Sdk.EFCore.Operations;
using Greta.Sdk.ExternalScale.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.ExportImport
{
    public class ProductImportTests
    {
        private readonly INotifier _notifier;
        private AuthenticateUser<string> _authenticateUser = null;

        public ProductImportTests()
        {
            _authenticateUser = new AuthenticateUser<string>()
            {
                UserName = "Greta.Sdk",
                UserId = new Guid().ToString(),
                IsAuthenticated = true,
                IsApplication = false,
            };
        }

        private ProductImport GetProductImporter(string dbName)
        {
            var provider = new ServiceCollection()
                .AddSingleton<INotifier, FakeNotifier>() // we could read from message property
                .AddDbContext<SqlServerContext>(opts => { opts.UseInMemoryDatabase(dbName); })
                .AddScoped<IProductRepository, FakeProductRepository>()
                .AddScoped<ICategoryRepository, FakeCategoryRepository>()
                .AddScoped<IDepartmentRepository, FakeDepartmentRepository>()
                .AddScoped<IStoreProductRepository, FakeStoreProductRepository>()
                .BuildServiceProvider();
            var logger = Mock.Of<ILogger<ProductImport>>();
            var inLogger = Mock.Of<ILogger<Introspector<Product>>>();
            var importer = new ProductImport(logger, provider, inLogger, _notifier);
            return importer;
        }

        // [Fact]
        // public void ProductImport_initialization()
        // {
        //     var importer = GetProductImporter("initialization");
        // }
    }

    #region Fake services

    // var productService = _provider.GetRequiredService<IProductService>();
    // var departmentService = _provider.GetRequiredService<IDepartmentService>();
    // var catService = _provider.GetRequiredService<ICategoryService>();
    // var storeProductService = _provider.GetRequiredService<IStoreProductService>();

    #endregion

    #region Fake repositories

    public class FakeProductRepository : OperationBase<long, string, Product>, IProductRepository
    {
        public FakeProductRepository(IAuthenticateUser<string> authenticatedUser, DbContext context) :
            base(authenticatedUser, context)
        {
        }

        public async Task<Product> GetByUPC(string upc, bool track = true)
        {
            return track
                ? await Context.Set<Product>().Include(e => e.StoreProducts).FirstOrDefaultAsync(e => e.UPC == upc)
                : await Context.Set<Product>().Include(e => e.StoreProducts).AsNoTracking()
                    .FirstOrDefaultAsync(e => e.UPC == upc);
        }
    }

    public class FakeScaleProductRepository : OperationBase<long, string, ScaleProduct>, IScaleProductRepository
    {
        public FakeScaleProductRepository(IAuthenticateUser<string> authenticatedUser, DbContext context) :
            base(authenticatedUser, context)
        {
        }

        public async Task<ScaleProduct> GetByUPC(string upc, bool track = true)
        {
            return track
                ? await Context.Set<ScaleProduct>().Include(e => e.StoreProducts).FirstOrDefaultAsync(e => e.UPC == upc)
                : await Context.Set<ScaleProduct>().Include(e => e.StoreProducts).AsNoTracking()
                    .FirstOrDefaultAsync(e => e.UPC == upc);
        }
    }

    public class FakeCategoryRepository : OperationBase<long, string, Category>, ICategoryRepository
    {
        public FakeCategoryRepository(IAuthenticateUser<string> authenticatedUser, DbContext context) :
            base(authenticatedUser, context)
        {
        }

        public async Task<Category> GetByCategory(int categoryId, bool track = true)
        {
            return track
                ? await Context.Set<Category>().FirstOrDefaultAsync(e => e.CategoryId == categoryId)
                : await Context.Set<Category>().AsNoTracking().FirstOrDefaultAsync(e => e.CategoryId == categoryId);
        }
    }

    public class FakeDepartmentRepository : OperationBase<long, string, Department>, IDepartmentRepository
    {
        public FakeDepartmentRepository(IAuthenticateUser<string> authenticatetUser, DbContext context) :
            base(authenticatetUser, context)
        {
        }

        public async Task<Department> GetByDepartment(int departmentId, bool track = true)
        {
            return track
                ? await Context.Set<Department>().FirstOrDefaultAsync(e => e.DepartmentId == departmentId)
                : await Context.Set<Department>().AsNoTracking()
                    .FirstOrDefaultAsync(e => e.DepartmentId == departmentId);
        }
    }

    public class FakeStoreProductRepository : OperationBase<long, string, StoreProduct>, IStoreProductRepository
    {
        public FakeStoreProductRepository(IAuthenticateUser<string> authenticatetUser, DbContext context) : base(
            authenticatetUser, context)
        {
        }
    }

    public class FakeProductService : IProductService
    {
        private readonly IScaleProductRepository _scaleProductRepository;

        public FakeProductService(IScaleProductRepository scaleProductRepository)
        {
            _scaleProductRepository = scaleProductRepository;
        }

        public Task<List<Product>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<List<ScaleProduct>> GetScaleProduct()
        {
            throw new NotImplementedException();
        }

        public Task<List<ScaleProduct>> GetScaleProductsByUpcPluProduct(string Upc, int Plu, string Name)
        {
            throw new NotImplementedException();
        }

        public Task<List<ScaleProduct>> GetScaleProductsByTemplate(long TemplateId)
        {
            throw new NotImplementedException();
        }

        public Task<Pager<Product>> Filter(int currentPage, int pageSize, ProductSearchModel filter,
            string searchstring, string sortstring)
        {
            throw new NotImplementedException();
        }

        public Task<Pager<Product>> FilterByStore(long storeId, int currentPage, int pageSize, Product filter,
            string searchstring, string sortstring)
        {
            throw new NotImplementedException();
        }

        public Task<Pager<Product>> FilterByBatch(long batchId, int currentPage, int pageSize, Product filter,
            string searchstring, string sortstring)
        {
            throw new NotImplementedException();
        }

        public Task<Pager<Product>> FilterByFamily(long familyId, int currentPage, int pageSize, Product filter,
            string searchstring,
            string sortstring)
        {
            throw new NotImplementedException();
        }

        public Task<Pager<Product>> FilterNotByFamily(int currentPage, int pageSize, Product filter,
            string searchstring,
            string sortstring)
        {
            throw new NotImplementedException();
        }

        public Task<Product> CreateProduct(Product product)
        {
            throw new NotImplementedException();
        }


        public async Task<ScaleProduct> CreateScaleProduct(ScaleProduct product)
        {
            var id = await _scaleProductRepository.CreateAsync(product);
            product.Id = id;
            return product;
        }

        public Task<KitProduct> CreateKitProduct(KitProduct product)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductById(long id)
        {
            throw new NotImplementedException();
        }

        public Task<ScaleProduct> GetScaleProductById(long id)
        {
            throw new NotImplementedException();
        }

        public Task<KitProduct> GetKitProductById(long id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductByIdWithDefaultShelfTag(long id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductByPLU(int plu, long productID = -1)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductByUPC(string upc, long productID = -1)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductByUPCWithStoreAndVendor(string upc, long productID = -1)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductByName(string name, long productID = -1)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductByPLUNumber(int plu, long productID = -1)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ChangeState(long id, bool state)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteRange(List<long> ids)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateProduct(long productid, Product entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateScaleProduct(long productid, ScaleProduct entity)
        {
            var id = await _scaleProductRepository.UpdateAsync(productid, entity);
            return id;
        }

        public Task<bool> UpdateKitProduct(long productid, KitProduct entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetProductsByStoreId(long storeId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ScaleProduct>> GetScaleProductsByStoreId(long storeId)
        {
            throw new NotImplementedException();
        }

        public Task<List<PLUModel>> GetUpdatesForScales(long storeId, DateTime last, List<long> deps)
        {
            throw new NotImplementedException();
        }

        public Task<List<PLUModel>> GetAllForScales(long storeId, List<long> deps)
        {
            throw new NotImplementedException();
        }

        public bool isNumber(string stringToVerify)
        {
            throw new NotImplementedException();
        }

        public Task<Pager<ScaleProduct>> GetScaleProductsByUpcPluProduct(int currentPage, int pageSize, ScaleProductSearchModel filter)
        {
            throw new NotImplementedException();
        }

        public Task<Pager<ScaleProduct>> GetScaleProductsByUpcPluProduct(long cutTemplateId, int currentPage, int pageSize, ScaleProductSearchModel filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<ScaleProduct>> GetScaleProductsByTemplate(long TemplateId, string filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<ScaleProduct>> GetScaleProductsByUpcOrPlu(string filter)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProductOnline(long id)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}