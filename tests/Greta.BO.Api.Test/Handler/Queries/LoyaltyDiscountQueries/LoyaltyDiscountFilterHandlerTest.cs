
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Queries.LoyaltyDiscount;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Queries.LoyaltyDiscountQueries;


public class LoyaltyDiscountFilterHandlerTest
{
    private readonly LoyaltyDiscountRepository _repository;
    private readonly LoyaltyDiscountFilterHandler _handler;

    public LoyaltyDiscountFilterHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(LoyaltyDiscountFilterHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new LoyaltyDiscountRepository(TestsSingleton.Auth, context);
        var service = new LoyaltyDiscountService(_repository, Mock.Of<ISynchroService>(), Mock.Of<ILogger<LoyaltyDiscountService>>());

        _handler = new LoyaltyDiscountFilterHandler(
            Mock.Of<ILogger<LoyaltyDiscountFilterHandler>>(),
            service,
            TestsSingleton.Mapper);
    }

    [Fact]
    public async Task FilterSpec_NormalCall_ResultOk()
    {
        // Arrange
        var loyaltyDiscounts = new List<LoyaltyDiscount>();
        for (var i = 0; i < 3; i++)
        {
            loyaltyDiscounts.Add(new LoyaltyDiscount()
            {
                Name = "LoyaltyDiscounte Name1 " + i
            });
        }

        await _repository.CreateRangeAsync(loyaltyDiscounts);
        var filter = new LoyaltyDiscountSearchModel() { Search = "LoyaltyDiscounte"};
        var query = new LoyaltyDiscountFilterQuery(1,3, filter);
        // Act
        var result = await _handler.Handle(query);

        // Assert
        Assert.Equal(3, result.Data.TotalItems);
    }

    [Fact]
    public async Task FilterSpec_CallWithNegativeValues_ResultBusinessObjectException()
    {
        // Arrange
        var loyaltyDiscounts = new List<LoyaltyDiscount>();
        for (var i = 0; i < 3; i++)
        {
            loyaltyDiscounts.Add(new LoyaltyDiscount()
            {
                Name = "LoyaltyDiscount Name " + i
            });
        }

        await _repository.CreateRangeAsync(loyaltyDiscounts);
        var filter = new LoyaltyDiscountSearchModel();

        // Act

        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var query = new LoyaltyDiscountFilterQuery(-1, 3, filter);

            await _handler.Handle(query);
        });

        // Assert
        
        Assert.IsType<BusinessLogicException>(exception);
    }

    [Fact]
    public async Task FilterSpec_CallWithNotFoundValue_ResultEmpty()
    {
        // Arrange
        var loyaltyDiscounts = new List<LoyaltyDiscount>();
        for (var i = 0; i < 3; i++)
        {
            loyaltyDiscounts.Add(new LoyaltyDiscount()
            {
                Name = "LoyaltyDiscount Name " + i
            });
        }

        await _repository.CreateRangeAsync(loyaltyDiscounts);
        var filter = new LoyaltyDiscountSearchModel();

        // Act
               
        var query = new LoyaltyDiscountFilterQuery(100, 3, filter);

        var result = await _handler.Handle(query);
        

        // Assert

        Assert.Empty(result.Data.Data);
    }
}