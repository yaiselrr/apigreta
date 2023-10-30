using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Command.LoyaltyDiscount;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Commands.LoyaltyDiscountCommands;


public class LoyaltyDiscountDeleteHandlerTest
{
    private readonly LoyaltyDiscountRepository _repository;
    private readonly LoyaltyDiscountDeleteHandler _handler;    

    public LoyaltyDiscountDeleteHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(LoyaltyDiscountDeleteHandlerTest)).ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new LoyaltyDiscountRepository(TestsSingleton.Auth, context);
        var service = new LoyaltyDiscountService(_repository, Mock.Of<ISynchroService>(), Mock.Of<ILogger<LoyaltyDiscountService>>());

        _handler = new LoyaltyDiscountDeleteHandler(
            Mock.Of<ILogger<LoyaltyDiscountDeleteHandler>>(),
            service);       
    }

    [Fact]
    public async Task Delete_WithSameId_ReturnResultTrue()
    {
        // Arrange

        var loyaltyDiscount = new LoyaltyDiscount()
        {
            Name = "LoyaltyDiscount Name "
        };             
       
        var id = await _repository.CreateAsync(loyaltyDiscount);

        var command = new LoyaltyDiscountDeleteCommand(id);

        // Act
        var result = await _handler.Handle(command);

        // Assert
               
        Assert.True(result.Data);
    }

    [Fact]
    public async Task Delete_WithNoExistId_ReturnResultFalse()
    {
        // Arrange

        var loyaltyDiscount = new LoyaltyDiscount()
        {
            Name = "LoyaltyDiscount Name "
        };

        await _repository.CreateAsync(loyaltyDiscount);

        var command = new LoyaltyDiscountDeleteCommand(10);

        // Act
        var result = await _handler.Handle(command);

        // Assert
                
        Assert.False(result.Data);
    }

    [Fact]
    public async Task Delete_WithIdLessThanCiro_ResultBusinessLogicException()
    {
        // Arrange

        var loyaltyDiscount = new LoyaltyDiscount()
        {
            Name = "LoyaltyDiscount Name "
        };

        await _repository.CreateAsync(loyaltyDiscount);
        
        // Act

        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var command = new LoyaltyDiscountDeleteCommand(-2);

            await _handler.Handle(command);
        });        

        // Assert
                
        Assert.Equal("Id parameter out of bounds", exception.Message);
    }
}