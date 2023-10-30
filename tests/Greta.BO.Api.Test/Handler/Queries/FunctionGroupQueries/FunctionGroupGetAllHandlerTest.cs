using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Queries.FunctionGroup;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MassTransit.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;

namespace Greta.BO.Api.Test.Handler.Queries.FunctionGroupQueries;

public class FunctionGroupGetAllHandlerTest
{
    private readonly FunctionGroupRepository _repository;    
    private readonly FunctionGroupGetAllHandler _handler;
    private readonly ClientApplicationRepository _repositoryApplication;
    private readonly IMapper _mapper;


    public FunctionGroupGetAllHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(FunctionGroupGetAllHandler)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new FunctionGroupRepository(TestsSingleton.Auth, context);
        var service = new FunctionGroupService(_repository, Mock.Of<ILogger<FunctionGroupService>>());

        _repositoryApplication = new ClientApplicationRepository(TestsSingleton.Auth, context);
        var serviceApplication = new ClientApplicationService(_repositoryApplication, Mock.Of<ILogger<ClientApplicationService>>());

        _handler = new FunctionGroupGetAllHandler(
            Mock.Of<ILogger<FunctionGroupGetAllHandler>>(),
            service,
            TestsSingleton.Mapper);

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new TestMappingProfile());
        });
        _mapper = mockMapper.CreateMapper();
    }
    
    [Fact]
    public async Task FunctionGroupGetAll_CallWithInvalidId_ReturnEmptyCollection()
    {
        // Arrange
        
        var name = "FunctionGroupGetAll_CallWithValidId_ThrowBusinessLogicException";
        
        var permissions = new List<Permission>();
        for (int i = 0; i < 3; i++)
        {
            permissions.Add(new Permission()
            {
                Name = "Permission " + i,
                Code = "Code " + i
            }); 
        }        

        var clientApplication = new ClientApplication() { Name = "Application" };
        var clientApplicationId = await _repository.CreateAsync(clientApplication);

        var functionGroup = new FunctionGroup()
        {
            Name = name,
            ClientApplicationId = clientApplicationId.Id,
            Permissions = permissions
        };
        await _repository.CreateAsync(functionGroup);

        // Act
        
        var query = new FunctionGroupGetAllQuery(-1);

        var result = await _handler.Handle(query);       

        // Assert
        Assert.Empty(result.Data);
    }
    
    [Fact]
    public async Task FunctionGroupGetById_CallWithValidIdButNotPresentId_ReturnEmpty()
    {
        // Arrange

        var name = "FunctionGroupGetById_CallWithValidIdButNotPresentId_ReturnNULL";

        var permissions = new List<Permission>();
        for (int i = 0; i < 3; i++)
        {
            permissions.Add(new Permission()
            {
                Name = "Permission " + i,
                Code = "Code " + i
            });
        }

        var clientApplication = new ClientApplication() { Name = "Application" };
        var clientApplicationId = await _repository.CreateAsync(clientApplication);

        var functionGroup = new FunctionGroup()
        {
            Name = name,
            ClientApplicationId = clientApplicationId.Id,
            Permissions = permissions
        };
        await _repository.CreateAsync(functionGroup);

        // Act
        var query = new FunctionGroupGetAllQuery(1000);
        var result = await _handler.Handle(query);

        // Assert
        Assert.Empty(result.Data);
    }
    
    [Fact]
    public async Task FunctionGroupGetById_CallWithValidId_ReturnObjectWithSameID()
    {
        // Arrange       

        var name = "FunctionGroupGetById_CallWithValidId_ReturnObjectWithSameID";

        var permissions = new List<Permission>();
        for (int i = 0; i < 3; i++)
        {
            permissions.Add(new Permission()
            {
                Name = "Permission " + i,
                Code = "Code " + i
            });
        }

        var clientApplication = new ClientApplication() { Name = "Application" };
        var clientApplicationId = await _repository.CreateAsync(clientApplication);

        var functionGroup = new FunctionGroup()
        {
            Name = name,
            ClientApplicationId = clientApplicationId.Id,
            Permissions = permissions
        };
        var id = await _repository.CreateAsync(functionGroup);

        // Act
        var query = new FunctionGroupGetAllQuery(id);
        var result = await _handler.Handle(query);

        // Assert
        Assert.NotEmpty(result.Data);        
    }
}