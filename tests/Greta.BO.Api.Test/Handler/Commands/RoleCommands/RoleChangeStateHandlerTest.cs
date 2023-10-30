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


public class RoleChangeStateHandlerTest
{
    private readonly RoleRepository _repository;
    private readonly RoleChangeStateHandler _handler;    

    public RoleChangeStateHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(RoleChangeStateHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new RoleRepository(TestsSingleton.Auth, context);
        var service = new RoleService(_repository, Mock.Of<ILogger<RoleService>>());

        _handler = new RoleChangeStateHandler(
            Mock.Of<ILogger<RoleChangeStateHandler>>(),
            service);        
    }

    [Fact]
    public async Task Put_NormalCall_ResultTrue()
    {
        // Arrange

        var role = new Role()
        {
            Name = "Role Put_NormalCall_ResultTrue ",
            State = false
        };             
       
        var id = await _repository.CreateAsync(role);

        role.Name = "Role ChangeStated";

        var command = new RoleChangeStateCommand(id , true);

        // Act
        var result = await _handler.Handle(command);

        // Assert
       
        Assert.True(result.Data);
    }

    [Fact]
    public async Task Put_WithIdNotExist_ResultTrue()
    {
        // Arrange

        var role = new Role()
        {
            Name = "Role Put_WithIdNotExist_ResultTrue",
            State = false
        };

        await _repository.CreateAsync(role);

        role.Name = "Role ChangeStated";

        var command = new RoleChangeStateCommand(5, true);

        // Act
        var result = await _handler.Handle(command);

        // Assert

        Assert.False(result.Data);
    }

    [Fact]
    public async Task Put_WithIdLessThanCiro_ResultBusinessLogicException()
    {
        // Arrange

        var role = new Role()
        {
            Name = "Role Put_WithIdLessThanCiro_ResultBusinessLogicException",
            State = false
        };

        await _repository.CreateAsync(role);

        role.Name = "Role ChangeStated";

        // Act

        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var command = new RoleChangeStateCommand(-1, true);

            var result = await _handler.Handle(command);
        });

        // Assert

        Assert.Equal("Id parameter out of bounds", exception.Message);
    }
}