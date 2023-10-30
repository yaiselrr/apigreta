using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Handlers.Command.MixAndMatch;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Commands.MixAndMatchCommands;


public class MixAndMatchDeleteRangeHandlerTest
{
    private readonly MixAndMatchRepository _repository;
    private readonly MixAndMatchDeleteRangeHandler _handler;

    public MixAndMatchDeleteRangeHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(MixAndMatchDeleteRangeHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new MixAndMatchRepository(TestsSingleton.Auth, context);
        var service = new MixAndMatchService(_repository, Mock.Of<ILogger<MixAndMatchService>>());

        _handler = new MixAndMatchDeleteRangeHandler(
            Mock.Of<ILogger<MixAndMatchDeleteRangeHandler>>(),
            service);       
    }  

    [Fact]
    public async Task DeleteRange_WithValidIdRange_ReturnResultTrue()
    {
        // Arrange        

        List<MixAndMatch> mixAndMatches = new List<MixAndMatch>();

        for (int i = 1; i <= 3; i++)
        {
            mixAndMatches.Add(new MixAndMatch() { Name = "MixAndMatch" + i });
        }
        
        await _repository.CreateRangeAsync(mixAndMatches);

        List<long> isd = new List<long>(mixAndMatches.Select(x=>x.Id));

        // Act

        var command = new MixAndMatchDeleteRangeCommand(isd);

        var result = await _handler.Handle(command);       

        // Assert

        Assert.True(result.Data);
    }

    [Fact]
    public async Task DeleteRange_WithNotExistIdInRange_ReturnResultFalse()
    {
        // Arrange       

        List<MixAndMatch> mixAndMatches = new List<MixAndMatch>();

        for (int i = 1; i <= 3; i++)
        {
            mixAndMatches.Add(new MixAndMatch() { Name = "MixAndMatch" + i });
        }

        await _repository.CreateRangeAsync(mixAndMatches);

        List<long> ids = new List<long> { 100, 101, 102 };
        // Act

        var command = new MixAndMatchDeleteRangeCommand(ids);

        var result = await _handler.Handle(command);

        // Assert                

        Assert.False(result.Data);
    }

    [Fact]
    public async Task DeleteRange_WithIdinvalidId_ReturnResultFalse()
    {
        // Arrange       

        List<MixAndMatch> mixAndMatches = new List<MixAndMatch>();

        for (int i = 1; i <= 3; i++)
        {
            mixAndMatches.Add(new MixAndMatch() { Name = "MixAndMatch" + i });
        }

        await _repository.CreateRangeAsync(mixAndMatches);

        List<long> ids = new List<long> { -1, -2, -3 };

        // Act
        
        var command = new MixAndMatchDeleteRangeCommand(ids);

        var result = await _handler.Handle(command);               

        // Assert
                
        Assert.False(result.Data);
    }
}