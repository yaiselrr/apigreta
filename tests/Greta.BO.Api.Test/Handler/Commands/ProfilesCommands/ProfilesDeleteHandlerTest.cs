using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Command.Profile;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Commands.ProfilesCommands;


public class ProfilesDeleteHandlerTest
{
    private readonly ProfilesRepository _repository;
    private readonly ProfilesDeleteHandler _handler;    

    public ProfilesDeleteHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(ProfilesDeleteHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new ProfilesRepository(TestsSingleton.Auth, context);
        var service = new ProfilesService(_repository, Mock.Of<ILogger<ProfilesService>>());

        _handler = new ProfilesDeleteHandler(
            Mock.Of<ILogger<ProfilesDeleteHandler>>(),
            service);       
    }

    [Fact]
    public async Task Delete_WithSameId_ReturnResultTrue()
    {
        // Arrange

        var Profiles = new Profiles()
        {
            Name = "Profiles Name "
        };             
       
        var id = await _repository.CreateAsync(Profiles);

        var command = new ProfilesDeleteCommand(id);

        // Act
        var result = await _handler.Handle(command);

        // Assert
               
        Assert.True(result.Data);
    }

    [Fact]
    public async Task Delete_WithNoExistId_ReturnResultFalse()
    {
        // Arrange

        var Profiles = new Profiles()
        {
            Name = "Profiles Name "
        };

        var id = await _repository.CreateAsync(Profiles);

        var command = new ProfilesDeleteCommand(10);

        // Act
        var result = await _handler.Handle(command);

        // Assert
                
        Assert.False(result.Data);
    }

    [Fact]
    public async Task Delete_WithIdLessThanCiro_ResultBusinessLogicException()
    {
        // Arrange

        var Profiles = new Profiles()
        {
            Name = "Profiles Name "
        };

        await _repository.CreateAsync(Profiles);
        
        // Act

        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var command = new ProfilesDeleteCommand(-2);

            var result = await _handler.Handle(command);
        });        

        // Assert
                
        Assert.Equal("Id parameter out of bounds", exception.Message);
    }
}