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


public class SacaleProductFilterByUpcPluProductHandlerTest
{
    private readonly ScaleProductRepository _repository;
    private readonly CutListTemplateRepository _repositoryCutListTemplate;
    private readonly ScaleProductGetByUpcPluProductHandler _handler;
    private readonly CutListTemplateService _cutListTemplateService;
    private SqlServerContext context;
    IMapper _mapper;
    private DbContextOptions options;

    public SacaleProductFilterByUpcPluProductHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        options = builder.UseInMemoryDatabase(nameof(SacaleProductFilterByUpcPluProductHandlerTest)).Options;
        context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new ScaleProductRepository(TestsSingleton.Auth, context);
        _repositoryCutListTemplate = new CutListTemplateRepository(TestsSingleton.Auth, context);
        var service = new ProductService(Mock.Of<ILogger<ProductService>>(), null,null, _repository, null, null, null, Mock.Of<IMediator>());

        _cutListTemplateService = new CutListTemplateService(_repositoryCutListTemplate, Mock.Of<ILogger<CutListTemplateService>>());

        _handler = new ScaleProductGetByUpcPluProductHandler(
            Mock.Of<ILogger<ScaleProductGetByUpcPluProductHandler>>(),
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
        scaleProducts.Add(new ScaleProduct() { Id = 123, Name = "ScaleProduct123", UPC = "UPC123", Description1 = "Desc1" });
        scaleProducts.Add(new ScaleProduct() { Id = 124, Name = "ScaleProduct124", UPC = "UPC124", Description1 = "Desc2" });
        var scaleProductsCreated = await _repository.CreateRangeAsync(scaleProducts);
        await context.SaveChangesAsync();

        List<long> idsScaleProduct = new List<long>();
        idsScaleProduct.Add(scaleProducts.First().Id);

        var cutListTemplate = new CutListTemplateModel() {
            Name = "CutListWithScaleProduct",
            ScaleProductIds = new List<long>(124)
        };

        var context1 = new SqlServerContext(options, TestsSingleton.Auth);

        var repository = new CutListTemplateRepository(TestsSingleton.Auth, context1);
        var service = new CutListTemplateService(repository, Mock.Of<ILogger<CutListTemplateService>>());
        var cutListTemplateCreated = await service.Post(_mapper.Map<CutListTemplate>(cutListTemplate));

        var filter = new ScaleProductSearchModel()
        {
            Name = "ScaleProduct124",
            Search = "",
            Sort = "",
            Upc = ""
        };
        var query = new ScaleProductGetByUpcPluProductQuery(cutListTemplateCreated.Id, 1, 1, filter);

        // Act

        var result = await _handler.Handle(query);

        // Assert
        Assert.Equal("ScaleProduct124", result.Data.Data.First().Name);
        Assert.Equal(124, result.Data.Data.First().Id);
    }
}