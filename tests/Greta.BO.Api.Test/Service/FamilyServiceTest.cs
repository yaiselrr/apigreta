using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.FamilySpecs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Service;

public class FamilyServiceTest
{
    readonly FamilyRepository _repository;
    readonly FamilyService _service;

    public FamilyServiceTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(FamilyServiceTest))
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new FamilyRepository(TestsSingleton.Auth, context);
        _service = new FamilyService(_repository, Mock.Of<ILogger<FamilyService>>(), Mock.Of<ISynchroService>());
    }

    [Fact]
    public async Task FilterFamilyProductSpec_OutRange_ThrowBusinessLogicException()
    {
        // Arrange
        var name = "FilterSpec_OutRange_ThrowBusinessLogicException";
        var family = new Family()
        {
            Name = name
        };
        var id = await _repository.CreateAsync(family);
        // Act
        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var result = await _service.FilterFamily(
                0, 0, null);
        });

        // Assert
        Assert.Equal("Page parameter out of bounds.", exception.Message);
    }

    [Fact]
    public async Task FilterFamilyProductSpec_NormalCall_ResultOk()
    {
        // Arrange
        var name = "FilterFamilyProductSpec_NormalCall_ResultOk";
        var family = new Family()
        {
            Name = name,
            Products = new List<Product>()
        };
        for (var i = 0; i < 10; i++)
        {
            family.Products.Add(new Product()
            {
                Name = "Product Name " + i,
                UPC = "UPC" + i,
            });
        }

        var id = await _repository.CreateAsync(family);

        var spec = new ProductByFamilyIdSpec(
            null,
            null,
            id
        );
        // Act

        var result = await _service.FilterFamily(
            1, 10, spec);

        // Assert
        Assert.Equal(10, result.Data.Count);
    }

    [Fact]
    public async Task CanDeleted_WithSingleFamily_ResultTrue()
    {
        // Arrange
        var name = "CanDeleted_WithSingleFamily_ResultTrue";
        var family = new Family()
        {
            Name = name
        };
        var id = await _repository.CreateAsync(family);
        // Act
        var result = await _service.CanDeleted(id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CanDeleted_WithSParentFamily_ResultFalse()
    {
        // Arrange
        var name = "CanDeleted_WithSParentFamily_ResultFalse";
        var family = new Family()
        {
            Name = name,
            Products = new List<Product>()
            {
                new Product()
                {
                    Name = "PParent",
                    UPC = "UPCWP"
                }
            }
        };
        var id = await _repository.CreateAsync(family);
        // Act
        var result = await _service.CanDeleted(id);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CanDeleted_WithNotFoundFamily_ResultFalse()
    {
        // Arrange
        // Act
        var result = await _service.CanDeleted(100000);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task AddProductsToFamily_ResultTrue()
    {
        // Arrange
        var name = "AddProducts_WithSParentFamily_ResultOk";
        var family = new Family()
        {
            Name = name,
            Products = new List<Product>()
            {
                new Product()
                {
                    Name = "PParent",
                    UPC = "UPCWPV"
                },
                new Product()
                {
                    Name = "PParent2",
                    UPC = "UPCWP2V"
                }
            }
        };
        var id = await _repository.CreateAsync(family);

        List<String> list = new List<string>();

        for (int i = 0; i < family.Products.Length(); i++)
        {
            list.Add(family.Products[i].UPC);
        }
        // Act
        var result = await _service.AddProductsToFamily(id, list);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddProductsToFamily_ResultFalse()
    {
        // Arrange
        var name = "AddProductsToFamily_WithSParentFamily_ResultFalse";
        var family = new Family()
        {
            Name = name,
            Products = new List<Product>()
            {
                new Product()
                {
                    Name = "PParent",
                    UPC = "UPCWP"
                },
                new Product()
                {
                    Name = "PParent2",
                    UPC = "UPCWP2"
                }
            }
        };
        var id = await _repository.CreateAsync(family);

        List<String> list = new List<string>();

        for (int i = 0; i < 2; i++)
        {
            list.Add("UPCWP4" + i);
        }
        // Act
        var result = await _service.AddProductsToFamily(id, list);

        // Assert
        Assert.Equal("Some products could not be found in the selected store.", result);
    }

    [Fact]
    public void Get_NormalCall_ResultOk()
    {
        // Act
        var result = _service.Get().Result;

        // Assert
        Assert.True(result.Count >= 0);
    }

    [Fact]
    public async Task Put_NormalCall_ChangeSuccess()
    {
        // Arrange
        var family = new Family()
        {
            Name = "Family Name "
        };
        var id = await _repository.CreateAsync(family);

        var familyUpdate = new Family()
        {
            Id = id,
            Name = "Family Name Update",
            State = true
        };

        // Act
        var result = await _service.Put(id, familyUpdate);

        // Assert
        Assert.True((bool)result);
    }


    [Fact]
    public async Task Put_CallWithNotFound_ThrowBusinessLogicException()
    {
        // Arrange
        long id = -1000;
        var familyUpdate = new Family()
        {
            Name = "Family Name Update",
            State = true
        };
        // Act
        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var result = await _service.Put(id, familyUpdate);
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
        var family = new Family()
        {
            Name = "Family Name "
        };
        var id = await _repository.CreateAsync(family);

        // Act
        var result = await _service.ChangeState(id, false);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Post_NormalCall_ResultSameId()
    {
        // Arrange
        var family = new Family()
        {
            Name = "Family Name "
        };

        // Act
        var result = await _service.Post(family);

        // Assert
        Assert.Equal(family.Name, (result as Family).Name);
    }


    [Fact]
    public async Task GetId_NormalCall_ResultSameAsInsert()
    {
        // Arrange
        var family = new Family()
        {
            Name = "Family Name "
        };
        var id = await _repository.CreateAsync(family);
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
    public async Task DeleteProduct_ResultTrue()
    {
        // Arrange
        var name = "DeleteProduct_WithSParentFamily_ResultFalse";
        var family = new Family()
        {
            Name = name,
            Products = new List<Product>()
            {
                new Product()
                {
                    Name = "PParent",
                    UPC = "UPCWP"
                },
                new Product()
                {
                    Name = "PParent2",
                    UPC = "UPCWP2"
                }
            }
        };
        var id = await _repository.CreateAsync(family);

        // Act
        var result = await _service.DeleteProduct(id, family.Products[0].Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteRangeProduct_ResultTrue()
    {
        // Arrange
        var name = "DeleteRangeProduct_WithSParentFamily_ResultFalse";
        var family = new Family()
        {
            Name = name,
            Products = new List<Product>()
            {
                new Product()
                {
                    Name = "PParent",
                    UPC = "UPCWP"
                },
                new Product()
                {
                    Name = "PParent2",
                    UPC = "UPCWP2"
                }
            }
        };
        var id = await _repository.CreateAsync(family);

        List<long> list = new List<long>();

        for (int i = 0; i < family.Products.Length(); i++)
        {
            list.Add(family.Products[i].Id);
        }

        // Act
        var result = await _service.DeleteRangeProduct(id, list);

        // Assert
        Assert.True(result);
    }
}