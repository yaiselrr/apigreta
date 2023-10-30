using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Handlers.Command.Fee;
using Greta.BO.BusinessLogic.Service;
using Microsoft.Azure.Amqp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Commands.FeeCommands;


public class FeeDeleteRangeHandlerTest
{
    private readonly FeeRepository _repository;
    private readonly FeeDeleteRangeHandler _handler;

    public FeeDeleteRangeHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(FeeDeleteRangeHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new FeeRepository(TestsSingleton.Auth, context);
        var service = new FeeService(_repository, Mock.Of<ILogger<FeeService>>());

        _handler = new FeeDeleteRangeHandler(
            Mock.Of<ILogger<FeeDeleteRangeHandler>>(),
            service);       
    }  

    [Fact]
    public async Task DeleteRange_WithValidIdRange_ReturnResultTrue()
    {
        // Arrange        

        List<Fee> Feees = new List<Fee>();

        for (int i = 1; i <= 3; i++)
        {
            Feees.Add(new Fee() { Name = "Fee" + i });
        }
        
        await _repository.CreateRangeAsync(Feees);

        List<long> isd = new List<long>(Feees.Select(x=>x.Id));

        // Act

        var command = new FeeDeleteRangeCommand(isd);

        var result = await _handler.Handle(command);       

        // Assert

        Assert.True(result.Data);
    }

    [Fact]
    public async Task DeleteRange_WithNotExistIdInRange_ReturnResultFalse()
    {
        // Arrange       

        List<Fee> Feees = new List<Fee>();

        for (int i = 1; i <= 3; i++)
        {
            Feees.Add(new Fee() { Name = "Fee" + i });
        }

        await _repository.CreateRangeAsync(Feees);

        List<long> ids = new List<long> { 100, 101, 102 };
        // Act

        var command = new FeeDeleteRangeCommand(ids);

        var result = await _handler.Handle(command);

        // Assert                

        Assert.False(result.Data);
    }

    [Fact]
    public async Task DeleteRange_WithIdinvalidId_ReturnResultFalse()
    {
        // Arrange       

        List<Fee> Feees = new List<Fee>();

        for (int i = 1; i <= 3; i++)
        {
            Feees.Add(new Fee() { Name = "Fee" + i });
        }

        await _repository.CreateRangeAsync(Feees);

        List<long> ids = new List<long> { -1, -2, -3 };

        // Act
        
        var command = new FeeDeleteRangeCommand(ids);

        var result = await _handler.Handle(command);               

        // Assert
                
        Assert.False(result.Data);
    }
}