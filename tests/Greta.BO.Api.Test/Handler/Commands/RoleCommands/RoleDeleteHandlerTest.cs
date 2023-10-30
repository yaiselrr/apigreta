using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Command.Role;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Commands.RoleCommands;


public class RoleDeleteHandlerTest
{
    private readonly RoleRepository _repository;
    private readonly RoleDeleteHandler _handler;    

    public RoleDeleteHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(RoleDeleteHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new RoleRepository(TestsSingleton.Auth, context);
        var service = new RoleService(_repository, Mock.Of<ILogger<RoleService>>());

        _handler = new RoleDeleteHandler(
            Mock.Of<ILogger<RoleDeleteHandler>>(),
            service);       
    }

    [Fact]
    public async Task Delete_WithSameId_ReturnResultTrue()
    {
        // Arrange

        var role = new Role()
        {
            Name = "Role Name "
        };             
       
        var id = await _repository.CreateAsync(role);

        var command = new RoleDeleteCommand(id);

        // Act
        var result = await _handler.Handle(command);

        // Assert
               
        Assert.True(result.Data);
    }

    [Fact]
    public async Task Delete_WithNoExistId_ReturnResultFalse()
    {
        // Arrange

        var role = new Role()
        {
            Name = "Role Name "
        };

        var id = await _repository.CreateAsync(role);

        var command = new RoleDeleteCommand(10);

        // Act
        var result = await _handler.Handle(command);

        // Assert
                
        Assert.False(result.Data);
    }

    [Fact]
    public async Task Delete_WithIdLessThanCiro_ResultBusinessLogicException()
    {
        // Arrange

        var role = new Role()
        {
            Name = "Role Name "
        };

        await _repository.CreateAsync(role);
        
        // Act

        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var command = new RoleDeleteCommand(-2);

            var result = await _handler.Handle(command);
        });        

        // Assert
                
        Assert.Equal("Id parameter out of bounds", exception.Message);
    }
}