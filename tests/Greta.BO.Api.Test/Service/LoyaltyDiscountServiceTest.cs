using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Service;

public class LoyaltyDiscountServiceTest
{
    readonly LoyaltyDiscountRepository _repository;
    readonly LoyaltyDiscountService _service;

    public LoyaltyDiscountServiceTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();

        var options = builder.UseInMemoryDatabase(nameof(LoyaltyDiscountServiceTest)).Options;
        
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new LoyaltyDiscountRepository(TestsSingleton.Auth, context);
        _service = new LoyaltyDiscountService(_repository, Mock.Of<ISynchroService>(), Mock.Of<ILogger<LoyaltyDiscountService>>());
        
    }        

    [Fact]
    public async Task GetRemainStores_NormalCall_ResultListStores()
    {
        // Arrange   

        List<Store> stores = new List<Store>();
        for (int i = 0; i < 3; i++)
        {
            stores.Add(new Store() { 
                Name = "Store",
            });
        }

        await _repository.CreateRangeAsync(stores);

        var loyaltyDiscount = new LoyaltyDiscount() { Name = "GetRemainStores_NormalCall_ResultListStores" };

        await _repository.CreateAsync(loyaltyDiscount);

        // Act

        var result = await _service.GetRemainStores();

        // Assert
        Assert.IsType<List<Store>>(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetRemainStores_NormalCall_ResultEmpty()
    {
        // Arrange   

        var store = new Store()
        {
            Name = "Store",
        };       

        var storeId = await _repository.CreateAsync(store);

        var loyaltyDiscount = new LoyaltyDiscount() { Name = "GetRemainStores_NormalCall_ResultListStores", StoreId = storeId.Id };

        await _repository.CreateAsync(loyaltyDiscount);

        // Act

        var result = await _service.GetRemainStores();

        // Assert
        Assert.IsType<List<Store>>(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByStore_IdOutOfRange_ReturnNull()
    {
        // Arrange

        var store = new Store() { Name = "Store" };
        var storeId = await _repository.CreateAsync(store);

        var name = "GetByStore_IdOutOfRange_ThrowBusinessLogicException";
        var loyaltyDiscount = new LoyaltyDiscount()
        {
            Name = name,
            StoreId = storeId.Id

        };
        await _repository.CreateAsync(loyaltyDiscount);

        // Act
        
        var result = await _service.GetByStore(-1);    

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByStore_ValidId_ReturnObject()
    {
        // Arrange

        var store = new Store() { Name = "Store" };
        var storeId = await _repository.CreateAsync(store);

        var name = "GetByStore_IdOutOfRange_ThrowBusinessLogicException";
        var loyaltyDiscount = new LoyaltyDiscount()
        {
            Name = name,
            StoreId = storeId.Id

        };
        await _repository.CreateAsync(loyaltyDiscount);

        // Act

        var result = await _service.GetByStore(storeId.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(loyaltyDiscount.StoreId, result.StoreId);
    }   
}