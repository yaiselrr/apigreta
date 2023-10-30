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


public class CutListTemplateChangeStateHandlerTest
{
    private readonly CutListTemplateRepository _repository;
    private readonly CutListTemplateChangeStateHandler _handler;    

    public CutListTemplateChangeStateHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(CutListTemplateChangeStateHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new CutListTemplateRepository(TestsSingleton.Auth, context);
        var service = new CutListTemplateService(_repository, Mock.Of<ILogger<CutListTemplateService>>());

        _handler = new CutListTemplateChangeStateHandler(
            Mock.Of<ILogger<CutListTemplateChangeStateHandler>>(),
            service);        
    }

    [Fact]
    public async Task PutChangeState_NormalCall_ResultTrue()
    {
        // Arrange

        var cutListTemplate = new CutListTemplate()
        {
            Name = "PutChangeState_NormalCall_ResultTrue",  
        };             
       
        var id = await _repository.CreateAsync(cutListTemplate);
        
        var command = new CutListTemplateChangeStateCommand(id , !cutListTemplate.State);

        // Act
        var result = await _handler.Handle(command);

        // Assert
       
        Assert.True(result.Data);
    }

    [Fact]
    public async Task PutChangeState_WithIdNotExist_ResultFalse()
    {
        // Arrange

        var cutListTemplate = new CutListTemplate()
        {
            Name = "PutChangeState_WithIdNotExist_ResultFalse",            
        };

        await _repository.CreateAsync(cutListTemplate);        

        var command = new CutListTemplateChangeStateCommand(5, !cutListTemplate.State);

        // Act
        var result = await _handler.Handle(command);

        // Assert

        Assert.False(result.Data);
    }

    [Fact]
    public async Task PutChangeState_WithIdLessThanCiro_ResultBusinessLogicException()
    {
        // Arrange

        var cutListTemplate = new CutListTemplate()
        {
            Name = "PutChangeState_WithIdLessThanCiro_ResultBusinessLogicException"
        };

        await _repository.CreateAsync(cutListTemplate);        

        // Act

        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var command = new CutListTemplateChangeStateCommand(-1, !cutListTemplate.State);

            var result = await _handler.Handle(command);
        });

        // Assert

        Assert.Equal("Id parameter out of bounds", exception.Message);
    }
}