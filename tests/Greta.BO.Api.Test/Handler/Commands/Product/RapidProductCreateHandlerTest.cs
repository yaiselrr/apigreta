using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Handlers.Command.Products;
using Greta.BO.BusinessLogic.Handlers.Command.StoreProduct;
using Greta.BO.BusinessLogic.Handlers.Command.VendorProduct;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;
using static Greta.BO.BusinessLogic.Handlers.Command.Products.ProductCreate;

namespace Greta.BO.Api.Test.Handler.Commands.Product;


public class RapidProductCreateHandlerTest
{
    private readonly ProductRepository _repository;
    private readonly CategoryRepository _categoryRepository;

    private readonly RapidProductCreateHandler _handler;

    private readonly Mock<IMediator> _mediatorMock;

    private readonly ProductService service;
    private readonly CategoryService categoryService;

    SqlServerContext context;
    IMapper mapper;


    public RapidProductCreateHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(RapidProductCreateHandlerTest))
                             .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                             .Options;

        context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new ProductRepository(TestsSingleton.Auth, context);
        _categoryRepository = new CategoryRepository(TestsSingleton.Auth, context);        

        service = new ProductService(Mock.Of<ILogger<ProductService>>(), _repository, null, null, null, null, null, Mock.Of<IMediator>() );   
                
        _mediatorMock = new Mock<IMediator>();

        _handler = new RapidProductCreateHandler(Mock.Of<ILogger<RapidProductCreateHandler>>(), 
                                                TestsSingleton.Mapper, _mediatorMock.Object, service, categoryService);
       

        var config = new MapperConfiguration(cfg => cfg.AddProfile<TestMappingProfile>());
        mapper = config.CreateMapper();
    }


    // [Fact]
    // public async Task Post_NormalCall_ResultSameId()
    // {
    //     //Arrange
    //
    //     Category category = new Category();
    //     category.Name = "Category";
    //     category.VisibleOnPos = true;
    //     category.PromptPriceAtPOS = true;
    //     category.SnapEBT = true;
    //     category.PrintShelfTag = true;
    //     category.NoPriceOnShelfTag = true;
    //     category.MinimumAge = 10;
    //     category.AddOnlineStore = true;
    //     category.Modifier = true;
    //     var cat = await _repository.CreateAsync(category);       
    //
    //     Department department = new Department();
    //     department.Name = "Department";
    //     var dep = await _repository.CreateAsync(department);
    //
    //     Store store = new Store();
    //     store.Name = "Store";        
    //     var str = await _repository.CreateAsync(store);
    //
    //     Vendor vendor = new Vendor();
    //     vendor.Name = "Vendor";
    //     var vend = await _repository.CreateAsync(vendor);
    //
    //     RapidProductModel rapidProductModel = new RapidProductModel();
    //     rapidProductModel.UPC = "123465";
    //     rapidProductModel.Name = "Product";
    //     rapidProductModel.CategoryId = category.Id;
    //     rapidProductModel.StoreId = str.Id;
    //     rapidProductModel.VendorId = vend.Id;
    //     rapidProductModel.DepartmentId = dep.Id;
    //     rapidProductModel.ProductType = ProductType.SPT;
    //     rapidProductModel.RetailPrice = 25;
    //     rapidProductModel.CasePack = 5;
    //     rapidProductModel.CaseCost = 25;
    //     rapidProductModel.UnitCost = 5;
    //     rapidProductModel.OrderByCase = VendorProductType.UNIT;
    //
    //     //Act
    //     var cancellationToken = new CancellationToken();
    //
    //     var command = new RapidProductCreateCommand(rapidProductModel);
    //     
    //     _mediatorMock.Setup(m => m.Send(It.IsAny<ProductCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ProductResult());
    //     _mediatorMock.Setup(m => m.Send(It.IsAny<StoreProductCreateCommand>(), default)).ReturnsAsync(new StoreProductCreateResponse());
    //     _mediatorMock.Setup(m => m.Send(It.IsAny<VendorProductCreate.Command>(), default)).ReturnsAsync(new VendorProductCreate.Response());
    //
    //     var result = await _handler.Handle(command, cancellationToken);
    //
    //     var storeProduct = _repository.GetEntity<StoreProduct>().FirstOrDefault();
    //     var vendorProduct = _repository.GetEntity<VendorProduct>().FirstOrDefault();
    //
    //     //Assert
    //
    //     _mediatorMock.Verify(m => m.Send(It.Is<ProductCommand>(c => c.Product != null), default));
    //     _mediatorMock.Verify(m => m.Send(It.Is<StoreProductCreateCommand>(c => c.storeproduct != null), default));
    //     _mediatorMock.Verify(m => m.Send(It.Is<VendorProductCreate.Command>(c => c != null), default));
    //
    //     Assert.NotNull(storeProduct);
    //     Assert.NotNull(vendorProduct);
    //     Assert.NotNull(result);
    // }
}