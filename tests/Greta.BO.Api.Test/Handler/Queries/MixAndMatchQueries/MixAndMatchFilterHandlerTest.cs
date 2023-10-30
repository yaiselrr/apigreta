
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Queries.MixAndMatch;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Queries.MixAndMatchQueries;


public class MixAndMatchFilterHandlerTest
{
    private readonly MixAndMatchRepository _repository;
    private readonly MixAndMatchFilterHandler _handler;

    public MixAndMatchFilterHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(MixAndMatchFilterHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new MixAndMatchRepository(TestsSingleton.Auth, context);
        var service = new MixAndMatchService(_repository, Mock.Of<ILogger<MixAndMatchService>>());

        _handler = new MixAndMatchFilterHandler(
            Mock.Of<ILogger<MixAndMatchFilterHandler>>(),
            service,
            TestsSingleton.Mapper);
    }

    [Fact]
    public async Task FilterSpec_NormalCall_ResultOk()
    {
        // Arrange
        var mixAndMatches = new List<MixAndMatch>();
        for (var i = 0; i < 3; i++)
        {
            mixAndMatches.Add(new MixAndMatch()
            {
                Name = "MixAndMatch Name " + i
            });
        }

        await _repository.CreateRangeAsync(mixAndMatches);
        var filter = new MixAndMatchSearchModel();
        var query = new MixAndMatchFilterQuery(1,3, filter);
        // Act
        var result = await _handler.Handle(query);

        // Assert
        Assert.Equal(3, result.Data.Data.Count);
    }

    [Fact]
    public async Task FilterSpec_CallWithNegativeValues_ResultBusinessObjectException()
    {
        // Arrange
        var mixAndMatches = new List<MixAndMatch>();
        for (var i = 0; i < 3; i++)
        {
            mixAndMatches.Add(new MixAndMatch()
            {
                Name = "MixAndMatch Name " + i
            });
        }

        await _repository.CreateRangeAsync(mixAndMatches);
        var filter = new MixAndMatchSearchModel();

        // Act

        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var query = new MixAndMatchFilterQuery(-1, 3, filter);

            await _handler.Handle(query);
        });

        // Assert
        
        Assert.IsType<BusinessLogicException>(exception);
    }

    [Fact]
    public async Task FilterSpec_CallWithNotFoundValue_ResultEmpty()
    {
        // Arrange
        var mixAndMatches = new List<MixAndMatch>();
        for (var i = 0; i < 3; i++)
        {
            mixAndMatches.Add(new MixAndMatch()
            {
                Name = "MixAndMatch Name " + i
            });
        }

        await _repository.CreateRangeAsync(mixAndMatches);
        var filter = new MixAndMatchSearchModel();

        // Act
               
        var query = new MixAndMatchFilterQuery(100, 3, filter);

        var result = await _handler.Handle(query);
        

        // Assert

        Assert.Empty(result.Data.Data);
    }
}