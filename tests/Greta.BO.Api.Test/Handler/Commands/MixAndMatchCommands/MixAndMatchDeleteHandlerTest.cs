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


public class MixAndMatchDeleteHandlerTest
{
    private readonly MixAndMatchRepository _repository;
    private readonly MixAndMatchDeleteHandler _handler;    

    public MixAndMatchDeleteHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(MixAndMatchDeleteHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new MixAndMatchRepository(TestsSingleton.Auth, context);
        var service = new MixAndMatchService(_repository, Mock.Of<ILogger<MixAndMatchService>>());

        _handler = new MixAndMatchDeleteHandler(
            Mock.Of<ILogger<MixAndMatchDeleteHandler>>(),
            service);       
    }

    [Fact]
    public async Task Delete_WithSameId_ReturnResultTrue()
    {
        // Arrange

        var mixAndMatch = new MixAndMatch()
        {
            Name = "MixAndMatch Name "
        };             
       
        var id = await _repository.CreateAsync(mixAndMatch);

        var command = new MixAndMatchDeleteCommand(id);

        // Act
        var result = await _handler.Handle(command);

        // Assert
               
        Assert.True(result.Data);
    }

    [Fact]
    public async Task Delete_WithNoExistId_ReturnResultFalse()
    {
        // Arrange

        var mixAndMatch = new MixAndMatch()
        {
            Name = "MixAndMatch Name "
        };

        await _repository.CreateAsync(mixAndMatch);

        var command = new MixAndMatchDeleteCommand(10);

        // Act
        var result = await _handler.Handle(command);

        // Assert
                
        Assert.False(result.Data);
    }

    [Fact]
    public async Task Delete_WithIdLessThanCiro_ResultBusinessLogicException()
    {
        // Arrange

        var mixAndMatch = new MixAndMatch()
        {
            Name = "MixAndMatch Name "
        };

        await _repository.CreateAsync(mixAndMatch);
        
        // Act

        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var command = new MixAndMatchDeleteCommand(-2);

            var result = await _handler.Handle(command);
        });        

        // Assert
                
        Assert.Equal("Id parameter out of bounds", exception.Message);
    }
}