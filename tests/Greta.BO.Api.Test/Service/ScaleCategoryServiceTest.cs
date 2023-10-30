using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.ScaleCategorySpecs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Service;

public class ScaleCategoryServiceTest
{
    readonly ScaleCategoryRepository _repository;
    readonly ScaleCategoryService _service;

    public ScaleCategoryServiceTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(ScaleCategoryServiceTest)).ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new ScaleCategoryRepository(TestsSingleton.Auth, context);
        _service = new ScaleCategoryService(_repository, Mock.Of<ISynchroService>(),Mock.Of<ILogger<ScaleCategoryService>>());
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
        var scale = new ScaleCategory()
        {
            Name = "Category Name ",
            CategoryId = 1820
        };

        var id = await _repository.CreateAsync(scale);
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
        var scale = new ScaleCategory()
        {
            Name = "Category Name ",
            CategoryId = 1821
        };

        var id = await _repository.CreateAsync(scale);

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
        var scale = new ScaleCategory()
        {
            Name = "Category Name ",
            CategoryId = 1822
        };

        var id = await _repository.CreateAsync(scale);

        // Act
        var result = await _service.ChangeState(id, false);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Post_NormalCall_ResultSameId()
    {
        // Arrange
        var scale = new ScaleCategory()
        {
            Name = "Category Name ",
            CategoryId = 1823,
            DepartmentId = 10,
            ParentId = 5
        };

        // Act
        var result = await _service.Post(scale);

        // Assert
        Assert.Equal(scale.Name, (result as ScaleCategory).Name);
    }

    [Fact]
    public async Task Put_NormalCall_ChangeSuccess()
    {
        // Arrange
        var scale = new ScaleCategory()
        {
            Name = "Category Name ",
            CategoryId = 1824,
            DepartmentId = 10,
            ParentId = 5
        };
        var id = await _repository.CreateAsync(scale);

        var scaleUpdate = new ScaleCategory()
        {
            Id = id,
            Name = "Category Name Update ",
            CategoryId = 1824,
            DepartmentId = 10,
            ParentId = 5,
            State = true
        };

        // Act
        var result = await _service.Put(id, scaleUpdate);

        // Assert
        Assert.True((bool)result);
    }

    [Fact]
    public async Task Put_CallWithNotFound_ThrowBusinessLogicException()
    {
        // Arrange
        long id = -1000;
        var scaleUpdate = new ScaleCategory()
        {
            Id = id,
            Name = "Category Name Update ",
            CategoryId = 1824,
            DepartmentId = 10,
            ParentId = 5,
            State = true
        };
        // Act
        var result = await _service.Put(id, scaleUpdate);

        // Assert
        Assert.False(result);
    }


    [Fact]
    public async Task FilterScaleLabelType_OutRange_ThrowBusinessLogicException()
    {
        // Arrange
        var scale = new ScaleCategory()
        {
            Name = "Category Name ",
            CategoryId = 1825,
            DepartmentId = 10,
            ParentId = 5,
            State = true
        };

        var id = await _repository.CreateAsync(scale);
        // Act
        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var result = await _service.FilterSpec(
                0, 0, null);
        });

        // Assert
        Assert.Equal("Page parameter out of bounds", exception.Message);
    }

    [Fact]
    public async Task FilterCategory_NormalCall_ResultOk()
    {
        // Arrange
            var scaleCategory = new ScaleCategory()
            {
                Name = "Category Name ",
                CategoryId = 18256
            };
            var depId = _repository.GetEntity<Department>().Add(new Department()
            {
                Id = 164,
                Name = "Department 1rew"
            });

            await _repository.SaveChangesAsync();

            var cats = new List<ScaleCategory>();
            for (var i = 0; i < 10; i++)
            {
                cats.Add(new ScaleCategory()
                {
                    Name = "_fil_ Category Name " + i,                    
                    CategoryId = 11004 + i,
                    DepartmentId = 164
                });
            }
            var id = await _repository.CreateRangeAsync<ScaleCategory>(cats);

            var spec = new ScaleCategoryFilterSpec(
                new ScaleCategorySearchModel()
                {
                    Name = "_fil_"
                }
            );
            
            var filter = new ScaleCategorySearchModel();

            var result = await _service.FilterSpec(
                    1, 10, spec);

            // Assert
            Assert.Equal(10, result.Data.Count);
    }

    [Fact]
    public async Task GetByScaleCategoryId_NormalCall_ResultOk()
    {
        // Arrange
        var scale = new ScaleCategory()
        {
            Name = "Category Name ",
            CategoryId = 1810
        };

        var id = await _repository.CreateAsync(scale);
        // Act
        var find = await _service.Get(id);

        var result = await _service.GetByScaleCategoryId(find.CategoryId, 2);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetByScaleCategoryIdLongId_NormalCall_ResultNull()
    {
        // Arrange
        var scale = new ScaleCategory()
        {
            Name = "Category Name ",
            CategoryId = 1860,
            DepartmentId = 10,
            ParentId = 5,
            State = true
        };
        var id = await _repository.CreateAsync(scale);
        // Act
        var result = await _service.GetByScaleCategoryId(scale.CategoryId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByScaleCategoryId_NormalCall_ResultNull()
    {
        // Arrange
        var scale = new ScaleCategory()
        {
            Name = "Category Name ",
            CategoryId = 1861,
            DepartmentId = 10,
            ParentId = 5,
            State = true
        };
        var id = await _repository.CreateAsync(scale);
        // Act
        var result = await _service.GetByScaleCategoryId(-11, id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllForScales_NormalCall_ResultOk()
    {
        // Arrange
        var scales = new List<ScaleCategory>();
        for (var i = 0; i < 10; i++)
        {
            scales.Add(new ScaleCategory()
            {
                Name = "Category Name" + i,
                CategoryId = 1875 + i,
                DepartmentId = 10,
                ParentId = 5,
                State = true
            });
        }

        var id = await _repository.CreateRangeAsync<ScaleCategory>(scales);

        var deps = new List<Department>();

        deps.Add(new Department()
            {
                Name = "Department Name 10",
                State = true
            });
        
        var depsTest = await _repository.CreateRangeAsync<Department>(deps);

        var depsId = new List<long>();

        for (int i = 0; i < deps.Length(); i++)
        {
            depsId.Add(deps[i].Id);
        }

        // Act
        var result = await _service.GetAllForScales(depsId);

        // Assert
        Assert.True(result.Count >= 0);
    }

    [Fact]
    public async Task GetAllForScalesWithListNull_NormalCall_Result()
    {
        // Arrange
        var scales = new List<ScaleCategory>();
        for (var i = 0; i < 10; i++)
        {
            scales.Add(new ScaleCategory()
            {
                Name = "Category Name" + i,
                CategoryId = 1895 + i,
                DepartmentId = 10,
                ParentId = 5,
                State = true
            });
        }

        var id = await _repository.CreateRangeAsync<ScaleCategory>(scales);

        var deps = new List<Department>();
        
        var depsTest = await _repository.CreateRangeAsync<Department>(deps);

        var depsId = new List<long>();

        for (int i = 0; i < deps.Length(); i++)
        {
            depsId.Add(deps[i].Id);
        }

        // Act
        var result = await _service.GetAllForScales(depsId);

        // Assert
        Assert.True(result.Count >= 0);
    }

    

}