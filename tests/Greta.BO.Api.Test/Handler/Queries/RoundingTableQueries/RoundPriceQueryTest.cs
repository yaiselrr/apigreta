using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Handlers.Queries.RoundingTableQueries;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Queries.RoundingTableQueries;

public class RoundPriceQueryTest
{
    private readonly RoundingTableRepository _repository;
    private readonly RoundPriceHandler _handler;

    public RoundPriceQueryTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(RoundPriceQueryTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new RoundingTableRepository(TestsSingleton.Auth, context);
        var service = new RoundingTableService(_repository, Mock.Of<ILogger<RoundingTableService>>());

        _handler = new RoundPriceHandler(service);
    }

    [Fact]
    public async Task ChangeBy_NotExit_ResultLessThanZero()
    {
        // Arrange
        var endBy = 1;
        var changeBy = 5;
        var receive = 92.51M;
        var obtain = 92.55M;
        var round = new RoundingTable()
        {
            Id = 2,
            EndWith = 1,
            ChangeBy = 5,
        };
        var id = await _repository.CreateAsync(round);
        // Act
        var result = await _handler.Handle(new RoundPriceQuery(receive));

        // Assert
        Assert.Equal(obtain, result.Data);
    }
}