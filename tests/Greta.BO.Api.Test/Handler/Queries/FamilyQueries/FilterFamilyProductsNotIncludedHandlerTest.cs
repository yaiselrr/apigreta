using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Queries.Family;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using LanguageExt.ClassInstances.Pred;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Queries.FamilyQueries;


public class FilterFamilyProductsNotIncludedHandlerTest
{
    private readonly FamilyRepository _repository;
    private readonly ProductRepository _productRepository;
    private readonly ProductFilterNotIncludedInFamilyHandler _handler;

    public FilterFamilyProductsNotIncludedHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(FamilyFilterHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new FamilyRepository(TestsSingleton.Auth, context);
        _productRepository = new ProductRepository(TestsSingleton.Auth, context);
        var productService = new ProductService(
            Mock.Of<ILogger<ProductService>>(),
            _productRepository,
            null,
            null,
            null,
            null,
            null,
            Mock.Of<IMediator>()
            );
       
        _handler = new ProductFilterNotIncludedInFamilyHandler(
            Mock.Of<ILogger<ProductFilterNotIncludedInFamilyHandler>>(),
            productService,
            TestsSingleton.Mapper);
    }
    
    [Fact]
    public async Task FilterFamilyProductsNotIncluded_NormalCall_ResultOk()
    {
        // Arrange
        var name = "FilterFamilyProductsNotIncluded_NormalCall_ResultOk";
        var family = new Family()
        {
            Name = name
        };
        var id = await _repository.CreateAsync(family);

        var prods = new List<Product>();
        for (var i=0; i < 3; i++)
        {
            prods.Add(new Product()
            {
                UPC = "UPC"+i,
                Name = "Name"+i,
                ProductType = ProductType.SPT, State = true
            });
        }
        var productsSaved = await _productRepository.CreateRangeAsync<Product>(prods);
        
        // Act1
        var query = new ProductFilterNotIncludedInFamilyQuery(id, 
            1, 
            20,
            new ProductSearchModel()
            {
                ProductType = ProductType.SPT,
                ProductTypeExcept = -1,
                UPC = "",
                Name = "",
                Description = "",
                State = true,
                DepartmentId = -1,
                CategoryId = -1,
                FamilyId = -1
            });
        var result = await _handler.Handle(query);
       
        // Assert1
        Assert.Equal(3,result.Data.Data.Count);
        
        //Adding one product to the family
        productsSaved[0].FamilyId = id;
        bool saved = await _productRepository.UpdateAsync(productsSaved[0]);
        
        // Act2
        var result2 = await _handler.Handle(query);
        
        // Assert2
        Assert.Equal(2, result2.Data.Data.Count );
        
    }
}