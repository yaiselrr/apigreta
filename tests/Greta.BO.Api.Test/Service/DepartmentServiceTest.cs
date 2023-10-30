using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.DepartmentSpecs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Service;

public class DepartmentServiceTest
{
    readonly DepartmentRepository _repository;
    readonly DepartmentService _service;

    public DepartmentServiceTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(DepartmentServiceTest)).ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new DepartmentRepository(TestsSingleton.Auth, context);
        _service = new DepartmentService(_repository, Mock.Of<ILogger<DepartmentService>>(), Mock.Of<ISynchroService>());
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
        var department = new Department()
        {
            Name = "Department Name 107",
            DepartmentId = 107,
            Perishable = true
        };

        var id = await _repository.CreateAsync(department);
        // Act
        var result = await _service.Get(id);

        // Assert
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetId_CallNotExist_ReturnNull()
    {
        // Arrange
        long id = 100000;
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
        var department = new Department()
        {
            Name = "Department Name 108",
            DepartmentId = 108,
            Perishable = true
        };

        var id = await _repository.CreateAsync(department);

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
        var department = new Department()
        {
            Name = "Department Name 109",
            DepartmentId = 109,
            Perishable = true
        };

        var id = await _repository.CreateAsync(department);

        // Act
        var result = await _service.ChangeState(id, false);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Post_NormalCall_ResultSameId()
    {
        // Arrange
        var department = new Department()
        {
            Name = "Department Name 110",
            DepartmentId = 110,
            Perishable = true
        };

        // Act
        var result = await _service.Post(department);

        // Assert
        Assert.Equal(department.Name, (result as Department).Name);
    }

    [Fact]
    public async Task Put_NormalCall_ChangeSuccess()
    {
        var department = new Department()
        {
            Name = "Department Name 111",
            DepartmentId = 111,
            Perishable = true
        };
        var id = await _repository.CreateAsync(department);

        var departmentUpdate = new Department()
        {
            Name = "Department Name 112",
            DepartmentId = 112,
            Perishable = false
        };

        // Act
        var result = await _service.Put(id, departmentUpdate);

        // Assert
        Assert.True((bool)result);
    }

    [Fact]
    public async Task FilterDepartment_OutRange_ThrowBusinessLogicException()
    {
        // Arrange
        var department = new Department()
        {
            Name = "Department Name 113",
            DepartmentId = 113,
            Perishable = true
        };
        var id = await _repository.CreateAsync(department);

        var spec = new DepartmentFilterSpec(
           new DepartmentSearchModel()
        );
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
    public async Task FilterDepartment_NormalCall_ResultOk()
    {
        // Arrange
        var deps = new List<Department>();
        for (var i = 0; i < 10; i++)
        {
            deps.Add(new Department()
            {
                Name = "_fil_ Department Name " + i,
                DepartmentId = 1145 + i,
                Perishable = true
            });
        }

        var id = await _repository.CreateRangeAsync<Department>(deps);

        var spec = new DepartmentFilterSpec(
                new DepartmentSearchModel()
                {
                    Name = "_fil_"
                }
            );

        var result = await _service.FilterSpec(
                     1, 10, spec);

        // Assert
        Assert.Equal(10, result.Data.Count);
    }

    [Fact]
    public async Task GetByDepartmentId_NormalCall_ResultSameAsInsert()
    {
        // Arrange
        var department = new Department()
        {
            Name = "Department Name 115",
            DepartmentId = 115,
            Perishable = true
        };
        var id = await _repository.CreateAsync(department);
        // Act
        var result = await _service.GetByDepartmentId(department.DepartmentId);

        // Assert
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetByDepartmentId_NormalCall_ResultNull()
    {
        // Arrange
        int departmentId = 10000000;

        // Act
        var result = await _service.GetByDepartmentId(departmentId, -1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByDepartmentId1_NormalCall_ResultSameAsInsert()
    {
        // Arrange
        var department = new Department()
        {
            Name = "Department Name 116",
            DepartmentId = 116,
            Perishable = true
        };
        var id = await _repository.CreateAsync(department);
        // Act
        var result = await _service.GetByDepartmentId(department.DepartmentId, -1);

        // Assert
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetByPerishable_ReturnOk()
    {
        // Act
        var result = await _service.Get(true);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetByNoPerishable_ReturnOk()
    {
        // Act
        var result = await _service.Get(false);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetByDepartment_NormalCall_ResultSOk()
    {
        // Arrange
        var department = new Department()
        {
            Name = "Department Name 117",
            DepartmentId = 117,
            Perishable = true
        };
        var id = await _repository.CreateAsync(department);
        // Act
        var result = await _service.GetByDepartmentId(department.DepartmentId);

        // Assert
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetByDepartment1_NormalCall_ResultSOk()
    {
        // Arrange
        var department = new Department()
        {
            Name = "Department Name 118",
            DepartmentId = 118,
            Perishable = true
        };
        var id = await _repository.CreateAsync(department);
        // Act
        var result = await _service.GetByDepartmentId(department.DepartmentId, -1);

        // Assert
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetIdFromDepartmentId_NormalCall_ResultSameAsInsert()
    {
        // Arrange
        var department = new Department()
        {
            Name = "Department Name 119",
            DepartmentId = 119,
            Perishable = true
        };
        var id = await _repository.CreateAsync(department);
        // Act
        var result = await _service.GetIdFromDepartmentId(department.DepartmentId);

        // Assert
        Assert.Equal(id, result);
    }

    [Fact]
    public async Task GetIdFromDepartmentId_NormalCall_ResultNull()
    {
        // Arrange
        int departmentId = 10000000;

        // Act
        var result = await _service.GetIdFromDepartmentId(departmentId);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task CanDeleted_WithSingleDepartment_ResultTrue()
    {
        // Arrange
        var department = new Department()
        {
            Name = "Department Name 120",
            DepartmentId = 120,
            Perishable = true
        };
        var id = await _repository.CreateAsync(department);
        // Act
        var result = await _service.CanDeleted(id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CanDeleted_WithNotFoundDepartment_ResultFalse()
    {
        // Arrange
        // Act
        var result = await _service.CanDeleted(100000);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetAllForScales_NormalCall_ResultOk()
    {
        // Arrange
        var deps = new List<Department>();
        for (var i = 0; i < 10; i++)
        {
            deps.Add(new Department()
            {
                Name = "Department Name " + i,
                DepartmentId = 1201 + i,
                Perishable = true
            });
        }

        var id = await _repository.CreateRangeAsync<Department>(deps);

        var depsIds = new List<long>();

        for (var i = 0; i < deps.Length(); i++)
        {
            depsIds.Add(deps[i].Id);
        }

        var result = await _service.GetAllForScales(depsIds);

        // Assert
        Assert.True(result.Count >= 0);
    }

    [Fact]
    public async Task GetAllForScalesNullList_NormalCall_ResultOk()
    {
        // Arrange

        var depsIds = new List<long>();

        var result = await _service.GetAllForScales(depsIds);

        // Assert
        Assert.True(result.Count >= 0);
    }

    [Fact]
    public async Task GetByDepartment_NormalCall_ResultSameAsInsert()
    {
        // Arrange
        var department = new Department()
        {
            Name = "Department Name 122",
            DepartmentId = 122,
            Perishable = true
        };

        var id = await _repository.CreateAsync(department);

        // Act
        var result = await _service.GetByDepartment(department.DepartmentId, true);

        // Assert
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetByDepartmentFalse_NormalCall_ResultSameAsInsert()
    {
        // Arrange
        var department = new Department()
        {
            Name = "Department Name 123",
            DepartmentId = 123,
            Perishable = true
        };

        var id = await _repository.CreateAsync(department);

        // Act
        var result = await _service.GetByDepartment(department.DepartmentId, false);

        // Assert
        Assert.Equal(id, result.Id);
    }
}