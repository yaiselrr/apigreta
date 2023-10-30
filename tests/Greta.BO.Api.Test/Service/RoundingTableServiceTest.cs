using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Service;

public class RoundingTableServiceTest
{
    readonly RoundingTableRepository _repository;
    readonly RoundingTableService _service;

    public RoundingTableServiceTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(RoundingTableServiceTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new RoundingTableRepository(TestsSingleton.Auth, context);
        _service = new RoundingTableService(_repository, Mock.Of<ILogger<RoundingTableService>>());
    }
    
    [Fact]
    public async Task ChangeBy_NormalCall_ResultOk()
    {
        // Arrange
        var endBy = 1;
        var changeBy = 5;
        var round = new RoundingTable()
        {
            Id = 1,
            EndWith = 1,
            ChangeBy = 5,
        };
        var id = await _repository.CreateAsync(round);
        // Act
        var result = await _service.ChangeBy(endBy);

        // Assert
        Assert.Equal(changeBy, result);
    }
    
    [Fact]
    public async Task ChangeBy_NotExit_ResultLessThanZero()
    {
        // Arrange
        var endBy = 1;
        var changeBy = 5;
        var round = new RoundingTable()
        {
            Id = 2,
            EndWith = 1,
            ChangeBy = 5,
        };
        var id = await _repository.CreateAsync(round);
        // Act
        var result = await _service.ChangeBy(endBy + 1);

        // Assert
        Assert.True(result < 0);
    }
}