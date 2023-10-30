using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Queries.Profiles;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Queries.ProfilesQueries;


public class ProfilesFilterHandlerTest
{
    private readonly ProfilesRepository _repository;
    private readonly ProfilesFilterHandler _handler;

    public ProfilesFilterHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(ProfilesFilterHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new ProfilesRepository(TestsSingleton.Auth, context);
        var service = new ProfilesService(_repository, Mock.Of<ILogger<ProfilesService>>());

        _handler = new ProfilesFilterHandler(
            Mock.Of<ILogger<ProfilesFilterHandler>>(),
            service,
            TestsSingleton.Mapper);
    }

    [Fact]
    public async Task FilterSpec_NormalCall_ResultOk()
    {
        // Arrange

        var clientApplication = new ClientApplication() { Name = "App" };
        var appId = await _repository.CreateAsync(clientApplication);              
        
        var profiles = new List<Profiles>();
        for (var i = 0; i < 10; i++)
        {
            profiles.Add(new Profiles()
            {
                Name = "Profiles Name " + i,
                ApplicationId = appId.Id                
            });
        }

        var id = await _repository.CreateRangeAsync<Profiles>(profiles);
        var filter = new ProfilesSearchModel() {  };
        var query = new ProfilesFilterQuery(1,10, filter);
        // Act
        var result = await _handler.Handle(query);

        // Assert
        Assert.Equal(10, result.Data.Data.Count);
    }

    
    [Fact]
    public async Task FilterSpec_NormalCall_WithCurrentPageOrPageSize_LessThanCiro_ResultBusinessException()
    {
        // Arrange

        var clientApplication = new ClientApplication() { Name = "App" };
        var appId = await _repository.CreateAsync(clientApplication);

        var profiles = new List<Profiles>();
        for (var i = 0; i < 10; i++)
        {
            profiles.Add(new Profiles()
            {
                Name = "Profiles Name " + i,
                ApplicationId = appId.Id
            });
        }

        var id = await _repository.CreateRangeAsync<Profiles>(profiles);

        // Act

        var filter = new ProfilesSearchModel();

        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var query = new ProfilesFilterQuery(-2, -1, filter);
            
            var result = await _handler.Handle(query);
        });

        // Assert
        Assert.Equal("Page parameter out of bounds", exception.Message);
    }
}