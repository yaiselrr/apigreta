
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Queries.CutListTemplate;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Queries.CutListTemplateQueries;


public class CutListTemplateFilterHandlerTest
{
    private readonly CutListTemplateRepository _repository;
    private readonly CutListTemplateFilterHandler _handler;

    public CutListTemplateFilterHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(CutListTemplateFilterHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new CutListTemplateRepository(TestsSingleton.Auth, context);
        var service = new CutListTemplateService(_repository, Mock.Of<ILogger<CutListTemplateService>>());

        _handler = new CutListTemplateFilterHandler(           
            service,
            TestsSingleton.Mapper);
    }

    [Fact]
    public async Task FilterSpec_NormalCall_ResultOk()
    {
        // Arrange
        var cutListTemplates = new List<CutListTemplate>();
        for (var i = 0; i < 3; i++)
        {
            cutListTemplates.Add(new CutListTemplate()
            {
                Name = "CutListTemplate Name " + i
            });
        }

        await _repository.CreateRangeAsync(cutListTemplates);
        var filter = new CutListTemplateSearchModel();
        var query = new CutListTemplateFilterQuery(1,3, filter);

        // Act
        var result = await _handler.Handle(query);

        // Assert
        Assert.Equal(3, result.Data.Data.Count);
    }

    [Fact]
    public async Task FilterSpec_CallWithNegativeValues_ResultBusinessObjectException()
    {
        // Arrange
        var cutListTemplates = new List<CutListTemplate>();
        for (var i = 0; i < 3; i++)
        {
            cutListTemplates.Add(new CutListTemplate()
            {
                Name = "CutListTemplate Name " + i
            });
        }

        await _repository.CreateRangeAsync(cutListTemplates);
        var filter = new CutListTemplateSearchModel();

        // Act

        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var query = new CutListTemplateFilterQuery(-1, 3, filter);

            await _handler.Handle(query);
        });

        // Assert
        
        Assert.IsType<BusinessLogicException>(exception);
    }

    [Fact]
    public async Task FilterSpec_CallWithNotFoundValue_ResultEmpty()
    {
        // Arrange
        var cutListTemplates = new List<CutListTemplate>();
        for (var i = 0; i < 3; i++)
        {
            cutListTemplates.Add(new CutListTemplate()
            {
                Name = "CutListTemplate Name " + i
            });
        }

        await _repository.CreateRangeAsync(cutListTemplates);
        var filter = new CutListTemplateSearchModel();

        // Act
               
        var query = new CutListTemplateFilterQuery(100, 3, filter);

        var result = await _handler.Handle(query);        

        // Assert

        Assert.Empty(result.Data.Data);
    }
}