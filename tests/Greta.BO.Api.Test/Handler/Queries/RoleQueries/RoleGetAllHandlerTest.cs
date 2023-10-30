using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Handlers.Queries.Role;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Queries.RoleQueries;

public class RoleGetAllHandlerTest
{
    private readonly RoleRepository _repository;
    private readonly RoleGetAllHandler _handler;
    
    public RoleGetAllHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(RoleGetAllHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new RoleRepository(TestsSingleton.Auth, context);
        var service = new RoleService(_repository, Mock.Of<ILogger<RoleService>>());

        _handler = new RoleGetAllHandler(            
            service,
            TestsSingleton.Mapper);        
    }
    
    [Fact]
    public async Task RoleGetAll_ReturnsRoleGetAllResponseWithData()
    {
        // Arrange
        
        var name = "RoleGetAll_ReturnsRoleGetAllResponseWithData";
        var role = new Role()
        {
            Name = name
        };
        await _repository.CreateAsync(role);
        // Act
       
        var query = new RoleGetAllQuery();

        var result = await _handler.Handle(query);
       
        // Assert
        Assert.NotNull(result);
        Assert.IsType<RoleGetAllResponse>(result);
        Assert.NotNull(result.Data);
    }
    
    [Fact]
    public async Task RoleGetAll_ReturnsRoleGetAllResponseWithOutData()
    {
        // Arrange
                
        // Act
        var query = new RoleGetAllQuery();
        var result = await _handler.Handle(query);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<RoleGetAllResponse>(result);
        Assert.Empty(result.Data);
    }
   
}