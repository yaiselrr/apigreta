
using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Handlers.Command.Role;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Commands.RoleCommands;


public class RoleCreateHandlerTest
{
    private readonly RoleRepository _repository;
    private readonly RoleCreateHandler _handler;
    private readonly IMapper _mapper;

    public RoleCreateHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(RoleCreateHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new RoleRepository(TestsSingleton.Auth, context);
        var service = new RoleService(_repository, Mock.Of<ILogger<RoleService>>());

        _handler = new RoleCreateHandler(
            Mock.Of<ILogger<RoleCreateHandler>>(),
            service,
            TestsSingleton.Mapper);

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new TestMappingProfile());
        });
        _mapper = mockMapper.CreateMapper();
    }

    [Fact]
    public async Task Post_NormalCall_ResultSameId()
    {
        // Arrange

        var role = new Role()
        {
            Name = "Role Name "
        };             
       
        var command = new RoleCreateCommand(_mapper.Map<RoleModel>(role));

        // Act
        var result = await _handler.Handle(command);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<RoleCreateResponse>(result);
        Assert.Equal(role.Name, (result.Data as RoleModel).Name);
    }
}