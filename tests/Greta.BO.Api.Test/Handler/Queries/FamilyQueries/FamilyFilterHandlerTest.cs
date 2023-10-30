
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Queries.Family;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Queries.FamilyQueries;


public class FamilyFilterHandlerTest
{
    private readonly FamilyRepository _repository;
    private readonly FamilyFilterHandler _handler;

    public FamilyFilterHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(FamilyFilterHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new FamilyRepository(TestsSingleton.Auth, context);
        var service = new FamilyService(_repository, Mock.Of<ILogger<FamilyService>>(), Mock.Of<ISynchroService>());

        _handler = new FamilyFilterHandler(
            service,
            TestsSingleton.Mapper);
    }

    [Fact]
    public async Task FilterSpec_NormalCall_ResultOk()
    {
        // Arrange
        var fams = new List<Family>();
        for (var i = 0; i < 10; i++)
        {
            fams.Add(new Family()
            {
                Name = "Family Name " + i
            });
        }

        var id = await _repository.CreateRangeAsync<Family>(fams);
        var filter = new FamilySearchModel();
        var query = new FamilyFilterQuery(1,10, filter);
        // Act
        var result = await _handler.Handle(query);

        // Assert
        Assert.Equal(10, result.Data.Data.Count);
    }
}