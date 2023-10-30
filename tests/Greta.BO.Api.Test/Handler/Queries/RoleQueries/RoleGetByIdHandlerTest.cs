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

public class RoleGetByIdHandlerTest
{
    private readonly RoleRepository _repository;
    private readonly RoleGetByIdHandler _handler;
    
    public RoleGetByIdHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(RoleGetByIdHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new RoleRepository(TestsSingleton.Auth, context);
        var service = new RoleService(_repository, Mock.Of<ILogger<RoleService>>());

        _handler = new RoleGetByIdHandler(            
            service,
            TestsSingleton.Mapper);        
    }
   

    [Fact]
    public async Task RoleGetById_CallWithInvalidNumber_ReturnNULL()
    {
        // Arrange
       
        var name = "RoleGetById_CallWithInvalidNumber_ReturnNULL";
        var role = new Role()
        {
            Name = name
        };

        await _repository.CreateAsync(role);
        // Act
       
        var query = new RoleGetByIdQuery(-13);

        var result = await _handler.Handle(query);
       
        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task RoleGetById_CallWithValidNumberButNotPresentId_ReturnNULL()
    {
        // Arrange
        var name = "RoleGetById_CallWithValidNumberButNotPresentId_ReturnNULL";
        var role = new Role()
        {
            Name = name
        };
        await _repository.CreateAsync(role);
        // Act
        var query = new RoleGetByIdQuery(1000);
        var result = await _handler.Handle(query);

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task RoleGetById_CallWithValidNumber_ReturnObjectWithSameID()
    {
        // Arrange
        var name = "RoleGetById_CallWithValidNumber_ReturnObjectWithSameID";
        var role = new Role()
        {
            Name = name
        };
        var id = await _repository.CreateAsync(role);
        // Act
        var query = new RoleGetByIdQuery(id);
        var result = await _handler.Handle(query);

        // Assert
        Assert.Equal(id, result.Data.Id);
    }
   
}