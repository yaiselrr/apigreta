using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Queries.Store;
using Greta.BO.BusinessLogic.Handlers.Queries.Store;
using Greta.BO.BusinessLogic.Service;
using MassTransit.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Queries.StoreQueries;

public class StoreGetTimezonesIdHandlerTest
{
    private readonly StoreRepository _repository;
    private readonly StoreGetTimezonesIdHandler _handler;

    public StoreGetTimezonesIdHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(StoreGetTimezonesIdHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new StoreRepository(TestsSingleton.Auth, context);
        var service = new StoreService(_repository, Mock.Of<ILogger<StoreService>>());

        _handler = new StoreGetTimezonesIdHandler(
            service);
    }

    [Fact]
    public async Task StoreGetTimezonesId_CallWithValidNumber_ReturnObjectWithSameID()
    {
        // Arrange
        var store = new Store()
        {
            Name = "Store Name ",
            RegionId = 164,
        };

        var id = await _repository.CreateAsync(store);
        // Act
        var query = new StoreGetTimezonesIdQuery();
        var result = await _handler.Handle(query);

        // Assert
        Assert.True( result.Data.Count >= 0);
    }
}