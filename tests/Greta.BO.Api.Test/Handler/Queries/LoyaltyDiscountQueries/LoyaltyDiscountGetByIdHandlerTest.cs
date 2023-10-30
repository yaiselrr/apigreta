using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Handlers.Queries.LoyaltyDiscount;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Queries.LoyaltyDiscountQueries;

public class LoyaltyDiscountGetByIdHandlerTest
{
    private readonly LoyaltyDiscountRepository _repository;
    private readonly LoyaltyDiscountGetByIdHandler _handler;

    public LoyaltyDiscountGetByIdHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(LoyaltyDiscountFilterHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new LoyaltyDiscountRepository(TestsSingleton.Auth, context);
        var service = new LoyaltyDiscountService(_repository, Mock.Of<ISynchroService>(), Mock.Of<ILogger<LoyaltyDiscountService>>());

        _handler = new LoyaltyDiscountGetByIdHandler(
            service,
            TestsSingleton.Mapper);
    }
    
    [Fact]
    public async Task GetById_CallWithInvalidNumber_ReturnNull()
    {
        // Arrange
        
        var name = "Loyalty Discount";
        var loyaltyDiscount = new LoyaltyDiscount()
        {
            Name = name
        };
        await _repository.CreateAsync(loyaltyDiscount);
        // Act
        
        var query = new LoyaltyDiscountGetByIdQuery(-13);

        var result = await _handler.Handle(query);        

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task GetById_CallWithValidNumberButNotPresentId_ReturnNULL()
    {
        // Arrange
        var name = "GetById_CallWithValidNumberButNotPresentId_ReturnNULL";
        var loyaltyDiscount = new LoyaltyDiscount()
        {
            Name = name
        };
        await _repository.CreateAsync(loyaltyDiscount);
        // Act
        var query = new LoyaltyDiscountGetByIdQuery(1000);
        var result = await _handler.Handle(query);

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task GetById_CallWithValidNumber_ReturnObjectWithSameID()
    {
        // Arrange
        var name = "GetById_CallWithValidNumber_ReturnObjectWithSameID";
        var loyaltyDiscount = new LoyaltyDiscount()
        {
            Name = name
        };
        var id = await _repository.CreateAsync(loyaltyDiscount);
        // Act
        var query = new LoyaltyDiscountGetByIdQuery(id);
        var result = await _handler.Handle(query);

        // Assert
        Assert.Equal(id, result.Data.Id);
    }
}