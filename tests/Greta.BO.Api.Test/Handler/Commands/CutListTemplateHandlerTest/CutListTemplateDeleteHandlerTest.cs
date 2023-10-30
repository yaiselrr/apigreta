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


public class CutListTemplateDeleteHandlerTest
{
    private readonly CutListTemplateRepository _repository;
    private readonly CutListTemplateDeleteHandler _handler;    

    public CutListTemplateDeleteHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(CutListTemplateDeleteHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new CutListTemplateRepository(TestsSingleton.Auth, context);
        var service = new CutListTemplateService(_repository, Mock.Of<ILogger<CutListTemplateService>>());

        _handler = new CutListTemplateDeleteHandler(
            Mock.Of<ILogger<CutListTemplateDeleteHandler>>(),
            service);       
    }

    [Fact]
    public async Task Delete_WithSameId_ReturnResultTrue()
    {
        // Arrange

        var cutListTemplate = new CutListTemplate()
        {
            Name = "CutListTemplate Name 1"
        };             
       
        var id = await _repository.CreateAsync(cutListTemplate);

        var command = new CutListTemplateDeleteCommand(id);

        // Act
        var result = await _handler.Handle(command);

        // Assert
               
        Assert.True(result.Data);
    }

    [Fact]
    public async Task Delete_WithNoExistId_ReturnResultFalse()
    {
        // Arrange

        var cutListTemplate = new CutListTemplate()
        {
            Name = "CutListTemplate Name "
        };

        await _repository.CreateAsync(cutListTemplate);

        var command = new CutListTemplateDeleteCommand(10);

        // Act
        var result = await _handler.Handle(command);

        // Assert
                
        Assert.False(result.Data);
    }

    [Fact]
    public async Task Delete_WithIdLessThanCiro_ResultBusinessLogicException()
    {
        // Arrange

        var cutListTemplate = new CutListTemplate()
        {
            Name = "CutListTemplate Name "
        };

        await _repository.CreateAsync(cutListTemplate);
        
        // Act

        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var command = new CutListTemplateDeleteCommand(-2);

            var result = await _handler.Handle(command);
        });        

        // Assert
                
        Assert.Equal("Id parameter out of bounds", exception.Message);
    }
}