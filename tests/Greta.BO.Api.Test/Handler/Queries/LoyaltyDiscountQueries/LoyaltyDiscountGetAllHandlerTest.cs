
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


public class LoyaltyDiscountGetAllHandlerTest
{
    private readonly LoyaltyDiscountRepository _repository;
    private readonly LoyaltyDiscountGetAllHandler _handler;

    public LoyaltyDiscountGetAllHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(LoyaltyDiscountGetAllHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new LoyaltyDiscountRepository(TestsSingleton.Auth, context);
        var service = new LoyaltyDiscountService(_repository, Mock.Of<ISynchroService>(), Mock.Of<ILogger<LoyaltyDiscountService>>());

        _handler = new LoyaltyDiscountGetAllHandler(
            service,
            TestsSingleton.Mapper);
    }

    [Fact]
    public async Task GetAll_NormalCall_ResultOk()
    {
        // Arrange
        var loyaltyDiscounts = new List<LoyaltyDiscount>();
        for (var i = 0; i < 3; i++)
        {
            loyaltyDiscounts.Add(new LoyaltyDiscount()
            {
                Name = "LoyaltyDiscount Name " + i,
                State = true
            });
        }

        await _repository.CreateRangeAsync(loyaltyDiscounts);
                
        var query = new LoyaltyDiscountGetAllQuery();
        // Act
        var result = await _handler.Handle(query);

        // Assert
        Assert.Equal(loyaltyDiscounts.Count, result.Data.Count);
    }
}