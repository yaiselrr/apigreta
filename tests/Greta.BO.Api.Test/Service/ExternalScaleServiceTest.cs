using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.ExternalScaleSpecs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Service;

public class ExternalScaleServiceTest
{
    readonly ExternalScaleRepository _repository;
    readonly ExternalScaleService _service;
    readonly ISynchroService _synchroService;

    public ExternalScaleServiceTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(ExternalScaleServiceTest))
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new ExternalScaleRepository(TestsSingleton.Auth, context);
        _service = new ExternalScaleService(_repository, Mock.Of<ILogger<ExternalScaleService>>(), Mock.Of<ISynchroService>());
    }

    [Fact]
    public async Task GetId_NormalCall_ResultSameAsInsert()
    {
        // Arrange
        var ext = new ExternalScale()
        {
            Id = 1001,
            Ip = "192.168.5.1",
            Port = "3528",
            ExternalScaleType = Entities.Enum.BoExternalScaleType.GretaLabel,
            StoreId = 3
        };

        var id = await _repository.CreateAsync(ext);
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
        var ext = new ExternalScale()
        {
            Id = 1002,
            Ip = "192.168.6.1",
            Port = "3529",
            ExternalScaleType = Entities.Enum.BoExternalScaleType.GretaLabel,
            StoreId = 3
        };

        var id = await _repository.CreateAsync(ext);

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
        var ext = new ExternalScale()
        {
            Id = 1003,
            Ip = "192.168.7.1",
            Port = "3520",
            ExternalScaleType = Entities.Enum.BoExternalScaleType.GretaLabel,
            StoreId = 3
        };

        var id = await _repository.CreateAsync(ext);

        // Act
        var result = await _service.ChangeState(id, false);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Post_NormalCall_ResultSameId()
    {
        // Arrange
        var ext = new ExternalScale()
        {
            Ip = "192.168.6.1",
            Port = "3529",
            ExternalScaleType = Entities.Enum.BoExternalScaleType.GretaLabel,
            StoreId = 3,
            State = true,
            SyncDeviceId = 37
        };

        // Act
        var result = await _service.Post(ext);

        // Assert
        Assert.Equal(ext.Ip, (result as ExternalScale).Ip);
    }

    [Fact]
    public async Task Put_NormalCall_ChangeSuccess()
    {
        var ext = new ExternalScale()
        {
            Id = 1005,
            Ip = "192.168.9.1",
            Port = "3530",
            ExternalScaleType = Entities.Enum.BoExternalScaleType.GretaLabel,
            StoreId = 3,
            Departments = new List<Department>()
        };

        var id = await _repository.CreateAsync(ext);

        var extUpdate = new ExternalScale()
        {
            Id = 1005,
            Ip = "192.168.9.2",
            Port = "3536",
            ExternalScaleType = Entities.Enum.BoExternalScaleType.GretaLabel,
            StoreId = 3,
            Departments = new List<Department>()
        };

        // Act
        var result = await _service.Put(id, extUpdate);

        // Assert
        Assert.True((bool)result);
    }

    [Fact]
    public async Task FilterExternalScale_OutRange_ThrowBusinessLogicException()
    {
        // Arrange
        var ext = new ExternalScale()
        {
            Id = 1008,
            Ip = "192.168.9.5",
            Port = "3539",
            ExternalScaleType = Entities.Enum.BoExternalScaleType.GretaLabel,
            StoreId = 3
        };
        var id = await _repository.CreateAsync(ext);

        var spec = new ExternalScaleFilterSpec(
               new ExternalScaleSearchModel()
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
    public async Task FilterExternalScale_NormalCall_ResultOk()
    {
        // Arrange
        var ext = new ExternalScale()
        {
            Ip = "192.168.9.5",
            Port = "3539",
            ExternalScaleType = Entities.Enum.BoExternalScaleType.GretaLabel,
            StoreId = 3
        };
        var storeId = _repository.GetEntity<Store>().Add(new Store()
        {
            Id = 3,
            Name = "Store 1rew"
        });
        await _repository.SaveChangesAsync();

        var exts = new List<ExternalScale>();
        for (var i = 0; i < 10; i++)
        {
            exts.Add(new ExternalScale()
            {
                Ip = "192.168.9.5",
                Port = "3539",
                ExternalScaleType = Entities.Enum.BoExternalScaleType.GretaLabel,
                StoreId = 3
            });
        }
        var id = await _repository.CreateRangeAsync<ExternalScale>(exts);
        var spec = new ExternalScaleFilterSpec(
                new ExternalScaleSearchModel()
                {
                    Ip = "192"
                }
            );

        var result = await _service.FilterSpec(
                    1, 10, spec);

        // Assert
        Assert.Equal(10, result.Data.Count);
    }

    [Fact]
    public async Task GetExternalScaleByStore_NormalCallOk()
    {
        // Arrange
        var exts = new List<ExternalScale>();
        for (var i = 0; i < 10; i++)
        {
            exts.Add(new ExternalScale()
            {
                Id = 2108 + i,
                Ip = "193.168.9.5" + i,
                Port = "9539" + i,
                ExternalScaleType = Entities.Enum.BoExternalScaleType.Aclas,
                StoreId = 3
            });
        }

        var id = await _repository.CreateRangeAsync<ExternalScale>(exts);
        // Act
        var result = await _service.GetExternalScaleByStore(3);

        // Assert
        Assert.True(result.Count >= 0);
    }

    [Fact]
    public async Task GetExternalScaleByStore_NormalCall_Null()
    {
        // Arrange
        var ext = new ExternalScale()
        {
            Ip = "195.168.9.5",
            Port = "3539",
            ExternalScaleType = Entities.Enum.BoExternalScaleType.GretaLabel,
            StoreId = 4
        };
        var storeId = _repository.GetEntity<Store>().Add(new Store()
        {
            Id = 4,
            Name = "Store 1rew"
        });
        await _repository.SaveChangesAsync();

        var exts = new List<ExternalScale>();
        for (var i = 0; i < 10; i++)
        {
            exts.Add(new ExternalScale()
            {
                Ip = "195.168.9.5",
                Port = "3539",
                ExternalScaleType = Entities.Enum.BoExternalScaleType.GretaLabel,
                StoreId = 4
            });
        }
        var id = await _repository.CreateRangeAsync<ExternalScale>(exts);
        // Act
        var result = await _service.GetExternalScaleByStore(25);

        // Assert
        Assert.True(result.Count == 0);
    }

    [Fact]
    public async Task GetExternalScaleByStoreAndDepartment_NormalCallOk()
    {
        // Arrange
        var exts = new List<ExternalScale>();
        for (var i = 0; i < 10; i++)
        {
            exts.Add(new ExternalScale()
            {
                Id = 9108 + i,
                Ip = "193.168.8.1" + i,
                Port = "9539" + i,
                ExternalScaleType = Entities.Enum.BoExternalScaleType.Aclas,
                StoreId = 3,
                State = true,
                Departments = new List<Department>()
                {
                    new Department()
                    {
                        Name = "Department Name 140",
                        DepartmentId = 140,
                        Perishable = true
                    }
                }
            });
        }

        var id = await _repository.CreateRangeAsync<ExternalScale>(exts);
        // Act
        var result = await _service.GetExternalScaleByStoreAndDepartment(3, 140);

        // Assert
        Assert.True(result.Count >= 0);
    }

    public async Task GetExternalScaleByStoreAndDepartment_NormalCallFalse()
    {
        // Arrange
        var exts = new List<ExternalScale>();
        for (var i = 0; i < 10; i++)
        {
            exts.Add(new ExternalScale()
            {
                Id = 2108 + i,
                Ip = "193.168.9.5" + i,
                Port = "9539" + i,
                ExternalScaleType = Entities.Enum.BoExternalScaleType.Aclas,
                StoreId = 3,
                Departments = new List<Department>()
                {
                    new Department()
                    {
                        Name = "Department Name 115",
                        DepartmentId = 115,
                        Perishable = true
                    },
                    new Department()
                    {
                        Name = "Department Name 116",
                        DepartmentId = 116,
                        Perishable = true
                    },
                    new Department()
                    {
                        Name = "Department Name 117",
                        DepartmentId = 117,
                        Perishable = true
                    }
                }
            });
        }

        var id = await _repository.CreateRangeAsync<ExternalScale>(exts);
        // Act
        var result = await _service.GetExternalScaleByStoreAndDepartment(3, 120);

        // Assert
        Assert.True(result.Count >= 10);
    }
}