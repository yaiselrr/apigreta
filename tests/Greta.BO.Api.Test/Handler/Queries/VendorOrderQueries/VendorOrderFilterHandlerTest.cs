
using Elastic.Apm.Api;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Handler.Queries.FamilyQueries;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Queries.Family;
using Greta.BO.BusinessLogic.Handlers.Queries.VendorOrder;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using LanguageExt.ClassInstances.Pred;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Queries.VendorOrderQueries;


public class VendorOrderFilterHandlerTest
{
    private readonly VendorOrderRepository _repository;
    private readonly VendorOrderFilterHandler _handler;
    private readonly SqlServerContext context;

    public VendorOrderFilterHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(VendorOrderFilterHandlerTest)).Options;
        context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new VendorOrderRepository(TestsSingleton.Auth, context);
        var service = new VendorOrderService(_repository, Mock.Of<ILogger<VendorOrderService>>());

        _handler = new VendorOrderFilterHandler(
            Mock.Of<ILogger<VendorOrderFilterHandler>>(),
            service,
            TestsSingleton.Mapper);
    }
    
    [Fact]
    public async Task FilterSpec_OutRange_ThrowBusinessLogicException()
    {
        // Arrange
        var name = "FilterSpec_OutRange_ThrowBusinessLogicException";
        var vendorOrder = new VendorOrder()
        {
            Vendor = new Vendor()
            {
                Name = "Vendor1"
            },
            Store = new Store()
            {
                Name = "Store1"
            }
        };
        var id = await _repository.CreateAsync(vendorOrder);
        // Act
        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var query = new VendorOrderFilterQuery(0, 0, null);

            var result = await _handler.Handle(query);
        });

        // Assert
        Assert.Equal("Page parameter out of bounds.", exception.Message);
    }
    
    /*
    [Fact]
    public async Task FilterSpec_NormalCall_ResultOk()
    {
        // Arrange
        var _repositoryVendor = new VendorRepository(TestsSingleton.Auth, context);
        var _repositoryStore = new StoreRepository(TestsSingleton.Auth, context);
        var _repositoryUser = new BOUserRepository(TestsSingleton.Auth, context);
        var v = new Vendor()
        {
            Id = 1,
            Name = "Vendor1",
            AccountNumber = "acc",
            MinimalOrder = 2,
            Address1 = "calle 1",
            Zip = "8256"
        };
        var s = new Store()
        {
            Id = 1,
            Name = "Store1"
        };
        var u = new BOUser()
        {
            Id = 1,
            UserName = "UserName1"
        };
        
        var idV = await _repositoryVendor.CreateAsync(v);
        var idS = await _repositoryStore.CreateAsync(s);
        var idU = await _repositoryUser.CreateAsync(u);
        
        var vOrders = new List<VendorOrder>();
        for (var i = 1; i <= 10; i++)
        {
            vOrders.Add(new VendorOrder()
            {
                VendorId = idV,
                Vendor = v,
                StoreId = idS,
                Store = s,
                UserId = idU,
                User = u,
                InvoiceNumber = "Invoice" + i,
                Status = VendorOrderStatus.Open,
                ReceivedDate = DateTime.Now,
                
                // State = true,
                // CreatedAt = DateTime.Now,
                // UpdatedAt = DateTime.Now,
                // AttachmentFilePath = "VVBDLCBQcm9kdWN0Q29kZSwgTmFtZSwgT3JkZXIgUXVhbnRpdHkKNTUxNzYwMDYxMywgNTY3NDMyLCAybmQgQ0hBTkNFIFNFSVpFIFRIRSBJUEEgNlBLIENBTlMsIDEwLjAwCg==",
                // UserCreatorId = "qqqq",
                // DeliveryCharge = 0,
                // SendCount = 0,
                // VendorOrderDetails = null
            });            
        }

        var id = await _repository.CreateRangeAsync<VendorOrder>(vOrders);
        var filter = new VendorOrderSearchModel()
        {
            InvoiceNumber = "",
            Search = "",
            Sort = "createdAt_asc",
            StoreId = -1,
            VendorId = -1
        };
        var query = new VendorOrderFilterQuery(1,10, filter);
        // Act
        var result = await _handler.Handle(query);

        //TODO: Yadira complete test
        // Assert
        //Assert.Equal(10, result.Data.Data.Count);
        Assert.Equal(10, 10);
    }
    */
}