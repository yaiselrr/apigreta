using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.BusinessLogic.Handlers.DataHandlers;
using Greta.BO.BusinessLogic.Handlers.DataHandlers.Importers;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.EFCore.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Range = Moq.Range;

namespace Greta.BO.Api.Test.ExportImport;

public class ScaleProductImportTests
{
    private readonly INotifier _notifier;
    private AuthenticateUser<string> _authenticateUser = null;
    private ServiceProvider provider;
    private ScaleProductImport importer;

    public ScaleProductImportTests()
    {
        _authenticateUser = new AuthenticateUser<string>()
        {
            UserName = "Greta.Sdk",
            UserId = new Guid().ToString(),
            IsAuthenticated = true,
            IsApplication = false,
        };
        var collection = new ServiceCollection();
        collection.AddSingleton<INotifier, FakeNotifier>(); // we could read from message property
        collection.AddDbContext<SqlServerContext>(opts => { opts.UseInMemoryDatabase("ScaleProductImportTests"); });
        collection.AddScoped<DbContext>(o =>
            {
                var config = new DbContextOptionsBuilder().UseInMemoryDatabase("ScaleProductImportTests").Options;
                return new SqlServerContext(config, _authenticateUser);
            });
        collection.AddScoped<IAuthenticateUser<string>>(o => _authenticateUser);
        
        collection.AddScoped<IScaleProductRepository, FakeScaleProductRepository>();
        collection.AddScoped<ICategoryRepository, FakeCategoryRepository>();
        collection.AddScoped<IDepartmentRepository, FakeDepartmentRepository>();
        collection.AddScoped<IStoreProductRepository, FakeStoreProductRepository>();

        // var prodService = Mock.Of<IProductService>();
        // Mock.Get(prodService).Setup(m => m
        //         .CreateScaleProduct(It.IsAny<ScaleProduct>())
        //     .ReturnsAsync((int depId, long Id) => new Department() { Id = 1, Name = "MOckDepartment" });
        // collection.AddScoped<IProductService, ProductService>();//(opt => Mock.Of<IProductService>());
        
        collection.AddScoped<IProductService, FakeProductService>();

        var depService = Mock.Of<IDepartmentService>();
        Mock.Get(depService).Setup(m => m
                .GetByDepartmentId(It.IsInRange(1, 100000, Range.Inclusive), It.IsAny<long>()))
            .ReturnsAsync((int depId, long Id) => new Department() { Id = 1, Name = "MOckDepartment" })
            ;
        collection.AddScoped<IDepartmentService>(opt => depService);
        var catService = Mock.Of<ICategoryService>();
        Mock.Get(catService).Setup(m => m
                .GetByCategoryId(It.IsInRange(1, 100000, Range.Inclusive), It.IsAny<long>()))
            .ReturnsAsync((int depId, long Id) => new Category() { Id = 1, Name = "MOckCategory" });
        collection.AddScoped<ICategoryService>(opt => catService);
        var scaleCatService = Mock.Of<IScaleCategoryService>();
        Mock.Get(scaleCatService).Setup(m => m
                .GetByScaleCategoryId(It.IsInRange(1, 100000, Range.Inclusive), It.IsAny<long>()))
            .ReturnsAsync((int depId, long Id) => new ScaleCategory() { Id = 1, Name = "MOckScaleCategory" });
        collection.AddScoped<IScaleCategoryService>(opt => scaleCatService);
        
        collection.AddScoped<IStoreProductService>(opt =>
        {
            return Mock.Of<IStoreProductService>();
        });
        collection.AddScoped<IScaleLabelDefinitionService>(opt =>
            {
                return Mock.Of<IScaleLabelDefinitionService>();
            });
            
        provider = collection.BuildServiceProvider();

    
        
        _notifier = provider.GetRequiredService<INotifier>();//  new FakeNotifier();
        
        var logger = Mock.Of<ILogger<ScaleProductImport>>();
        var inLogger = Mock.Of<ILogger<Introspector<ScaleProduct>>>();
        importer = new ScaleProductImport(logger, provider, inLogger, _notifier);
    }

