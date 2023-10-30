using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Handler.Queries.VendorOrderQueries;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Handlers.Command.VendorOrder;
using Greta.BO.BusinessLogic.Handlers.Queries.VendorOrder;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Command.VendorOrderCommand;

public class VendorOrderReceivedHandlerTest
{
    private readonly VendorOrderRepository _repository;
    private readonly VendorOrderDetailRepository _repositoryDetail;
    private readonly VendorOrderDetailCreditRepository _repositoryDetailCredit;
    private readonly StoreProductRepository _repositoryStoreProduct;
    private readonly VendorProductRepository _repositoryVendorProduct;
    private readonly ProductRepository _repositoryProduct;
    private readonly VendorRepository _repositoryVendor;
    private readonly StoreRepository _repositoryStore;
    private readonly QtyHandTrackRepository _repositoryQtyHandTrack;

    private readonly VendorOrderReceivedHandler _handler;

    public VendorOrderReceivedHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(VendorOrderReceivedHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new VendorOrderRepository(TestsSingleton.Auth, context);
        _repositoryDetail = new VendorOrderDetailRepository(TestsSingleton.Auth, context);
        _repositoryDetailCredit = new VendorOrderDetailCreditRepository(TestsSingleton.Auth, context);
        _repositoryStoreProduct = new StoreProductRepository(TestsSingleton.Auth, context);
        _repositoryVendorProduct = new VendorProductRepository(TestsSingleton.Auth, context);
        _repositoryProduct = new ProductRepository(TestsSingleton.Auth, context);
        _repositoryVendor = new VendorRepository(TestsSingleton.Auth, context);
        _repositoryStore = new StoreRepository(TestsSingleton.Auth, context);
        _repositoryQtyHandTrack = new QtyHandTrackRepository(TestsSingleton.Auth, context);
        
        var service = new VendorOrderService(_repository, Mock.Of<ILogger<VendorOrderService>>());
        var serviceDetail = new VendorOrderDetailService(_repositoryDetail, Mock.Of<ILogger<VendorOrderDetailService>>());
        var serviceDetailCredit = new VendorOrderDetailCreditService(_repositoryDetailCredit, Mock.Of<ILogger<VendorOrderDetailCreditService>>());
        var serviceStoreProduct = new StoreProductService(_repositoryStoreProduct, _repositoryQtyHandTrack, 
            Mock.Of<IShelfTagService>(), Mock.Of<IRoundingTableService>(),Mock.Of<ISynchroService>(), Mock.Of<IMediator>(), Mock.Of<ILogger<StoreProductService>>());
        var serviceVendorProduct = new VendorProductService(_repositoryVendorProduct, Mock.Of<ILogger<VendorProductService>>(), Mock.Of<IShelfTagService>());
        var serviceProduct = new ProductService(Mock.Of<ILogger<ProductService>>(), _repositoryProduct,
                Mock.Of<IKitProductRepository>(), Mock.Of<IScaleProductRepository>(), Mock.Of<IImageRepository>(),
                Mock.Of<IShelfTagService>(), Mock.Of<ISynchroService>(), Mock.Of<IMediator>());
        var serviceVendor = new VendorService(_repositoryVendor, Mock.Of<ILogger<VendorService>>(), Mock.Of<ISynchroService>());
    
        _handler = new VendorOrderReceivedHandler(
            service,
            serviceDetail,
            serviceDetailCredit,
            serviceStoreProduct,
            serviceVendorProduct,
            _repositoryQtyHandTrack,
            serviceProduct,
            serviceVendor);
    }
    
    [Fact]
    public async Task VendorOrderReceivedHandler__UpdateVendorOrder()
    {
        // Arrange
        var name = "VendorOrderChangeStateHandler_StateFalse_ResultTrue";
        
        var vendor1 = new Vendor()
        {
            Name = "Vendor1",
            AccountNumber = "acc",
            MinimalOrder = 2,
            Address1 = "calle 1",
            Zip = "8256"
        };
        var idVendor1 = await _repositoryVendor.CreateAsync(vendor1);

        var product1 = new Product()
        {
            Id = 1,
            Name = "Product1",
            UPC = "123456"
        };
        var idProduct1 = await _repositoryProduct.CreateAsync(product1);

        var store1 = new Store()
        {
            Id = 1,
            Name = "Store1"
        };
        var idStore1 = await _repositoryStore.CreateAsync(store1);

        var storeProduct1 = new StoreProduct()
        {
            StoreId = 1,
            ProductId = 1,
            Price = 30,
            QtyHand = 10,
            Cost = 20
        };
        var idStoreProduct1 = await _repositoryStoreProduct.CreateAsync(storeProduct1);
        

        var vendorOrder = new VendorOrder()
        {
            State = false
        };
        var id = await _repository.CreateAsync(vendorOrder);
        
        //TODO: Yadira Continue the test
        // Act
        var request = new VendorOrderChangeStateCommand(id, true);
        //var result = await _handler.Handle(request);

        // Assert
        //Assert.True(result.Data);
        Assert.True(true);
    }
}