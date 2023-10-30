using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Queries.InventoryQueries;
using Greta.BO.BusinessLogic.Handlers.Queries.StoreProduct;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.Core.Models.Pager;
using Hangfire.Dashboard;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Queries.InventoryQueries;


public class InventorySuggestedFilterHandlerTest
{
    private readonly StoreProductRepository _repository;
    private readonly QtyHandTrackRepository _repositoryQty;

    private readonly InventorySuggestedFilterHandler _handler;

    private readonly ShelfTagService _shelfTagService;
    private readonly IShelfTagRepository _repositoryShel;

    public InventorySuggestedFilterHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(InventorySuggestedFilterHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new StoreProductRepository(TestsSingleton.Auth, context);
        _repositoryQty = new QtyHandTrackRepository(TestsSingleton.Auth, context);

        //_repositoryShel = new IShelfTagRepository(TestsSingleton.Auth, context);

        _shelfTagService = new ShelfTagService(Mock.Of<IShelfTagRepository>(), Mock.Of<ILogger<ShelfTagService>>());

        //var synchroService = new SynchroService(Mock.Of<IStoreRepository>(), Mock.Of<ISynchroRepository>(), Mock.Of<ISynchroDetailRepository>(), Mock.Of<ILogger<SynchroService>>());

        var service = new StoreProductService(_repository, _repositoryQty, _shelfTagService, Mock.Of<IRoundingTableService>(),Mock.Of<ISynchroService>(), Mock.Of<MediatR.IMediator>(),Mock.Of<ILogger<StoreProductService>>());

        _handler = new InventorySuggestedFilterHandler(
            service,
            TestsSingleton.Mapper);
    }

    [Fact]
    public async Task FilterSpec_NormalCall_ResultOk()
    {
        // Arrange      

        var vendor = new Vendor() { Name = "Vendor" };
        var vend = await _repository.CreateAsync(vendor);

        var product = new Product() { Name = "Product", UPC = "UPC", State = true };
        var prod = await _repository.CreateAsync(product);

        var vendorProducts = new List<VendorProduct>();
        vendorProducts.Add(new VendorProduct() { VendorId = vend.Id, ProductId = prod.Id, ProductCode = "Product Code" });
        await _repository.CreateRangeAsync(vendorProducts);

        var store = new Store() { Name = "Store", State = true};
        var str = await _repository.CreateAsync(store);

        var binLocation = new BinLocation() { Name = "BinLocation" , State = true};
        var binLoc = await _repository.CreateAsync(binLocation);

        var storeProducts = new List<StoreProduct>();
        for (var i = 0; i < 3; i++)
        {
            storeProducts.Add(new StoreProduct()
            {
                ProductId = prod.Id,
                OrderAmount = i,
                Cost = 5,
                BinLocationId = binLocation.Id,
                BatchOldPrice = 2,
                GrossProfit = 3,
                QtyHand = 2,
                StoreId = str.Id,
                OrderTrigger = 5                
            });
        }

        var id = await _repository.CreateRangeAsync<StoreProduct>(storeProducts);

        var filter = new InventorySearchModel();

        var query = new InventorySuggestedFilterQuery(str.Id, 1,  1, 1, filter);
        // Act
        var result = await _handler.Handle(query);

        // Assert
        Assert.IsType<Pager<InventoryResponseModel>>(result.Data);
        Assert.NotEmpty(result.Data.Pages);
    }


    [Fact]
    public async Task FilterSpec_NormalCall_WithCurrentPageOrPageSize_LessThanCiro_ResultBusinessException()
    {
        // Arrange

        var filter = new InventorySearchModel();

        // Act        

        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var query = new InventorySuggestedFilterQuery(1, 1, -1, 1, filter);

            var result = await _handler.Handle(query);
        });

        // Assert
        Assert.Equal("Page parameter out of bounds", exception.Message);
    }
}