using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Queries.Category;
using Greta.BO.BusinessLogic.Service;
using MassTransit.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Queries.CategoryQueries;

public class CategoryGetByDepartmentHandlerTest
{
    private readonly CategoryRepository _repository;
    private readonly CategoryGetByDepartmentHandler _handler;

    public CategoryGetByDepartmentHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(CategoryGetByDepartmentHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new CategoryRepository(TestsSingleton.Auth, context);
        var service = new CategoryService(_repository, Mock.Of<ISynchroService>(),Mock.Of<ILogger<CategoryService>>());

        _handler = new CategoryGetByDepartmentHandler(
            service,
            TestsSingleton.Mapper);
    }
    
    [Fact]
    public async Task CategoryGetByDepartment_CallWithValidNumberButNotPresentId_ReturnNULL()
    {
        // Arrange
        // Act
        var query = new CategoryGetByDepartmentQuery(10000);
        var result = await _handler.Handle(query);

        // Assert
        Assert.Empty(result.Data);
    }
    
    [Fact]
    public async Task CategoryGetByDepartment_CallWithValidNumber_ReturnOk()
    {
        // Arrange
        await _repository.CreateAsync(new Category { Name = "Category 1", DepartmentId = 1 });
        // Act
        var query = new CategoryGetByDepartmentQuery(1);
        var result = await _handler.Handle(query);

        // Assert
        Assert.NotEmpty(result.Data);
    }
}