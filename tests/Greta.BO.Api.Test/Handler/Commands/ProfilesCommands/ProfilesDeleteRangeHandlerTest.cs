using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Handlers.Command.Profile;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Commands.ProfilesCommands;


public class ProfilesDeleteRangeHandlerTest
{
    private readonly ProfilesRepository _repository;
    private readonly ProfilesDeleteRangeHandler _handler;

    public ProfilesDeleteRangeHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(ProfilesDeleteRangeHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new ProfilesRepository(TestsSingleton.Auth, context);
        var service = new ProfilesService(_repository, Mock.Of<ILogger<ProfilesService>>());

        _handler = new ProfilesDeleteRangeHandler(
            Mock.Of<ILogger<ProfilesDeleteRangeHandler>>(),
            service);       
    }

    private void ResetDataBase()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(ProfilesDeleteRangeHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);
    }

    [Fact]
    public async Task DeleteRange_WithValidIdRange_ReturnResultTrue()
    {
        // Arrange

        ResetDataBase();

        List<Profiles> rols = new List<Profiles>();

        for (int i = 1; i <= 3; i++)
        {
            rols.Add(new Profiles() { Name = "Profiles" + i });
        }
        
        await _repository.CreateRangeAsync(rols);

        List<long> isd = new List<long>(rols.Select(x=>x.Id));

        // Act

        var command = new ProfilesDeleteRangeCommand(isd);

        var result = await _handler.Handle(command);       

        // Assert

        Assert.True(result.Data);
    }

    [Fact]
    public async Task DeleteRange_WithNotExistIdInRange_ReturnResultFalse()
    {
        // Arrange

        ResetDataBase();

        List<Profiles> rols = new List<Profiles>();

        for (int i = 1; i <= 3; i++)
        {
            rols.Add(new Profiles() { Name = "Profiles" + i });
        }

        await _repository.CreateRangeAsync(rols);

        List<long> ids = new List<long> { 100, 101, 102 };
        // Act

        var command = new ProfilesDeleteRangeCommand(ids);

        var result = await _handler.Handle(command);

        // Assert                

        Assert.False(result.Data);
    }

    [Fact]
    public async Task DeleteRange_WithIdLessThanCiro_ReturnResultFalse()
    {
        // Arrange

        ResetDataBase();

        List<Profiles> rols = new List<Profiles>();

        for (int i = 1; i <= 3; i++)
        {
            rols.Add(new Profiles() { Name = "Profiles" + i });
        }

        await _repository.CreateRangeAsync(rols);

        List<long> ids = new List<long> { -1, -2, -3 };

        // Act
        
        var command = new ProfilesDeleteRangeCommand(ids);

        var result = await _handler.Handle(command);               

        // Assert
                
        Assert.False(result.Data);
    }
}