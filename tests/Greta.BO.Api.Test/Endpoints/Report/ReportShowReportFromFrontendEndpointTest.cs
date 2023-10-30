using Greta.BO.Api.Endpoints.ReportEndpoints;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Greta.BO.Api.Test.Endpoints.Report;

public class ReportShowReportFromFrontendEndpointTest
{
    readonly ReportRepository _repository;
    readonly ReportShowReportFromFrontEnd _endpoint;

    public ReportShowReportFromFrontendEndpointTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(ReportShowReportFromFrontendEndpointTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new ReportRepository(TestsSingleton.Auth, context);
        _endpoint = new ReportShowReportFromFrontEnd(Mock.Of<IConfiguration>());
    }

    [Fact]
    public async Task ShowReport_WithSameId_ResultRedirectResult()
    {
        // Arrange

        var report = new Entities.Report() { Name = "Report", Category = Entities.Enum.ReportCategory.SALES, State = true, GuidId = Guid.NewGuid() };

        await _repository.CreateAsync(report);

        string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjA2OGNiNzRhLWVjOTgtNDA5Yi04M2U5LWIxMGJhZWY0YjU0OCIsInVzZXJlbWFpbCI6ImNoZW5yeWhhYmFuYTIwNSttYW5hZ2VydGVzdGN1YmFAZ21haWwuY29tIiwidXNlciI6Im1hbmFnZXJkZXYiLCJ1c2Vycm9sZSI6ImRlZmluZVJvbGUiLCJzY29wZSI6InRlc3RjdWJhIiwibmJmIjoxNjkwODI3NjMzLCJleHAiOjE2OTA5MTQwMzMsImlzcyI6Imh0dHBzOi8vaWRlbnRpdHkuZ3JldGF0ZXN0LmNvbSIsImF1ZCI6Imh0dHBzOi8vaWRlbnRpdHkuZ3JldGF0ZXN0LmNvbSJ9.zjc_Rgeml3eSpet4GLtHZHuXYhq1GBd4eNPi23ZymW8";
        
        var request = new ReportShowReportFromFrontEndRequest() { ReportName = report.Name, ParameterLongId = 22, Token = token };

        var cancellationToken = new CancellationToken();
        //Act

        var result = await _endpoint.HandleAsync(request, cancellationToken);

        // Assert                       
               
        Assert.IsType<RedirectResult>(result);
    }    
}