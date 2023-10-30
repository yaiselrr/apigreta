
using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Handlers.Command.Profile;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Commands.ProfilesCommands;


public class ProfilesCreateHandlerTest
{
    private readonly ProfilesRepository _repository;
    private readonly ProfilesCreateHandler _handler;

    public ProfilesCreateHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(ProfilesCreateHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new ProfilesRepository(TestsSingleton.Auth, context);
        var service = new ProfilesService(_repository, Mock.Of<ILogger<ProfilesService>>());

        _handler = new ProfilesCreateHandler(
            Mock.Of<ILogger<ProfilesCreateHandler>>(),
            service,
            TestsSingleton.Mapper);

    }

    [Fact]
    public async Task Post_NormalCall_ResultSameId()
    {
        // Arrange

        var Profiles = new ProfilesModel()
        {
            Name = "Profiles Name "
        };             
       
        var command = new ProfilesCreateCommand(Profiles);

        // Act
        var result = await _handler.Handle(command);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ProfilesCreateResponse>(result);
        Assert.Equal(Profiles.Name, (result.Data as ProfilesListModel).Name);
    }
}