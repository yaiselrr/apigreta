using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Handlers.Queries.CutListTemplate;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Queries.CutListTemplateQueries;

public class CutListTemplateGetByIdHandlerTest
{
    private readonly CutListTemplateRepository _repository;
    private readonly CutListTemplateGetByIdHandler _handler;
    private readonly CutListTemplateService _service;
    IMapper _mapper;
    
    public CutListTemplateGetByIdHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(CutListTemplateGetByIdHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new CutListTemplateRepository(TestsSingleton.Auth, context);
        var service = new CutListTemplateService(_repository, Mock.Of<ILogger<CutListTemplateService>>());

        _service = service;

        _handler = new CutListTemplateGetByIdHandler(            
            service,
            TestsSingleton.Mapper);

        var config = new MapperConfiguration(cfg => cfg.AddProfile<TestMappingProfile>());
        _mapper = config.CreateMapper();
    }   

    [Fact]
    public async Task GetById_CallWithInvalidNumber_ReturnNULL()
    {
        // Arrange       

        var name = "GetById_CallWithInvalidNumber_ReturnNULL";
        var cutListTemplate = new CutListTemplateModel()
        {
            Name = name,
           
        };

        await _repository.CreateAsync(_mapper.Map<CutListTemplate>(cutListTemplate));
        // Act
       
        var query = new CutListTemplateGetByIdQuery(-13);

        var result = await _handler.Handle(query);
       
        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task GetById_CallWithValidNumberButNotPresentId_ReturnNULL()
    {
        // Arrange
        var name = "GetById_CallWithValidNumberButNotPresentId_ReturnNULL";
        var cutListTemplate = new CutListTemplate()
        {
            Name = name
        };
        await _repository.CreateAsync(cutListTemplate);
        // Act
        var query = new CutListTemplateGetByIdQuery(1000);
        var result = await _handler.Handle(query);

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task GetById_CallWithValidNumber_ReturnObjectWithSameIdAndInclude()
    {
        // Arrange
        List<ScaleProduct> scaleProducts = new List<ScaleProduct>();
        scaleProducts.Add(new ScaleProduct() { Name = "ScaleProduct1", UPC = "UPC1", Description1 = "Desc1" });
        scaleProducts.Add(new ScaleProduct() { Name = "ScaleProduct2", UPC = "UPC2", Description1 = "Desc2" });
        var scaleProductsCreated = await _repository.CreateRangeAsync(scaleProducts);

        var name = "GetById_CallWithValidNumber_ReturnObjectWithSameIdAndInclude";
        var cutListTemplate = new CutListTemplateModel()
        {
            Name = name            
        };

        var id = await _repository.CreateAsync(_mapper.Map<CutListTemplate>(cutListTemplate));

        CutListTemplate cutListTemplateUpdate = new CutListTemplate();
        cutListTemplateUpdate = _mapper.Map<CutListTemplate>(cutListTemplate);
        cutListTemplateUpdate.ScaleProducts = scaleProducts;
        var r = await _service.Put(id, cutListTemplateUpdate);

        // Act
        var query = new CutListTemplateGetByIdQuery(id);
        var result = await _handler.Handle(query);        

        // Assert
        Assert.Equal(result.Data.ScaleProducts.Count, scaleProductsCreated.Count);
        Assert.Equal(id, result.Data.Id);
    }
   
}