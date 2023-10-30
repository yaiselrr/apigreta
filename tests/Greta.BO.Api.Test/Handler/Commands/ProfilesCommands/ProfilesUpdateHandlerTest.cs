using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Command.Profile;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Commands.ProfilesCommands;


public class ProfilesUpdateHandlerTest
{
    private readonly ProfilesRepository _repository;
    private readonly ProfilesUpdateHandler _handler;
    private readonly IMapper _mapper;

    public ProfilesUpdateHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(ProfilesUpdateHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new ProfilesRepository(TestsSingleton.Auth, context);
        var service = new ProfilesService(_repository, Mock.Of<ILogger<ProfilesService>>());

        _handler = new ProfilesUpdateHandler(
            Mock.Of<ILogger<ProfilesUpdateHandler>>(),
            service,
            TestsSingleton.Mapper);

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new TestMappingProfile());
        });
        _mapper = mockMapper.CreateMapper();
    }

    [Fact]
    public async Task Put_NormalCall_ResultTrue()
    {
        // Arrange

        var Profiles = new Profiles()
        {
            Name = "Profiles Put_NormalCall_ResultTrue "
        };             
       
        var id = await _repository.CreateAsync(Profiles);

        Profiles.Name = "Profiles Updated";

        var command = new ProfilesUpdateCommand(id ,_mapper.Map<ProfilesModel>(Profiles));

        // Act
        var result = await _handler.Handle(command);

        // Assert
       
        Assert.True(result.Data);
    }

    [Fact]
    public async Task Put_WithIdNotExist_ResultTrue()
    {
        // Arrange

        var Profiles = new Profiles()
        {
            Name = "Profiles Put_WithIdNotExist_ResultTrue"
        };

        await _repository.CreateAsync(Profiles);

        Profiles.Name = "Profiles Updated";

        // Act

        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var command = new ProfilesUpdateCommand(100, _mapper.Map<ProfilesModel>(Profiles));

            var result = await _handler.Handle(command);
        });

        // Assert

        Assert.Equal("Id parameter out of bounds", exception.Message);
    }

    // [Fact]
    // public async Task Put_WithIdLessThanCiro_ResultBusinessLogicException()
    // {
    //     // Arrange
    //
    //     var Profiles = new Profiles()
    //     {
    //         Name = "Profiles Put_WithIdLessThanCiro_ResultBusinessLogicException"
    //     };
    //
    //     await _repository.CreateAsync(Profiles);
    //
    //     Profiles.Name = "Profiles Updated";
    //
    //     // Act
    //
    //     var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
    //     {
    //         var command = new ProfilesUpdateCommand(-1, _mapper.Map<ProfilesModel>(Profiles));
    //
    //         var result = await _handler.Handle(command);
    //     });
    //
    //     // Assert
    //
    //     Assert.Equal("Id parameter out of bounds", exception.Message);
    // }
}