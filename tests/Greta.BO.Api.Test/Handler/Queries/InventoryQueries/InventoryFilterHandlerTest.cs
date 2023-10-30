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


public class InventoryFilterHandlerTest
{
    private readonly StoreProductRepository _repository;
    private readonly QtyHandTrackRepository _repositoryQty;

    private readonly InventoryFilterHandler _handler;

    private readonly ShelfTagService _shelfTagService;


    public InventoryFilterHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(InventoryFilterHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new StoreProductRepository(TestsSingleton.Auth, context);
        _repositoryQty = new QtyHandTrackRepository(TestsSingleton.Auth, context);        

        _shelfTagService = new ShelfTagService(Mock.Of<IShelfTagRepository>(), Mock.Of<ILogger<ShelfTagService>>());        

        var service = new StoreProductService(_repository, _repositoryQty, _shelfTagService, Mock.Of<IRoundingTableService>(),Mock.Of<ISynchroService>(), Mock.Of<MediatR.IMediator>(),Mock.Of<ILogger<StoreProductService>>());

        _handler = new InventoryFilterHandler(
            service,
            TestsSingleton.Mapper);
    }

    [Fact]
    public async Task FilterSpec_NormalCall_ResultOk()
    {
        // Arrange      

        var product = new Product() { Name = "Product", UPC = "UPC", State = true };
        var prod = await _repository.CreateAsync(product);

        var store = new Store() { Name = "Store", State = true };
        var str = await _repository.CreateAsync(store);

        var binLocation = new BinLocation() { Name = "BinLocation" , State = true };
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
                StoreId = str.Id                
            });
        }

        var id = await _repository.CreateRangeAsync<StoreProduct>(storeProducts);

        var filter = new InventorySearchModel();

        var query = new InventoryFilterQuery(str.Id, 1, 1, filter);
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
            var query = new InventoryFilterQuery(1, -1, 1, filter);

            var result = await _handler.Handle(query);
        });

        // Assert
        Assert.Equal("Page parameter out of bounds", exception.Message);
    }
}