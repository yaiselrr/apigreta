using AutoMapper;
using Elastic.Apm.Api;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Handlers.Queries.Products;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Queries.ProductQueries;


public class SacaleProductByTemplate
{
    private readonly ScaleProductRepository _repository;
    private readonly CutListTemplateRepository _repositoryCutListTemplate;
    private readonly ScaleProductGetByCutListTemplateHandler _handler;
    private readonly CutListTemplateService _cutListTemplateService;
    private SqlServerContext context;
    IMapper _mapper;
    private DbContextOptions options;

    public SacaleProductByTemplate()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        options = builder.UseInMemoryDatabase(nameof(SacaleProductFilterByUpcPluProductHandlerTest)).Options;
        context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new ScaleProductRepository(TestsSingleton.Auth, context);
        _repositoryCutListTemplate = new CutListTemplateRepository(TestsSingleton.Auth, context);
        var service = new ProductService(Mock.Of<ILogger<ProductService>>(), null,null, _repository, null, null, null, Mock.Of<IMediator>());

        _cutListTemplateService = new CutListTemplateService(_repositoryCutListTemplate, Mock.Of<ILogger<CutListTemplateService>>());

        _handler = new ScaleProductGetByCutListTemplateHandler(
            Mock.Of<ILogger<ScaleProductGetByCutListTemplateHandler>>(),
            service,
            TestsSingleton.Mapper);

        var config = new MapperConfiguration(cfg => cfg.AddProfile<TestMappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task FilterSpec_NormalCall_ResultOk()
    {
        // Arrange       

        List<ScaleProduct> scaleProducts = new List<ScaleProduct>();
        scaleProducts.Add(new ScaleProduct() { Id = 121, Name = "ScaleProduct1", UPC = "UPC121", Description1 = "Desc1" });
        scaleProducts.Add(new ScaleProduct() { Id = 122, Name = "ScaleProduct2", UPC = "UPC122", Description1 = "Desc2" });
        var scaleProductsCreated = await _repository.CreateRangeAsync(scaleProducts);
        await context.SaveChangesAsync();

        List<long> idsScaleProduct = new List<long>();
        idsScaleProduct.Add(scaleProducts.Last().Id);

        var cutListTemplate = new CutListTemplateModel() {
            Name = "CutListWithScaleProduct",
            ScaleProductIds = idsScaleProduct
        };

        var context1 = new SqlServerContext(options, TestsSingleton.Auth);

        var repository = new CutListTemplateRepository(TestsSingleton.Auth, context1);
        var service = new CutListTemplateService(repository, Mock.Of<ILogger<CutListTemplateService>>());
        var cutListTemplateCreated = await service.Post(_mapper.Map<CutListTemplate>(cutListTemplate));
                
        var query = new ScaleProductGetByCutListTemplateQuery(cutListTemplateCreated.Id);

        // Act

        var result = await _handler.Handle(query);

        // Assert
       
        Assert.Equal(122, result.Data.First().Id);
    }
}