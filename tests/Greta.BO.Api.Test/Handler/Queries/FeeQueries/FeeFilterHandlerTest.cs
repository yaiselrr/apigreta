
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Queries.Fee;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Queries.FeeQueries;


public class FeeFilterHandlerTest
{
    private readonly FeeRepository _repository;
    private readonly FeeFilterHandler _handler;

    public FeeFilterHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(FeeFilterHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new FeeRepository(TestsSingleton.Auth, context);
        var service = new FeeService(_repository, Mock.Of<ILogger<FeeService>>());

        _handler = new FeeFilterHandler(
            Mock.Of<ILogger<FeeFilterHandler>>(),
            service,
            TestsSingleton.Mapper);
    }

    [Fact]
    public async Task FilterSpec_NormalCall_ResultOk()
    {
        // Arrange
        var fees = new List<Fee>();
        for (var i = 0; i < 3; i++)
        {
            fees.Add(new Fee()
            {
                Name = "Fee Name " + i
            });
        }

        await _repository.CreateRangeAsync(fees);
        var filter = new FeeSearchModel();
        var query = new FeeFilterQuery(1,3, filter);
        // Act
        var result = await _handler.Handle(query);

        // Assert
        Assert.Equal(3, result.Data.Data.Count);
    }

    [Fact]
    public async Task FilterSpec_CallWithNegativeValues_ResultBusinessObjectException()
    {
        // Arrange
        var fees = new List<Fee>();
        for (var i = 0; i < 3; i++)
        {
            fees.Add(new Fee()
            {
                Name = "Fee Name " + i
            });
        }

        await _repository.CreateRangeAsync(fees);
        var filter = new FeeSearchModel();

        // Act

        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var query = new FeeFilterQuery(-1, 3, filter);

            await _handler.Handle(query);
        });

        // Assert
        
        Assert.IsType<BusinessLogicException>(exception);
    }

    [Fact]
    public async Task FilterSpec_CallWithNotFoundValue_ResultEmpty()
    {
        // Arrange
        var fees = new List<Fee>();
        for (var i = 0; i < 3; i++)
        {
            fees.Add(new Fee()
            {
                Name = "Fee Name " + i
            });
        }

        await _repository.CreateRangeAsync(fees);
        var filter = new FeeSearchModel();

        // Act
               
        var query = new FeeFilterQuery(100, 3, filter);

        var result = await _handler.Handle(query);
        

        // Assert

        Assert.Empty(result.Data.Data);
    }
}