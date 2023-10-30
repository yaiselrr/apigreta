
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Handlers.Queries.MixAndMatch;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Queries.MixAndMatchQueries;


public class MixAndMatchGetAllHandlerTest
{
    private readonly MixAndMatchRepository _repository;
    private readonly MixAndMatchGetAllHandler _handler;

    public MixAndMatchGetAllHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(MixAndMatchGetAllHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new MixAndMatchRepository(TestsSingleton.Auth, context);
        var service = new MixAndMatchService(_repository, Mock.Of<ILogger<MixAndMatchService>>());

        _handler = new MixAndMatchGetAllHandler(
            service,
            TestsSingleton.Mapper);
    }

    [Fact]
    public async Task GetAll_NormalCall_ResultOk()
    {
        // Arrange
        var mixAndMatches = new List<MixAndMatch>();
        for (var i = 0; i < 3; i++)
        {
            mixAndMatches.Add(new MixAndMatch()
            {
                Name = "MixAndMatch Name " + i,
                State = true
            });
        }

        await _repository.CreateRangeAsync(mixAndMatches);
                
        var query = new MixAndMatchGetAllQuery();
        // Act
        var result = await _handler.Handle(query);

        // Assert
        Assert.Equal(mixAndMatches.Count, result.Data.Count);
    }
       

    [Fact]
    public async Task GetAllSpec_CallNormal_ResultEmpty()
    {
        // Arrange
               
        // Act
               
        var query = new MixAndMatchGetAllQuery();

        var result = await _handler.Handle(query);        

        // Assert

        Assert.Empty(result.Data);
    }
}