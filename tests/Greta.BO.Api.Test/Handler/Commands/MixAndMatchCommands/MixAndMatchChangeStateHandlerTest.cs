using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Command.MixAndMatch;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Commands.MixAndMatchCommands;


public class MixAndMatchChangeStateHandlerTest
{
    private readonly MixAndMatchRepository _repository;
    private readonly MixAndMatchChangeStateHandler _handler;    

    public MixAndMatchChangeStateHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(MixAndMatchChangeStateHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new MixAndMatchRepository(TestsSingleton.Auth, context);
        var service = new MixAndMatchService(_repository, Mock.Of<ILogger<MixAndMatchService>>());

        _handler = new MixAndMatchChangeStateHandler(
            Mock.Of<ILogger<MixAndMatchChangeStateHandler>>(),
            service);        
    }

    [Fact]
    public async Task PutChangeState_NormalCall_ResultTrue()
    {
        // Arrange

        var mixAndMatch = new MixAndMatch()
        {
            Name = "MixAndMatch PutChangeState_NormalCall_ResultTrue ",  
        };             
       
        var id = await _repository.CreateAsync(mixAndMatch);
        
        var command = new MixAndMatchChangeStateCommand(id , !mixAndMatch.State);

        // Act
        var result = await _handler.Handle(command);

        // Assert
       
        Assert.True(result.Data);
    }

    [Fact]
    public async Task PutChangeState_WithIdNotExist_ResultFalse()
    {
        // Arrange

        var mixAndMatch = new MixAndMatch()
        {
            Name = "MixAndMatch PutChangeState_WithIdNotExist_ResultFalse",            
        };

        await _repository.CreateAsync(mixAndMatch);        

        var command = new MixAndMatchChangeStateCommand(5, !mixAndMatch.State);

        // Act
        var result = await _handler.Handle(command);

        // Assert

        Assert.False(result.Data);
    }

    [Fact]
    public async Task PutChangeState_WithIdLessThanCiro_ResultBusinessLogicException()
    {
        // Arrange

        var mixAndMatch = new MixAndMatch()
        {
            Name = "MixAndMatch PutChangeState_WithIdLessThanCiro_ResultBusinessLogicException"
        };

        await _repository.CreateAsync(mixAndMatch);        

        // Act

        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var command = new MixAndMatchChangeStateCommand(-1, !mixAndMatch.State);

            var result = await _handler.Handle(command);
        });

        // Assert

        Assert.Equal("Id parameter out of bounds", exception.Message);
    }
}