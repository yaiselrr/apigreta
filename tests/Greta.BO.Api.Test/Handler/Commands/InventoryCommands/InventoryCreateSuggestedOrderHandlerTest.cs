using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Command.Inventory;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.Identity.Api.EventContracts.BO.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Commands.InventoryCommands;

public class InventoryCreateSuggestedOrderHandlerTest
{
    private readonly StoreProductRepository _repository;
    private readonly BOUserRepository _repositoryUser;

    private readonly InventoryCreateSuggestedOrderHandler _handler;

    public InventoryCreateSuggestedOrderHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(InventoryCreateSuggestedOrderHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new StoreProductRepository(TestsSingleton.Auth, context);
        var repositoryQty = new QtyHandTrackRepository(TestsSingleton.Auth, context);
        _repositoryUser = new BOUserRepository(TestsSingleton.Auth, context);
        var repositoryVendorOrder = new VendorOrderRepository(TestsSingleton.Auth, context);
       

        var shelfTagService = new ShelfTagService(Mock.Of<IShelfTagRepository>(), Mock.Of<ILogger<ShelfTagService>>());

        var service = new StoreProductService(_repository, repositoryQty, shelfTagService, Mock.Of<IRoundingTableService>(),Mock.Of<ISynchroService>(), Mock.Of<MediatR.IMediator>(),Mock.Of<ILogger<StoreProductService>>());

        var userServices = new BOUserService(_repositoryUser,Mock.Of<ILogger<BOUserService>>(), Mock.Of<ISynchroService>(), Mock.Of<IRequestClient<FilterUserRequestContract>>());
        
        var vendorOrderService = new VendorOrderService(repositoryVendorOrder, Mock.Of<ILogger<VendorOrderService>>());

        var user = TestsSingleton.Auth;
        user.UserName = "adrianalvarez";
        user.UserId = "12346798";
        
        _handler = new InventoryCreateSuggestedOrderHandler(
            Mock.Of<ILogger<InventoryCreateSuggestedOrderHandler>>(),
            service,
            vendorOrderService,
            userServices,
            user);
    }

    [Fact]
    public async Task CreateSuggestedOrder_NormalCall_ResultOk()
    {
        // Arrange              

        var permissions = new List<Permission>() { new Permission() { Name = "Permission", Code = "Code P" } };

        var profile = new Profiles() { Name = "Profile", Permissions = permissions};
        var prof = await _repositoryUser.CreateAsync(profile);

        var role = new Role() { Name = "Admin" };
        var rol = await _repositoryUser.CreateAsync(role);

        var user = new BOUser() { UserName = "adrianalvarez", BOProfileId = prof.Id, UserId = "12346798", RoleId = rol.Id, State = true };
        await _repositoryUser.CreateAsync(user);

        var vendor = new Vendor() { Name = "Vendor" };
        var vend = await _repository.CreateAsync(vendor);      

        var store = new Store() { Name = "Store", State = true };
        var str = await _repository.CreateAsync(store);

        var filter = new InventorySearchModel() { CreationDate = DateTime.Now };

        var command = new InventoryCreateSuggestedOrderCommand(str.Id, vend.Id, filter);

        // Act

        var result = await _handler.Handle(command);

        // Assert

        Assert.IsType<long>(result.Data);
        Assert.True(result.Data > 0);
    }

    [Fact]
    public async Task CreateSuggestedOrder_WithException_ResultBusinessLogicException()
    {
        // Arrange              

        var vendor = new Vendor() { Name = "Vendor" };
        var vend = await _repository.CreateAsync(vendor);

        var store = new Store() { Name = "Store", State = true };
        var str = await _repository.CreateAsync(store);

        var filter = new InventorySearchModel() { CreationDate = DateTime.Now };

        var command = new InventoryCreateSuggestedOrderCommand(str.Id, vend.Id, filter);

        // Act

        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            await _handler.Handle(command);
        });

        // Assert
       
        Assert.Equal("An exception occurred while creating a suggested order", exception.Message);
    }
}