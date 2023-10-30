using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Greta.BO.Api.Test.Service;

public class CutListTemplateServiceTest
{
    readonly CutListTemplateRepository _repository;
    private ScaleProductRepository _repositoryProduct;
    readonly CutListTemplateService _service;
    private SqlServerContext context;
    IMapper _mapper;
    private DbContextOptions options;

    public CutListTemplateServiceTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        options = builder.UseInMemoryDatabase(nameof(CutListTemplateServiceTest)).Options;
        context = new SqlServerContext(options, TestsSingleton.Auth);

        _repositoryProduct = new ScaleProductRepository(TestsSingleton.Auth, context);       
        
        _repository = new CutListTemplateRepository(TestsSingleton.Auth, context);        
        _service = new CutListTemplateService(_repository, Mock.Of<ILogger<CutListTemplateService>>());

        var config = new MapperConfiguration(cfg => cfg.AddProfile<TestMappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Post_NormalCall_ResultSameId()
    {
        // Arrange     

        var cutListTemplate = new CutListTemplateModel()
        {
            Name = "CutListTemplate Name "              
        };

        // Act
        var result = await _service.Post(_mapper.Map<CutListTemplate>(cutListTemplate));

        // Assert
        Assert.Equal(cutListTemplate.Name, result.Name);
    }

    [Fact]
    public async Task Post_NormalCallWithScaleProducts_ResultSameId()
    {
        // Arrange     

        List<ScaleProduct> scaleProducts = new List<ScaleProduct>();
        scaleProducts.Add(new ScaleProduct() { Id = 123, Name = "ScaleProduct1", UPC = "UPC1", Description1 = "Desc1" });
        scaleProducts.Add(new ScaleProduct() { Id = 124, Name = "ScaleProduct2", UPC = "UPC2", Description1 = "Desc2" });
        var scaleProductsCreated = await _repositoryProduct.CreateRangeAsync(scaleProducts);

        await context.SaveChangesAsync();
        
        var cutListTemplate = new CutListTemplateModel()
        {
            Name = "CutListTemplate Name1 ",
            ScaleProductIds = new List<long>(){ 123, 124 }
        };
        
        var context1 = new SqlServerContext(options, TestsSingleton.Auth);
        
        var repository = new CutListTemplateRepository(TestsSingleton.Auth, context1);        
        var service = new CutListTemplateService(repository, Mock.Of<ILogger<CutListTemplateService>>());

        // Act
        var result = await service.Post(_mapper.Map<CutListTemplate>(cutListTemplate));


        var result2 = await repository.GetEntity<CutListTemplate>()
            .Include(x => x.ScaleProducts)
            .FirstOrDefaultAsync(x => x.Id == result.Id);


        // Assert
        Assert.Equal(cutListTemplate.Name, result.Name);
        Assert.Equal(2, result2.ScaleProducts.Count);
    }

    [Fact]
    public async Task Put_NormalCall_ChangeSuccess()
    {
        //Arrange
        var cutListTemplate = new CutListTemplateModel()
        {
            Name = "CutListTemplate Name ",
            ScaleProductIds = new List<long>()
        };
        var id = await _repository.CreateAsync(_mapper.Map<CutListTemplate>(cutListTemplate));

        var cutListTemplateUpdate = new CutListTemplateModel()
        {
            Id = id,
            Name = "CutListTemplate Name Update",
            State = !cutListTemplate.State            
        };

        //Act
        var result = await _service.Put(id, _mapper.Map<CutListTemplate>(cutListTemplateUpdate));

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Put_NormalCallChangeScaleProduct_ChangeSuccess()
    {
        //Arrange
        List<ScaleProduct> scaleProducts = new List<ScaleProduct>();
        scaleProducts.Add(new ScaleProduct() { Id=321, Name = "ScaleProduct1", UPC = "UPC1" , Description1 = "Desc1"});
        scaleProducts.Add(new ScaleProduct() { Id=322, Name = "ScaleProduct2", UPC = "UPC2" , Description1 = "Desc2" });
        scaleProducts.Add(new ScaleProduct() { Id=323, Name = "ScaleProduct3", UPC = "UPC3" , Description1 = "Desc3" });
        var scaleProductsCreated = await _repositoryProduct.CreateRangeAsync(scaleProducts);
        await context.SaveChangesAsync();
        var cutListTemplate = new CutListTemplateModel()
        {
            Name = "CutListTemplate Name000",
            ScaleProductIds = new List<long>(){321, 322, 323},
        };
        
        
        var context1 = new SqlServerContext(options, TestsSingleton.Auth);
        
        var repository = new CutListTemplateRepository(TestsSingleton.Auth, context1);        
        var service = new CutListTemplateService(repository, Mock.Of<ILogger<CutListTemplateService>>());
        
        var id = await service.Post(_mapper.Map<CutListTemplate>(cutListTemplate));

        var cutListTemplatehUpdate = new CutListTemplateModel()
        {
            Id = id.Id,
            Name = "CutListTemplate Name",
            ScaleProductIds = new List<long>(){321, 323},
        };
        //Act
        var result = await service.Put(id.Id, _mapper.Map<CutListTemplate>(cutListTemplatehUpdate));
     
        var result2 = await repository.GetEntity<CutListTemplate>()
            .Include(x => x.ScaleProducts)
            .FirstOrDefaultAsync(x => x.Id == id.Id);
        //Assert        
        Assert.True(result);
        Assert.Equal(2, result2.ScaleProducts.Count);
    }

    [Fact]
    public async Task Put_CallWithNotFound_ThrowBusinessLogicException()
    {
        //Arrange
        long id = -1;
        var cutListTemplate = new CutListTemplate()
        {
            Name = "CutListTemplate Name Update",
            State = true
        };
        //Act
       var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
       {
           await _service.Put(id, cutListTemplate);
       });

        //Assert
        Assert.Equal("Id parameter out of bounds", exception.Message);
    }
    
    
}