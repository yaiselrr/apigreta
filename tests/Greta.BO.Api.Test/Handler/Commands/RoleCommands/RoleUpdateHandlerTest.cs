using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Command.Role;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Commands.RoleCommands;


public class RoleUpdateHandlerTest
{
    private readonly RoleRepository _repository;
    private readonly RoleUpdateHandler _handler;
    private readonly IMapper _mapper;

    public RoleUpdateHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(RoleUpdateHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new RoleRepository(TestsSingleton.Auth, context);
        var service = new RoleService(_repository, Mock.Of<ILogger<RoleService>>());

        _handler = new RoleUpdateHandler(
            Mock.Of<ILogger<RoleUpdateHandler>>(),
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

        var role = new Role()
        {
            Name = "Role Put_NormalCall_ResultTrue "
        };             
       
        var id = await _repository.CreateAsync(role);

        role.Name = "Role Updated";

        var command = new RoleUpdateCommand(id ,_mapper.Map<RoleModel>(role));

        // Act
        var result = await _handler.Handle(command);

        // Assert
       
        Assert.True(result.Data);
    }

    [Fact]
    public async Task Put_WithIdNotExist_ResultTrue()
    {
        // Arrange

        var role = new Role()
        {
            Name = "Role Put_WithIdNotExist_ResultTrue"
        };

        await _repository.CreateAsync(role);

        role.Name = "Role Updated";

        var command = new RoleUpdateCommand(5, _mapper.Map<RoleModel>(role));

        // Act
        var result = await _handler.Handle(command);

        // Assert

        Assert.False(result.Data);
    }

    [Fact]
    public async Task Put_WithIdLessThanCiro_ResultBusinessLogicException()
    {
        // Arrange

        var role = new Role()
        {
            Name = "Role Put_WithIdLessThanCiro_ResultBusinessLogicException"
        };

        await _repository.CreateAsync(role);

        role.Name = "Role Updated";

        // Act

        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var command = new RoleUpdateCommand(-1, _mapper.Map<RoleModel>(role));

            var result = await _handler.Handle(command);
        });

        // Assert

        Assert.Equal("Id parameter out of bounds", exception.Message);
    }
}