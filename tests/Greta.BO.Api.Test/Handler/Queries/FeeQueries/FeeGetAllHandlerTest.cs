
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Handlers.Queries.Fee;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Queries.FeeQueries;


public class FeeGetAllHandlerTest
{
    private readonly FeeRepository _repository;
    private readonly FeeGetAllHandler _handler;

    public FeeGetAllHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(FeeGetAllHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new FeeRepository(TestsSingleton.Auth, context);
        var service = new FeeService(_repository, Mock.Of<ILogger<FeeService>>());

        _handler = new FeeGetAllHandler(
            service,
            TestsSingleton.Mapper);
    }

    [Fact]
    public async Task GetAll_NormalCall_ResultOk()
    {
        // Arrange
        var fees = new List<Fee>();
        for (var i = 0; i < 3; i++)
        {
            fees.Add(new Fee()
            {
                Name = "Fee Name " + i,
                State = true
            });
        }

        await _repository.CreateRangeAsync(fees);
                
        var query = new FeeGetAllQuery();
        // Act
        var result = await _handler.Handle(query);

        // Assert
        Assert.Equal(fees.Count, result.Data.Count);
    }
       

    [Fact]
    public async Task GetAllSpec_CallNormal_ResultEmpty()
    {
        // Arrange
               
        // Act
               
        var query = new FeeGetAllQuery();

        var result = await _handler.Handle(query);        

        // Assert

        Assert.Empty(result.Data);
    }
}