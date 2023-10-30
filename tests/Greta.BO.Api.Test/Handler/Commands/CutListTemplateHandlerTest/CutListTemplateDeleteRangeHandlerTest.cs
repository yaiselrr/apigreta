using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Command.CutListTemplate;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Commands.CutListTemplateCommands;


public class CutListTemplateDeleteRangeHandlerTest
{
    private readonly CutListTemplateRepository _repository;
    private readonly CutListTemplateDeleteRangeHandler _handler;

    public CutListTemplateDeleteRangeHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(CutListTemplateDeleteRangeHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new CutListTemplateRepository(TestsSingleton.Auth, context);
        var service = new CutListTemplateService(_repository, Mock.Of<ILogger<CutListTemplateService>>());

        _handler = new CutListTemplateDeleteRangeHandler(
            Mock.Of<ILogger<CutListTemplateDeleteRangeHandler>>(),
            service);       
    }  

    [Fact]
    public async Task DeleteRange_WithValidIdRange_ReturnResultTrue()
    {
        // Arrange        

        List<CutListTemplate> cutListTemplate = new List<CutListTemplate>();

        for (int i = 1; i <= 3; i++)
        {
            cutListTemplate.Add(new CutListTemplate() { Name = "CutListTemplate" + i });
        }
        
        await _repository.CreateRangeAsync(cutListTemplate);

        List<long> isd = new List<long>(cutListTemplate.Select(x=>x.Id));

        // Act

        var command = new CutListTemplateDeleteRangeCommand(isd);

        var result = await _handler.Handle(command);       

        // Assert

        Assert.True(result.Data);
    }

    [Fact]
    public async Task DeleteRange_WithNotExistIdInRange_ReturnBusinesLogicException()
    {
        // Arrange       

        List<CutListTemplate> cutListTemplate = new List<CutListTemplate>();

        for (int i = 1; i <= 3; i++)
        {
            cutListTemplate.Add(new CutListTemplate() { Name = "CutListTemplate" + i });
        }

        await _repository.CreateRangeAsync(cutListTemplate);

        List<long> ids = new List<long> { 100, 101, 102 };
        // Act

        var command = new CutListTemplateDeleteRangeCommand(ids);

        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            await _handler.Handle(command);
        });

        // Assert

        Assert.Equal("List of ids is null or empty", exception.Message);
    }

    [Fact]
    public async Task DeleteRange_WithInvalidId_ReturnBusinesLogicException()
    {
        // Arrange       

        List<CutListTemplate> cutListTemplate = new List<CutListTemplate>();

        for (int i = 1; i <= 3; i++)
        {
            cutListTemplate.Add(new CutListTemplate() { Name = "CutListTemplate" + i });
        }

        await _repository.CreateRangeAsync(cutListTemplate);

        List<long> ids = new List<long> { -1, -2, -3 };

        // Act
        
        var command = new CutListTemplateDeleteRangeCommand(ids);

        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            await _handler.Handle(command);
        });
               
        // Assert
                
        Assert.Equal("List of ids is null or empty", exception.Message);
    }
}