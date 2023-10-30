using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Service;

public class ScaleLabelTypeServiceTest
{
    readonly ScaleLabelTypeRepository _repository;
    readonly ScaleLabelTypeService _service;

    public ScaleLabelTypeServiceTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(ScaleLabelTypeServiceTest)).ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new ScaleLabelTypeRepository(TestsSingleton.Auth, context);
        _service = new ScaleLabelTypeService(_repository, Mock.Of<ILogger<ScaleLabelTypeService>>(), Mock.Of<ISynchroService>());
    }

    [Fact]
    public void Get_NormalCall_ResultOk()
    {
        // Arrange

        // Act
        var result = _service.Get().Result;

        // Assert
        Assert.True(result.Count >= 0);
    }

    [Fact]
    public async Task GetId_NormalCall_ResultSameAsInsert()
    {
        // Arrange
        var scaleLabelType = new ScaleLabelType()
        {
            Name = "Category Name ",
            LabelId = 1,
            ScaleType = 0,
            Design = ""
        };

        var id = await _repository.CreateAsync(scaleLabelType);
        // Act
        var result = await _service.Get(id);

        // Assert
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetId_CallNotExist_ReturnNull()
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
    public async Task Delete_NormalCall_ResultSucess()
    {
        // Arrange
        var scaleLabelType = new ScaleLabelType()
        {
            Name = "Category Name ",
            LabelId = 1,
            ScaleType = 0,
            Design = ""
        };

        var id = await _repository.CreateAsync(scaleLabelType);

        // Act
        var result = await _service.Delete(id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Delete_CallWithNotFound_ThrowBusinessLogicException()
    {
        // Arrange
        long id = -1000;
        // Act
        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var result = await _service.Delete(id);
        });

        // Assert
        Assert.Equal("Id parameter out of bounds", exception.Message);
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
        Assert.Equal("Id parameter out of bounds", exception.Message);
    }

    [Fact]
    public async Task ChangeState_NormalCall_ResultTrue()
    {
        // Arrange
        var scaleLabelType = new ScaleLabelType()
        {
            Name = "Category Name ",
            LabelId = 1,
            ScaleType = 0,
            Design = ""
        };

        var id = await _repository.CreateAsync(scaleLabelType);

        // Act
        var result = await _service.ChangeState(id, false);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Post_NormalCall_ResultSameId()
    {
        // Arrange
        var scaleLabelType = new ScaleLabelType()
        {
            Name = "Category Name ",
            LabelId = 1,
            ScaleType = 0,
            Design = ""
        };

        // Act
        var result = await _service.Post(scaleLabelType);

        // Assert
        Assert.Equal(scaleLabelType.Name, (result as ScaleLabelType).Name);
    }

    [Fact]
    public async Task Put_NormalCall_ChangeSuccess()
    {
        // Arrange
        var scaleLabelType = new ScaleLabelType()
        {
            Name = "Category Name ",
            LabelId = 1,
            ScaleType = 0,
            Design = ""
        };
        var id = await _repository.CreateAsync(scaleLabelType);

        var scaleLabelTypeUpdate = new ScaleLabelType()
        {
            Name = "Category Name Update",
            LabelId = 1,
            ScaleType = 0,
            Design = "",
            State = true
        };

        // Act
        var result = await _service.Put(id, scaleLabelTypeUpdate);

        // Assert
        Assert.True((bool)result);
    }

    [Fact]
    public async Task Put_CallWithNotFound_ThrowBusinessLogicException()
    {
        // Arrange
        long id = -1000;
        var scaleLabelTypeUpdate = new ScaleLabelType()
        {
            Name = "Category Name Update",
            LabelId = 1,
            ScaleType = 0,
            Design = "",
            State = true
        };
        // Act
        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var result = await _service.Put(id, scaleLabelTypeUpdate);
        });

        // Assert
        Assert.Equal("Id parameter out of bounds", exception.Message);
    }


    [Fact]
    public async Task FilterScaleLabelType_OutRange_ThrowBusinessLogicException()
    {
        // Arrange
        var scaleLabelType = new ScaleLabelType()
        {
            Name = "Category Name ",
            LabelId = 1,
            ScaleType = 0,
            Design = ""
        };

        var id = await _repository.CreateAsync(scaleLabelType);
        // Act
        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var result = await _service.FilterTag(
                0, 0, null, "", "");
        });

        // Assert
        Assert.Equal("Page parameter out of bounds.", exception.Message);
    }

    [Fact]
    public async Task FilterCategory_NormalCall_ResultOk()
    {
        // Arrange
        // Arrange
        var scaleLabelTypeTest = new ScaleLabelType()
        {
            Name = "Category Name ",
            LabelId = 1,
            ScaleType = 0,
            Design = ""
        };
        var test = await _repository.CreateAsync(scaleLabelTypeTest);

        var scales = new List<ScaleLabelType>();
        for (var i = 0; i < 10; i++)
        {
            scales.Add(new ScaleLabelType()
            {
                Name = "Category Name " + i,
                LabelId = 1,
                ScaleType = 0,
                Design = ""
            });
        }

        var id = await _repository.CreateRangeAsync<ScaleLabelType>(scales);

        var filter = new ScaleLabelTypeSearchModel();

        var result = await _service.FilterTag(
                1, 10, scaleLabelTypeTest, "", "");

        // Assert
        Assert.Equal(10, result.Data.Count);
    }

    [Fact]
    public async Task GetScaleLabelTypeByType_NormalCall_ResultOk()
    {
        // Arrange
        var scaleLabelType = new ScaleLabelType()
        {
            Name = "Category Name ",
            LabelId = 1,
            ScaleType = 0,
            Design = ""
        };
        var id = await _repository.CreateAsync(scaleLabelType);
        // Act
        var result = await _service.GetByType(scaleLabelType.ScaleType);

        // Assert
        Assert.True(result.Count >= 0);
    }

}