using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Command.Profile;
using Greta.BO.BusinessLogic.Service;
using Greta.Identity.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Commands.ProfilesCommands;


public class ProfilesChangeStateHandlerTest
{
    private readonly ProfilesRepository _repository;
    private readonly ProfilesChangeStateHandler _handler;

    public ProfilesChangeStateHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(ProfilesChangeStateHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new ProfilesRepository(TestsSingleton.Auth, context);
                
        var service = new ProfilesService(_repository, Mock.Of<ILogger<ProfilesService>>());

        _handler = new ProfilesChangeStateHandler(
            Mock.Of<ILogger<ProfilesChangeStateHandler>>(),
            service);
    }

    [Fact]
    public async Task ChangeState_WithSameId_ResultTrue()
    {
        // Arrange

        var profile = new Profiles()
        {
            Name = "Profiles ChangeState_WithSameId_ResultTrue ",                       
        };

        var id = await _repository.CreateAsync(profile);
                
        var command = new ProfilesChangeStateCommand(id, !profile.State);

        // Act
        var result = await _handler.Handle(command);

        // Assert

        Assert.True(result.Data);
    }

    [Fact]
    public async Task ChangeState_WithIdNotExist_ResultFalse()
    {
        // Arrange

        var profile = new Profiles()
        {
            Name = "Profiles ChangeState_WithIdNotExist_ResultFalse",
        };

        await _repository.CreateAsync(profile);

        var command = new ProfilesChangeStateCommand(5, !profile.State);

        // Act
        var result = await _handler.Handle(command);

        // Assert

        Assert.False(result.Data);
    }

    [Fact]
    public async Task ChangeState_WithIdLessThanCiro_ResultBusinessLogicException()
    {
        // Arrange

        var profile = new Profiles()
        {
            Name = "Profiles ChangeState_WithIdNotExist_ResultFalse",
        };

        await _repository.CreateAsync(profile);       

        // Act

        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var command = new ProfilesChangeStateCommand(-1, !profile.State);

            var result = await _handler.Handle(command);
        });

        // Assert

        Assert.Equal("Id parameter out of bounds", exception.Message);
    }
}