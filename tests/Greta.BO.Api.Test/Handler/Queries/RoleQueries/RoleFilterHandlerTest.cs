using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Queries.Role;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Queries.RoleQueries;


public class RoleFilterHandlerTest
{
    private readonly RoleRepository _repository;
    private readonly RoleFilterHandler _handler;

    public RoleFilterHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(RoleFilterHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new RoleRepository(TestsSingleton.Auth, context);
        var service = new RoleService(_repository, Mock.Of<ILogger<RoleService>>());

        _handler = new RoleFilterHandler(
            Mock.Of<ILogger<RoleFilterHandler>>(),
            service,
            TestsSingleton.Mapper);
    }

    [Fact]
    public async Task FilterSpec_NormalCall_ResultOk()
    {
        // Arrange
        var fams = new List<Role>();
        for (var i = 0; i < 10; i++)
        {
            fams.Add(new Role()
            {
                Name = "Role Name " + i
            });
        }

        var id = await _repository.CreateRangeAsync<Role>(fams);
        var filter = new RoleSearchModel();
        var query = new RoleFilterQuery(1,10, filter);
        // Act
        var result = await _handler.Handle(query);

        // Assert
        Assert.Equal(10, result.Data.Data.Count);
    }

    
    [Fact]
    public async Task FilterSpec_NormalCall_WithCurrentPageOrPageSize_LessThanCiro_ResultBusinessException()
    {
        // Arrange
        var fams = new List<Role>();
        for (var i = 0; i < 10; i++)
        {
            fams.Add(new Role()
            {
                Name = "Role Name " + i
            });
        }

        var id = await _repository.CreateRangeAsync<Role>(fams);

        // Act

        var filter = new RoleSearchModel();

        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var query = new RoleFilterQuery(-2, -1, filter);
            
            var result = await _handler.Handle(query);
        });

        // Assert
        Assert.Equal("Page parameter out of bounds", exception.Message);
    }
}