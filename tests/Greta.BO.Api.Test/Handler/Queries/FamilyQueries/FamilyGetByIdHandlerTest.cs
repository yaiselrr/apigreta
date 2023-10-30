using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Queries.Family;
using Greta.BO.BusinessLogic.Service;
using MassTransit.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Queries.FamilyQueries;

public class FamilyGetByIdHandlerTest
{
    private readonly FamilyRepository _repository;
    private readonly FamilyGetByIdHandler _handler;

    public FamilyGetByIdHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(FamilyFilterHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new FamilyRepository(TestsSingleton.Auth, context);
        var service = new FamilyService(_repository, Mock.Of<ILogger<FamilyService>>(), Mock.Of<ISynchroService>());

        _handler = new FamilyGetByIdHandler(
            service,
            TestsSingleton.Mapper);
    }
    
    [Fact]
    public async Task FamilyGetById_CallWithInvalidNumber_ReturnNull()
    {
        // Arrange
        var mediator = new  Mock<IMediator>();
        var name = "FamilyGetById_CallWithInvalidNumber_ReturnNull";
        var family = new Family()
        {
            Name = name
        };
        var id = await _repository.CreateAsync(family);

        // Act
       
        var query = new FamilyGetByIdQuery(-13);

        var result = await _handler.Handle(query);        

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task FamilyGetById_CallWithValidNumber_ReturnObjectWithSameID()
    {
        // Arrange
        var name = "FilterSpec_OutRange_ThrowBusinessLogicException";
        var family = new Family()
        {
            Name = name
        };
        var id = await _repository.CreateAsync(family);
        // Act
        var query = new FamilyGetByIdQuery(id);
        var result = await _handler.Handle(query);

        // Assert
        Assert.Equal(id, result.Data.Id);
    }
}