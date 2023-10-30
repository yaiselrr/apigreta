using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Handlers.Queries.Report;
using Greta.BO.BusinessLogic.Service;
using Greta.Report.SPA.EventContracts.Corporate.Report;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Greta.BO.Api.Test.Handler.Queries.ReportQueries;

public class ReportGetAllHandlerTest
{
    private readonly ReportRepository _repository;
    private readonly ReportGetAllHandler _handler;
    
    public ReportGetAllHandlerTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(ReportGetAllHandlerTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new ReportRepository(TestsSingleton.Auth, context);
        var service = new ReportService(_repository, Mock.Of<ILogger<ReportService>>(), Mock.Of<IRequestClient<FilterReportRequestContract>>());

        _handler = new ReportGetAllHandler(
            service,
            TestsSingleton.Mapper);        
    }
    
    [Fact]
    public async Task ReportGetAll_ReturnsReportGetAllResponseWithData()
    {
        // Arrange
        
        var name = "ReportGetAll_ReturnsReportGetAllResponseWithData";
        var report = new Entities.Report()
        {
            Name = name
        };
        await _repository.CreateAsync(report);
        // Act
       
        var query = new ReportGetAllQuery();

        var result = await _handler.Handle(query);
       
        // Assert
        Assert.NotNull(result);        
        Assert.NotEmpty(result.Data);
    }
    
    [Fact]
    public async Task ReportGetAll_ReturnsReportGetAllResponseWithOutData()
    {
        // Arrange
                
        // Act       
        
        var query = new ReportGetAllQuery();
        var result = await _handler.Handle(query);        

        // Assert        
        Assert.Empty(result.Data);
    }
}