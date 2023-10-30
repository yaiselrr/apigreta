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

public class LoyaltyDiscountChangeStateHandlerTest
{
    private readonly LoyaltyDiscountRepository _repository;
    private readonly LoyaltyDiscountChangeStateHandler _handler;    

    public LoyaltyDiscountChangeStateHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(LoyaltyDiscountChangeStateHandlerTest))
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new LoyaltyDiscountRepository(TestsSingleton.Auth, context);
        var service = new LoyaltyDiscountService(_repository, Mock.Of<ISynchroService>(), Mock.Of<ILogger<LoyaltyDiscountService>>());

        _handler = new LoyaltyDiscountChangeStateHandler(            
            Mock.Of<ILogger<LoyaltyDiscountChangeStateHandler>>(),
            service
            );        
    }

    [Fact]
    public async Task PutChangeState_NormalCall_ResultTrue()
    {
        // Arrange

        var loyaltyDiscount = new LoyaltyDiscount()
        {
            Name = "LoyaltyDiscount PutChangeState_NormalCall_ResultTrue ",  
        };             
       
        var id = await _repository.CreateAsync(loyaltyDiscount);
        
        var command = new LoyaltyDiscountChangeStateCommand(id , !loyaltyDiscount.State);

        // Act
        var result = await _handler.Handle(command);

        // Assert
       
        Assert.True(result.Data);
    }

    [Fact]
    public async Task PutChangeState_WithIdNotExist_ResultFalse()
    {
        // Arrange

        var loyaltyDiscount = new LoyaltyDiscount()
        {
            Name = "LoyaltyDiscount PutChangeState_WithIdNotExist_ResultFalse",            
        };

        await _repository.CreateAsync(loyaltyDiscount);        

        var command = new LoyaltyDiscountChangeStateCommand(5, !loyaltyDiscount.State);

        // Act
        var result = await _handler.Handle(command);

        // Assert

        Assert.False(result.Data);
    }

    [Fact]
    public async Task PutChangeState_WithIdLessThanCiro_ResultBusinessLogicException()
    {
        // Arrange

        var loyaltyDiscount = new LoyaltyDiscount()
        {
            Name = "LoyaltyDiscount PutChangeState_WithIdLessThanCiro_ResultBusinessLogicException"
        };

        await _repository.CreateAsync(loyaltyDiscount);        

        // Act

        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var command = new LoyaltyDiscountChangeStateCommand(-1, !loyaltyDiscount.State);

            await _handler.Handle(command);
        });

        // Assert

        Assert.Equal("Id parameter out of bounds", exception.Message);
    }
}