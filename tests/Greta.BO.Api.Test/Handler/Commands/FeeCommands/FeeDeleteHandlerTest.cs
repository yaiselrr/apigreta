using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Command.Fee;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Commands.FeeCommands;


public class FeeDeleteHandlerTest
{
    private readonly FeeRepository _repository;
    private readonly FeeDeleteHandler _handler;    

    public FeeDeleteHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(FeeDeleteHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new FeeRepository(TestsSingleton.Auth, context);
        var service = new FeeService(_repository, Mock.Of<ILogger<FeeService>>());

        _handler = new FeeDeleteHandler(
            Mock.Of<ILogger<FeeDeleteHandler>>(),
            service);       
    }

    [Fact]
    public async Task Delete_WithSameId_ReturnResultTrue()
    {
        // Arrange

        var fee = new Fee()
        {
            Name = "Fee Name "
        };             
       
        var id = await _repository.CreateAsync(fee);

        var command = new FeeDeleteCommand(id);

        // Act
        var result = await _handler.Handle(command);

        // Assert
               
        Assert.True(result.Data);
    }

    [Fact]
    public async Task Delete_WithNoExistId_ReturnResultFalse()
    {
        // Arrange

        var fee = new Fee()
        {
            Name = "Fee Name "
        };

        await _repository.CreateAsync(fee);

        var command = new FeeDeleteCommand(10);

        // Act
        var result = await _handler.Handle(command);

        // Assert
                
        Assert.False(result.Data);
    }

    [Fact]
    public async Task Delete_WithIdLessThanCiro_ResultBusinessLogicException()
    {
        // Arrange

        var fee = new Fee()
        {
            Name = "Fee Name "
        };

        await _repository.CreateAsync(fee);
        
        // Act

        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var command = new FeeDeleteCommand(-2);

            var result = await _handler.Handle(command);
        });        

        // Assert
                
        Assert.Equal("Id parameter out of bounds", exception.Message);
    }
}