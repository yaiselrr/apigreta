using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto.MixAndMatchDto;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Service;

public class MixAndMatchServiceTest
{
    readonly MixAndMatchRepository _repository;
    readonly MixAndMatchService _service;
    IMapper _mapper;

    public MixAndMatchServiceTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(MixAndMatchServiceTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new MixAndMatchRepository(TestsSingleton.Auth, context);        
        _service = new MixAndMatchService(_repository, Mock.Of<ILogger<MixAndMatchService>>());

        var config = new MapperConfiguration(cfg => cfg.AddProfile<TestMappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Put_NormalCall_ChangeSuccess()
    {
        //Arrange
        var mixAndMatch = new MixAndMatch()
        {
           Name = "MixAndMatch Name "
        };
        var id = await _repository.CreateAsync(mixAndMatch);

        var mixAndMatchUpdate = new MixAndMatch()
        {
            Id = id,
            Name = "MixAndMatch Name Update",
            State = !mixAndMatch.State,
            Products = new List<Product>(),
            Families = new List<Family>()
        };

        //Act
        var result = await _service.Put(id, mixAndMatchUpdate);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Put_CallWithNotFound_ThrowBusinessLogicException()
    {
        //Arrange
        long id = -1;
        var mixAndMatchUpdate = new MixAndMatch()
        {
            Name = "MixAndMatch Name Update",
            State = true
        };
        //Act
       var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
       {
           await _service.Put(id, mixAndMatchUpdate);
       });

        //Assert
        Assert.Equal("Id parameter out of bounds", exception.Message);
    }
    
    [Fact]
    public async Task Post_NormalCall_ResultSameId()
    {
        // Arrange
        List<Product> products = new List<Product>() { 
            new Product(){ Name = "Product1" , UPC = "UPC"},
            new Product(){ Name = "Product2" , UPC = "UPC"}
        };
        await _repository.CreateRangeAsync(products);

        List<Family> families = new List<Family>() {
            new Family(){ Name = "Family1" },
            new Family(){ Name = "Family2" }
        };
        await _repository.CreateRangeAsync(families);

        var idsProduct = products.Select(x => x.Id).ToList();
        var idsFamily = families.Select(x => x.Id).ToList();

        var mixAndMatch = new MixAndMatchModel()
        {
            Name = "MixAndMatch Name ",
            ProductIds = idsProduct,
            FamilyIds = idsFamily
        };

        // Act
        var result = await _service.Post(_mapper.Map<MixAndMatch>(mixAndMatch));

        // Assert
        Assert.Equal(mixAndMatch.Name, result.Name);
    }


    [Fact]
    public async Task GetId_NormalCall_ResultSameAsInsert()
    {
        // Arrange
        var mixAndMatch = new MixAndMatch()
        {
            Name = "MixAndMatch Name "
        };
        var id = await _repository.CreateAsync(mixAndMatch);
        // Act
        var result = await _service.Get(id);

        // Assert
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetId_CallNotExist_ReturnNull()
    {
        //Arrange
        long id = 1000;
        //Act
        var result = await _service.Get(id);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetId_CallNegative_ThrowBusinessLogicException()
    {
        // Arrange
        long id = -1000;
        // Act
        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            await _service.Get(id);
        });

        // Assert
        Assert.Equal("Id parameter out of bounds", exception.Message);
    }
}