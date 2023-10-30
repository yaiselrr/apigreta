
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Queries.Family;
using Greta.BO.BusinessLogic.Handlers.Queries.Report;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.Report.SPA.EventContracts.Corporate.Report;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Queries.ReportQueries;


public class ReportFilterHandlerTest
{
    private readonly ReportRepository _repository;
    private readonly ReportFilterHandler _handler;

    public ReportFilterHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(ReportFilterHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new ReportRepository(TestsSingleton.Auth, context);
        var service = new ReportService(_repository, Mock.Of<ILogger<ReportService>>(), Mock.Of<IRequestClient<FilterReportRequestContract>>());

        _handler = new ReportFilterHandler(
            Mock.Of<ILogger<ReportFilterHandler>>(),
            service,
            TestsSingleton.Mapper);
    }

    [Fact]
    public async Task FilterSpec_NormalCall_ResultOk()
    {
        // Arrange
        var reports = new List<Api.Entities.Report>();
        for (var i = 0; i < 3; i++)
        {
            reports.Add(new Entities.Report()
            {
                Name = "Report Name " + i
            });
        }

        var id = await _repository.CreateRangeAsync(reports);

        var filter = new ReportSearchModel();
        var query = new ReportFilterQuery(1,10, filter);
        // Act
        var result = await _handler.Handle(query);

        // Assert
        Assert.NotEmpty(result.Data.Pages);
    }

    [Fact]
    public async Task FilterSpec_WithInvalidCurrentPage_ResultBusinessLogicException()
    {
        // Arrange
        var reports = new List<Api.Entities.Report>();
        for (var i = 0; i < 3; i++)
        {
            reports.Add(new Entities.Report()
            {
                Name = "Report Name " + i
            });
        }

        await _repository.CreateRangeAsync(reports);

        var filter = new ReportSearchModel();
        var query = new ReportFilterQuery(-1, 10, filter);

        // Act

        var exception = await Assert.ThrowsAsync<BusinessLogicException>(async () =>
        {
            var result = await _handler.Handle(query);
        });

        // Assert
        Assert.IsType<BusinessLogicException>(exception);
        Assert.Contains("Error filter reports.", exception.Message);
    }
}