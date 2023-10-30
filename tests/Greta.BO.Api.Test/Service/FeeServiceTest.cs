using Greta.BO.Api.Endpoints.FeeEndpoints;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.FeeSpecs;
using LanguageExt.ClassInstances;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Service;

public class FeeServiceTest
{
    readonly FeeRepository _repository;
    readonly FeeService _service;

    public FeeServiceTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(FeeServiceTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new FeeRepository(TestsSingleton.Auth, context);
        _service = new FeeService(_repository, Mock.Of<ILogger<FeeService>>());
    }

    [Fact]
    public async Task GetById_NormalCall_ResultSameAsInsert()
    {
        // Arrange
        var Fee = new Fee()
        {
            Name = "Fee Name "
        };
        var id = await _repository.CreateAsync(Fee);
        // Act
        var result = await _service.Get(id);

        // Assert
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetById_CallNotExist_ReturnNull()
    {
        // Arrange
        long id = 1000;
        // Act
        var result = await _service.Get(id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetId_CallNegative_ThrowBusinessLogicException()
    {
        // Arrange
        long id = -1000;
        // Act
        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var result = await _service.Get(id);
        });

        // Assert
        Assert.Equal("Id parameter out of bounds", exception.Message);
    }

    [Fact]
    public async Task ChangeState_NormalCall_ChangeSuccess()
    {
        // Arrange
       
        var Fee = new Fee()
        {
            Name = "Fee Name "
        };
        var id = await _repository.CreateAsync(Fee);       

        // Act
        var result = await _service.ChangeState(id, !Fee.State);

        // Assert
        Assert.True((bool)result);
    }

    [Fact]
    public async Task ChangeState_CallWithNotFound_ThrowBusinessLogicException()
    {
        // Arrange
        long id = -1000;
        // Act
        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var result = await _service.ChangeState(id, true);
        });

        // Assert
        Assert.Equal("Id parameter out of bounds.", exception.Message);
    }

    [Fact]
    public async Task Put_NormalCall_ChangeSuccess()
    {
        // Arrange
        var Fee = new Fee()
        {
            Name = "Fee Name",
            Products = new List<Product>(),
            Families = new List<Family>(),
            Categories = new List<Category>()
        };
        var id = await _repository.CreateAsync(Fee);

        var FeeUpdate = new Fee()
        {
            Id = id,
            Name = "Fee Name Update",
            Products = new List<Product>(),
            Families = new List<Family>(),
            Categories = new List<Category>()
        };

        // Act
        var result = await _service.Put(id, FeeUpdate);

        // Assert
        Assert.True((bool)result);
    }


    [Fact]
    public async Task Put_CallWithNotFound_ThrowBusinessLogicException()
    {
        // Arrange
        long id = -1000;
        var FeeUpdate = new Fee()
        {
            Name = "Fee Name Update",
            State = true
        };
        // Act
        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var result = await _service.Put(id, FeeUpdate);
        });

        // Assert
        Assert.Equal("Id parameter out of bounds", exception.Message);
    }
           

    [Fact]
    public async Task Post_NormalCall_ResultSameId()
    {
        // Arrange
        var product = new List<Product>() { new Product() { Name = "Product" } };
        var productId = _repository.CreateRangeAsync(product);

        var category = new List<Category>() { new Category() { Name = "Category" } };
        var categoryId = _repository.CreateRangeAsync(product);

        var fee = new Fee() { Name = "Fee", Products = product, Categories = category };
        var feeId = _repository.CreateAsync(fee);

        // Act
        var result = await _service.Post(fee);

        // Assert
        Assert.Equal(fee.Name, result.Name);
    }
}