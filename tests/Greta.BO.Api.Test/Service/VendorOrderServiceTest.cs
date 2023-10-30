using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.FamilySpecs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Service;

public class VendorOrderServiceTest
{
    readonly VendorOrderRepository _repository;
    readonly VendorOrderService _service;

    public VendorOrderServiceTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(VendorOrderServiceTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new VendorOrderRepository(TestsSingleton.Auth, context);
        _service = new VendorOrderService(_repository, Mock.Of<ILogger<VendorOrderService>>());
    }
    
    /*
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
                0,0,null);
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
            1,10, spec);

        // Assert
        Assert.Equal(10, result.Data.Count);
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
        var result = await _service.CanDeleted( id);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public async Task CanDeleted_WithNotFoundFamily_ResultFalse()
    {
        // Arrange
        // Act
        var result = await _service.CanDeleted( 100000);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public async Task AddProductsToFamily_NoFound_ResultFalse()
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
        var result = await _service.CanDeleted( id);

        // Assert
        Assert.False(result);
    }
     */
}