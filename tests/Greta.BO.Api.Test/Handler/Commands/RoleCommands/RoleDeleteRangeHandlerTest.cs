using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Handlers.Command.Role;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Commands.RoleCommands;


public class RoleDeleteRangeHandlerTest
{
    private readonly RoleRepository _repository;
    private readonly RoleDeleteRangeHandler _handler;

    public RoleDeleteRangeHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(RoleDeleteRangeHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new RoleRepository(TestsSingleton.Auth, context);
        var service = new RoleService(_repository, Mock.Of<ILogger<RoleService>>());

        _handler = new RoleDeleteRangeHandler(
            Mock.Of<ILogger<RoleDeleteRangeHandler>>(),
            service);       
    }

    private void ResetDataBase()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(RoleDeleteRangeHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);
    }

    [Fact]
    public async Task DeleteRange_WithValidIdRange_ReturnResultTrue()
    {
        // Arrange

        ResetDataBase();

        List<Role> rols = new List<Role>();

        for (int i = 1; i <= 3; i++)
        {
            rols.Add(new Role() { Name = "Role" + i });
        }
        
        await _repository.CreateRangeAsync(rols);

        List<long> isd = new List<long>(rols.Select(x=>x.Id));

        // Act

        var command = new RoleDeleteRangeCommand(isd);

        var result = await _handler.Handle(command);       

        // Assert

        Assert.True(result.Data);
    }

    [Fact]
    public async Task DeleteRange_WithNotExistIdInRange_ReturnResultFalse()
    {
        // Arrange

        ResetDataBase();

        List<Role> rols = new List<Role>();

        for (int i = 1; i <= 3; i++)
        {
            rols.Add(new Role() { Name = "Role" + i });
        }

        await _repository.CreateRangeAsync(rols);

        List<long> ids = new List<long> { 100, 101, 102 };
        // Act

        var command = new RoleDeleteRangeCommand(ids);

        var result = await _handler.Handle(command);

        // Assert                

        Assert.False(result.Data);
    }

    [Fact]
    public async Task DeleteRange_WithIdLessThanCiro_ReturnResultFalse()
    {
        // Arrange

        ResetDataBase();

        List<Role> rols = new List<Role>();

        for (int i = 1; i <= 3; i++)
        {
            rols.Add(new Role() { Name = "Role" + i });
        }

        await _repository.CreateRangeAsync(rols);

        List<long> ids = new List<long> { -1, -2, -3 };

        // Act
        
        var command = new RoleDeleteRangeCommand(ids);

        var result = await _handler.Handle(command);               

        // Assert
                
        Assert.False(result.Data);
    }
}