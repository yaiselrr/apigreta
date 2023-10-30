
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Handlers.Queries.CutListTemplate;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Queries.CutListTemplateQueries;


public class CutListTemplateGetAllHandlerTest
{
    private readonly CutListTemplateRepository _repository;
    private readonly CutListTemplateGetAllHandler _handler;

    public CutListTemplateGetAllHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(CutListTemplateGetAllHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new CutListTemplateRepository(TestsSingleton.Auth, context);
        var service = new CutListTemplateService(_repository, Mock.Of<ILogger<CutListTemplateService>>());

        _handler = new CutListTemplateGetAllHandler(
            service,
            TestsSingleton.Mapper);
    }

    [Fact]
    public async Task GetAll_NormalCall_ResultOk()
    {
        // Arrange
        var cutListTemplates = new List<CutListTemplate>();
        for (var i = 0; i < 3; i++)
        {
            cutListTemplates.Add(new CutListTemplate()
            {
                Name = "CutListTemplate Name " + i,
                State = true
            });
        }

        await _repository.CreateRangeAsync(cutListTemplates);
                
        var query = new CutListTemplateGetAllQuery();
        // Act
        var result = await _handler.Handle(query);

        // Assert
        Assert.Equal(cutListTemplates.Count, result.Data.Count);
    }
}