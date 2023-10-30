using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Handlers.Command.LoyaltyDiscount;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Commands.LoyaltyDiscountCommands;


public class LoyaltyDiscountDeleteRangeHandlerTest
{
    private readonly LoyaltyDiscountRepository _repository;
    private readonly LoyaltyDiscountDeleteRangeHandler _handler;

    public LoyaltyDiscountDeleteRangeHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(LoyaltyDiscountDeleteRangeHandlerTest))
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new LoyaltyDiscountRepository(TestsSingleton.Auth, context);
        var service = new LoyaltyDiscountService(_repository, Mock.Of<ISynchroService>(), Mock.Of<ILogger<LoyaltyDiscountService>>());

        _handler = new LoyaltyDiscountDeleteRangeHandler(
            Mock.Of<ILogger<LoyaltyDiscountDeleteRangeHandler>>(),
            service);       
    }  

    [Fact]
    public async Task DeleteRange_WithValidIdRange_ReturnResultTrue()
    {
        // Arrange        

        List<LoyaltyDiscount> loyaltyDiscounts = new List<LoyaltyDiscount>();

        for (int i = 1; i <= 3; i++)
        {
            loyaltyDiscounts.Add(new LoyaltyDiscount() { Name = "LoyaltyDiscount" + i });
        }
        
        await _repository.CreateRangeAsync(loyaltyDiscounts);

        List<long> isd = new List<long>(loyaltyDiscounts.Select(x=>x.Id));

        // Act

        var command = new LoyaltyDiscountDeleteRangeCommand(isd);

        var result = await _handler.Handle(command);       

        // Assert

        Assert.True(result.Data);
    }

    [Fact]
    public async Task DeleteRange_WithNotExistIdInRange_ReturnResultFalse()
    {
        // Arrange       

        List<LoyaltyDiscount> loyaltyDiscounts = new List<LoyaltyDiscount>();

        var store = new Store()
        {
            Id = 1,
            Name = "Store1",
        }; 
        await _repository.CreateAsync(store);

        for (int i = 1; i <= 3; i++)
        {
            loyaltyDiscounts.Add(new LoyaltyDiscount() { Name = "LoyaltyDiscount" + i, StoreId = 1 });
        }

        await _repository.CreateRangeAsync(loyaltyDiscounts);

        List<long> ids = new List<long> { 100, 101, 102 };
        // Act

        var command = new LoyaltyDiscountDeleteRangeCommand(ids);

        var result = await _handler.Handle(command);

        // Assert                

        Assert.False(result.Data);
    }

    [Fact]
    public async Task DeleteRange_WithInvalidId_ReturnResultFalse()
    {
        // Arrange       

        List<LoyaltyDiscount> loyaltyDiscounts = new List<LoyaltyDiscount>();

        for (int i = 1; i <= 3; i++)
        {
            loyaltyDiscounts.Add(new LoyaltyDiscount() { Name = "LoyaltyDiscount" + i });
        }

        await _repository.CreateRangeAsync(loyaltyDiscounts);

        List<long> ids = new List<long> { -1, -2, -3 };

        // Act
        
        var command = new LoyaltyDiscountDeleteRangeCommand(ids);

        var result = await _handler.Handle(command);               

        // Assert
                
        Assert.False(result.Data);
    }
}