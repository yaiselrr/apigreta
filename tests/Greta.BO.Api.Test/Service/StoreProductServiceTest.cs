using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MassTransit.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Service;

public class StoreProductServiceTest
{
    readonly StoreProductRepository _repository;

    readonly StoreProductService _service;

    public StoreProductServiceTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();

        var options = builder.UseInMemoryDatabase(nameof(StoreProductServiceTest))        
        .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
        .Options;        
        
        var context = new SqlServerContext(options, TestsSingleton.Auth);
        
        _repository = new StoreProductRepository(TestsSingleton.Auth, context);
        var repositoryQty = new QtyHandTrackRepository(TestsSingleton.Auth, context);

        var shelfTagService = new ShelfTagService(Mock.Of<IShelfTagRepository>(), Mock.Of<ILogger<ShelfTagService>>());

        _service = new StoreProductService(_repository, repositoryQty, shelfTagService, Mock.Of<IRoundingTableService>(),Mock.Of<ISynchroService>(), Mock.Of<MediatR.IMediator>(),Mock.Of<ILogger<StoreProductService>>());
    }

    [Fact]
    public async Task Update_NormalCall_ChangeSuccess()
    {
        // Arrange

        var product = new Product() { Name = "Product", UPC = "UPC", State = true, 
                                      Category = new Category() { Name = "Category" }, 
                                      Department = new Department() { Name = "Department" } };

        var prod = await _repository.CreateAsync(product);

        var store = new Store() { Name = "Store", State = true };
        var str = await _repository.CreateAsync(store);

        var binLocation = new BinLocation() { Name = "BinLocation", State = true };
        var binLoc = await _repository.CreateAsync(binLocation);

        var storeProducts = new List<StoreProduct>();
        for (var i = 0; i < 3; i++)
        {
            storeProducts.Add(new StoreProduct()
            {
                ProductId = prod.Id,
                OrderAmount = 3,
                Cost = 5,
                BinLocationId = binLocation.Id,
                BatchOldPrice = 2,
                GrossProfit = 3,
                QtyHand = 2,
                StoreId = str.Id
            });
        }

        await _repository.CreateRangeAsync(storeProducts);

        var update = new InventoryUpdateModel() { 
            StoreProductId = storeProducts.First().Id,
            BinLocationId = binLoc.Id,
            OrderAmount = 7,
            OrderTrigger = 7,
            QtyHand = 7
        };

        // Act
        var result = await _service.UpdateInventory(update);

        // Assert
        Assert.IsType<StoreProduct>(result);
        Assert.NotNull(result);
        Assert.Equal(storeProducts.First().Id, result.Id);
        Assert.Equal(storeProducts.First().OrderAmount, update.OrderAmount);
    }

    [Fact]
    public async Task Update_CallWithIdNotFound_ResultNull()
    {
        // Arrange

        var update = new InventoryUpdateModel()
        {
            StoreProductId = 7,
            BinLocationId = 7,
            OrderAmount = 7,
            OrderTrigger = 7,
            QtyHand = 7
        };

        // Act
        var result = await _service.UpdateInventory(update);

        // Assert
        
        Assert.Null(result);       
    }

    [Fact]
    public async Task UpdateOrderAmount_NormalCall_ChangeSuccess()
    {
        // Arrange

        var product = new Product()
        {
            Name = "Product",
            UPC = "UPC",
            State = true,
            Category = new Category() { Name = "Category" },
            Department = new Department() { Name = "Department" }
        };

        var prod = await _repository.CreateAsync(product);

        var store = new Store() { Name = "Store", State = true };
        var str = await _repository.CreateAsync(store);

        var binLocation = new BinLocation() { Name = "BinLocation", State = true };
        await _repository.CreateAsync(binLocation);

        var storeProducts = new List<StoreProduct>();
        for (var i = 0; i < 3; i++)
        {
            storeProducts.Add(new StoreProduct()
            {
                ProductId = prod.Id,
                OrderAmount = 3,
                Cost = 5,
                BinLocationId = binLocation.Id,
                BatchOldPrice = 2,
                GrossProfit = 3,
                QtyHand = 2,
                StoreId = str.Id
            });
        }

        await _repository.CreateRangeAsync(storeProducts);       

        // Act
        var result = await _service.PutByOrderAmount(storeProducts.First().Id, 12);

        // Assert
        Assert.True(result);        
    }

    [Fact]
    public async Task UpdateOrderAmount_WithInvalidId_ResultBusinessLogicException()
    {
        // Arrange

        // Act
        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            await _service.PutByOrderAmount(-1, 12);
        });

        // Assert
        Assert.Equal("Id parameter out of bounds", exception.Message);
    }

    [Fact]
    public async Task ProcessFiscalInventory_NormalCall_ChangeSuccess()
    {
        // Arrange

        var product = new Product()
        {
            Name = "Product",
            UPC = "UPC",
            State = true,
            Category = new Category() { Name = "Category" },
            Department = new Department() { Name = "Department" }
        };

        var prod = await _repository.CreateAsync(product);

        var store = new Store() { Name = "Store", State = true };
        var str = await _repository.CreateAsync(store);

        var binLocation = new BinLocation() { Name = "BinLocation", State = true };
        await _repository.CreateAsync(binLocation);

        var storeProducts = new List<StoreProduct>();
        for (var i = 0; i < 3; i++)
        {
            storeProducts.Add(new StoreProduct()
            {
                ProductId = prod.Id,
                OrderAmount = 3,
                Cost = 5,
                BinLocationId = binLocation.Id,
                BatchOldPrice = 2,
                GrossProfit = 3,
                QtyHand = 2,
                StoreId = str.Id
            });
        }

        await _repository.CreateRangeAsync(storeProducts);

        var process = new InventoryFiscalModel() { 
            StoreId = str.Id,
            DateTime = DateTime.Now,
            Items = new List<InventoryFiscalItemModel>() { 
                new InventoryFiscalItemModel(){ Name = "Updated", QtyHand = 2, Count = 1, CountSold = 2, Id = 1, UPC = "UPC", Update = false }
            }
        };

        // Act
        var result = await _service.ProcessFiscalInventory(process);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task PreProcessFiscalInventory_NormalCall_ChangeSuccess()
    {
        // Arrange

        var product = new Product()
        {
            Name = "Product",
            UPC = "UPC",
            State = true,
            Category = new Category() { Name = "Category" },
            Department = new Department() { Name = "Department" }
        };

        var prod = await _repository.CreateAsync(product);

        var store = new Store() { Name = "Store", State = true };
        var str = await _repository.CreateAsync(store);

        var binLocation = new BinLocation() { Name = "BinLocation", State = true };
        await _repository.CreateAsync(binLocation);

        var storeProducts = new List<StoreProduct>();
        for (var i = 0; i < 3; i++)
        {
            storeProducts.Add(new StoreProduct()
            {
                ProductId = prod.Id,
                OrderAmount = 3,
                Cost = 5,
                BinLocationId = binLocation.Id,
                BatchOldPrice = 2,
                GrossProfit = 3,
                QtyHand = 2,
                StoreId = str.Id
            });
        }

        await _repository.CreateRangeAsync(storeProducts);

        var process = new InventoryFiscalModel()
        {
            StoreId = str.Id,
            DateTime = DateTime.Now,
            Items = new List<InventoryFiscalItemModel>() {
                new InventoryFiscalItemModel(){ Name = "Updated", QtyHand = 2, Count = 1, CountSold = 2, Id = 1, UPC = "UPC", Update = false }
            }
        };

        // Act
        var result = await _service.PreprocessFiscalInventory(process);

        // Assert
        Assert.IsType<InventoryFiscalModel>(result);
        Assert.NotNull(result);
    }
}