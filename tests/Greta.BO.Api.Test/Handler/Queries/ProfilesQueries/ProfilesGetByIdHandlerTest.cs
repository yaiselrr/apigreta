using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Queries.Family;
using Greta.BO.BusinessLogic.Handlers.Queries.Profiles;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Queries.ProfilesQueries;

public class ProfilesGetByIdHandlerTest
{
    private readonly ProfilesRepository _repository;
    private readonly ProfilesGetByIdHandler _handler;
    
    public ProfilesGetByIdHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(ProfilesGetByIdHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new ProfilesRepository(TestsSingleton.Auth, context);
        var service = new ProfilesService(_repository, Mock.Of<ILogger<ProfilesService>>());

        _handler = new ProfilesGetByIdHandler(
            Mock.Of<ILogger<ProfilesGetByIdHandler>>(),
            service);        
    }   

    [Fact]
    public async Task ProfilesGetById_CallWithInvalidNumber_ReturnBusinessLogicException()
    {
        // Arrange

        var application = new ClientApplication() { Name = "Application" };
        var app = await _repository.CreateAsync(application);

        var permisions = new List<Permission>();
        for (int i = 0; i < 3; i++)
        {
            permisions.Add(new Permission() { Name = "Permission " + i, Code = "Code" });
        }

        var name = "ProfilesGetById_CallWithInvalidNumber_ReturnBusinessLogicException";
        var Profiles = new Profiles()
        {
            Name = name,
            ApplicationId = app.Id,
            Permissions = permisions
        };

        await _repository.CreateAsync(Profiles);
        // Act

        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var query = new ProfilesGetByIdQuery(-13);

            var result = await _handler.Handle(query);
        });       
       
        // Assert
        Assert.Equal("Id parameter out of bounds", exception.Message);
    }
    
    [Fact]
    public async Task ProfilesGetById_CallWithValidNumberButNotPresentId_ReturnNULL()
    {
        // Arrange
        var name = "ProfilesGetById_CallWithValidNumberButNotPresentId_ReturnNULL";

        var application = new ClientApplication() { Name = "Application" };
        var app = await _repository.CreateAsync(application);

        var permisions = new List<Permission>();
        for (int i = 0; i < 3; i++)
        {
            permisions.Add(new Permission() { Name = "Permission " + i, Code = "Code" });
        }

        var Profiles = new Profiles()
        {
            Name = name,
            ApplicationId = app.Id,
            Permissions = permisions
        };
        await _repository.CreateAsync(Profiles);
        // Act
        var query = new ProfilesGetByIdQuery(1000);
        var result = await _handler.Handle(query);

        // Assert
        Assert.Null(result.Data);
    }
    
    [Fact]
    public async Task ProfilesGetById_CallWithValidNumber_ReturnObjectWithSameID()
    {
        // Arrange
        var name = "ProfilesGetById_CallWithValidNumber_ReturnObjectWithSameID";

        var application = new ClientApplication() { Name = "Application" };
        var app = await _repository.CreateAsync(application);

        var permisions = new List<Permission>();
        for (int i = 0; i < 3; i++)
        {
            permisions.Add(new Permission() { Name = "Permission " + i, Code = "Code" });
        }

        var Profiles = new Profiles()
        {
            Name = name,
            ApplicationId = app.Id,
            Permissions = permisions
        };
        var id = await _repository.CreateAsync(Profiles);
        // Act
        var query = new ProfilesGetByIdQuery(id);
        var result = await _handler.Handle(query);

        // Assert
        Assert.Equal(id, result.Data.Id);
    }
   
}