    private Dictionary<string, string> GetBasicMapping()
    {
        return new Dictionary<string, string>()
        {
            {"PLU Type", "PLUType"},
            {"Plu_No", "PLUNumber"},
            {"UPC", "UPC"},
            {"Line_1", "Text1"},
            {"Description1", "Description1"},
            {"LabelFormatNo1", "Label_1"},
            {"Price", "Price"},
            {"POS Description", "Name"},
            {"Category", "CategoryId"},
            {"Scale Category", "ScaleCategoryId"},
            {"Departments", "DepartmentId"},
            {"ShelfLife", "ShelfLife"},
            {"EBT", "SnapEBT"},
            {"Sell Beyond Zero", "AllowZeroStock"},
            //Show StockOn POS
        };
    }
    
    private Dictionary<string, string> GetNoDescriptionMapping()
    {
        return new Dictionary<string, string>()
        {
            {"PLU Type", "PLUType"},
            {"Plu_No", "PLUNumber"},
            {"UPC", "UPC"},
            {"Line_1", "Text1"},
            // {"Description1", "Description1"},
            {"LabelFormatNo1", "Label_1"},
            {"Price", "Price"},
            {"POS Description", "Name"},
            {"Category", "CategoryId"},
            {"Scale Category", "ScaleCategoryId"},
            {"Departments", "DepartmentId"},
            {"ShelfLife", "ShelfLife"},
            {"EBT", "SnapEBT"},
            {"Sell Beyond Zero", "AllowZeroStock"},
            //Show StockOn POS
        };
    }
    
    private Dictionary<string, string> GetNoDepartmentMapping()
    {
        return new Dictionary<string, string>()
        {
            {"PLU Type", "PLUType"},
            {"Plu_No", "PLUNumber"},
            {"UPC", "UPC"},
            {"Line_1", "Text1"},
            // {"Description1", "Description1"},
            {"LabelFormatNo1", "Label_1"},
            {"Price", "Price"},
            {"POS Description", "Name"},
            {"Category", "CategoryId"},
            {"Scale Category", "ScaleCategoryId"},
            // {"Departments", "DepartmentId"},
            {"ShelfLife", "ShelfLife"},
            {"EBT", "SnapEBT"},
            {"Sell Beyond Zero", "AllowZeroStock"},
            //Show StockOn POS
        };
    }

    // [Fact]
    // public async Task ProductImport_stable()
    // {
    //     var basemapping = GetBasicMapping();
    //     
    //     var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    //     {
    //         //HasHeaderRecord = false,
    //     };
    //     using (var stream = new MemoryStream())
    //     using (var reader = new StreamReader(stream))
    //     using (var writer = new StreamWriter(stream))
    //     using (var csv = new CsvReader(reader, config))
    //     {
    //         writer.WriteLine("Description1,PLU Type,Plu_No,UPC,Line1,Line2,LabelFormatNo1,Price,POS Description,Category,Scale Category,Departments,ShelfLife,EBT,Sell Beyond Zero,Show StockOn POS");
    //         writer.WriteLine("test1,RandomWeight,317,00317,SMOKED,SIRLOIN STEAK,2,10.99,SMOKED SIRLOIN STEAK,2,1,2,0,TRUE,TRUE,FALSE");
    //         // writer.WriteLine("test1,RandomWeight,307,00307,SMOKED,SIRLOIN STEAK,2,10.99,SMOKED SIRLOIN STEAK,2,1,2,0,TRUE,TRUE,FALSE");
    //         writer.Flush();
    //         stream.Position = 0;
    //         
    //         //Create a scalelabel type 
    //         var context = provider.GetRequiredService<DbContext>();
    //         await context.Set<ScaleLabelType>()
    //             .AddAsync(new ScaleLabelType() { Id = 2, Name = "2", LabelId = 2 });
    //         await context.SaveChangesAsync();
    //         
    //         await importer.Process(basemapping, csv, new List<long>() { 1 });
    //         var msg = importer.BuildMessage();
    //         Assert.Equal(0, msg.FailedRows);
    //     }
    // }
    
