using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Handlers.Queries.VendorOrder;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Queries.VendorOrderQueries;

public class VendorOrderHasProductsHandlerTest
{
    private readonly VendorOrderRepository _repository;
    private readonly VendorOrderHasProductsHandler _handler;

    public VendorOrderHasProductsHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(VendorOrderFilterHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new VendorOrderRepository(TestsSingleton.Auth, context);
        var service = new VendorOrderService(_repository, Mock.Of<ILogger<VendorOrderService>>());

        _handler = new VendorOrderHasProductsHandler(
            Mock.Of<ILogger<VendorOrderHasProductsHandler>>(),
            service,
            TestsSingleton.Mapper);
    }
    
    [Fact]
    public async Task VendorOrderHasProductsHandler_WithProducts_ResultTrue()
    {
        // Arrange
        var name = "HasProducts_WithProducts_ResultTrue";
        
        var vendorOrder = new VendorOrder()
        {
            VendorOrderDetails = new List<VendorOrderDetail>()
            {
                new VendorOrderDetail()
                {
                    Product = new Product()
                    {
                        Name = "PParent",
                        UPC = "UPCWP"
                    }
                }
            }
        };
        var id = await _repository.CreateAsync(vendorOrder);
        // Act
        var query = new VendorOrderHasProductsQuery(id);
        var result = await _handler.Handle(query);
        
        //var result = await _service.HasProducts(id);

        // Assert
        Assert.True(result.Data);
    }
}