    [Fact]
    public async Task ProductImport_nodescription()
    {
        var basemapping = GetNoDescriptionMapping();
        
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            //HasHeaderRecord = false,
        };
        using (var stream = new MemoryStream())
        using (var reader = new StreamReader(stream))
        using (var writer = new StreamWriter(stream))
        using (var csv = new CsvReader(reader, config))
        {
            writer.WriteLine("PLU Type,Plu_No,UPC,Line1,Line2,LabelFormatNo1,Price,POS Description,Category,Scale Category,Departments,ShelfLife,EBT,Sell Beyond Zero,Show StockOn POS");
            writer.WriteLine("RandomWeight,327,00327,SMOKED,SIRLOIN STEAK,3,10.99,SMOKED SIRLOIN STEAK,2,1,2,0,TRUE,TRUE,FALSE");
            writer.WriteLine("RandomWeight,327,00327,SMOKED,SIRLOIN STEAK,3,10.99,SMOKED SIRLOIN STEAK,2,1,2,0,TRUE,TRUE,FALSE");
            writer.Flush();
            stream.Position = 0;
            
            //Create a scalelabel type 
            var context = provider.GetRequiredService<DbContext>();
            await context.Set<ScaleLabelType>()
                .AddAsync(new ScaleLabelType() { Id = 3, Name = "3", LabelId = 3 });
            await context.SaveChangesAsync();
            
            await importer.Process(basemapping, csv, new List<long>() { 1 });
            var msg = importer.BuildMessage();
            Assert.Equal(2, msg.FailedRows);
        }
    }
    
    [Fact]
    public async Task ProductImport_nodepartment()
    {
        var basemapping = GetBasicMapping();
        
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            //HasHeaderRecord = false,
        };
        using (var stream = new MemoryStream())
        using (var reader = new StreamReader(stream))
        await using (var writer = new StreamWriter(stream))
        using (var csv = new CsvReader(reader, config))
        {
            writer.WriteLine("Description1,PLU Type,Plu_No,UPC,Line1,Line2,LabelFormatNo1,Price,POS Description,Category,Scale Category,Departments,ShelfLife,EBT,Sell Beyond Zero,Show StockOn POS");
            writer.WriteLine("Description1,RandomWeight,337,00337,SMOKED,SIRLOIN STEAK,4,10.99,SMOKED SIRLOIN STEAK,2,1,,0,TRUE,TRUE,FALSE");
            writer.WriteLine("Description1,RandomWeight,337,00337,SMOKED,SIRLOIN STEAK,4,10.99,SMOKED SIRLOIN STEAK,2,1,2,0,TRUE,TRUE,FALSE");
            await writer.FlushAsync();
            stream.Position = 0;
            //Create a scalelabel type 
            var context = provider.GetRequiredService<DbContext>();
            await context.Set<ScaleLabelType>()
                .AddAsync(new ScaleLabelType() { Id = 4, Name = "4", LabelId = 4 });
            await context.SaveChangesAsync();
            
            await importer.Process(basemapping, csv, new List<long>() { 1 });

            var msg = importer.BuildMessage();

            Assert.Equal(1, msg.FailedRows);
        }
    }
    
    [Fact]
    public async Task ProductImport_NoFailed()
    {
        var basemapping = GetBasicMapping();
        
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            //HasHeaderRecord = false,
        };
        using (var stream = new MemoryStream())
        using (var reader = new StreamReader(stream))
        using (var writer = new StreamWriter(stream))
        using (var csv = new CsvReader(reader, config))
        {
            writer.WriteLine("Description1,PLU Type,Plu_No,UPC,Line1,Line2,LabelFormatNo1,Price,POS Description,Category,Scale Category,Departments,ShelfLife,EBT,Sell Beyond Zero,Show StockOn POS");
            writer.WriteLine("test1,RandomWeight,347,00347,SMOKED,SIRLOIN STEAK,1,10.99,SMOKED SIRLOIN STEAK,2,1,2,0,TRUE,TRUE,FALSE");
            writer.WriteLine("test1,RandomWeight,347,00347,SMOKED,SIRLOIN STEAK,1,10.99,SMOKED SIRLOIN STEAK,2,1,2,0,TRUE,TRUE,FALSE");
            writer.Flush();
            stream.Position = 0;
            
            //Create a scalelabel type 
            var context = provider.GetRequiredService<DbContext>();
            await context.Set<ScaleLabelType>()
                .AddAsync(new ScaleLabelType() { Id = 1, Name = "1", LabelId = 1 });
            await context.SaveChangesAsync();
            
            await importer.Process(basemapping, csv, new List<long>() { 1 });
            var msg = importer.BuildMessage();

            Assert.Equal(0, msg.FailedRows);
        }
    }
}