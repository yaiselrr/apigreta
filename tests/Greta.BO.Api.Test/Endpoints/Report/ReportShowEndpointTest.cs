using Greta.BO.Api.Endpoints.ReportEndpoints;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.Api.Test.Helpers;
using Greta.BO.BusinessLogic.Handlers.DataHandlers.Validations.Extensions;
using Greta.BO.BusinessLogic.Handlers.Queries.Report;
using Greta.BO.BusinessLogic.Service;
using Greta.Report.SPA.EventContracts.Corporate.Report;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using IMediator = MediatR.IMediator;

namespace Greta.BO.Api.Test.Endpoints.Report;

public class ReportShowEndpointTest
{
    readonly ReportRepository _repository;
    readonly ReportService _service;
    readonly ReportShowReport _endpoint;

    readonly Mock<IMediator> _mediatorMock;


    public ReportShowEndpointTest()
    {
        var builder = new DbContextOptionsBuilder<SqlServerContext>();
        var options = builder.UseInMemoryDatabase(nameof(ReportShowEndpointTest)).Options;
        var context = new SqlServerContext(options, TestsSingleton.Auth);

        _repository = new ReportRepository(TestsSingleton.Auth, context);
        _service = new ReportService(_repository, Mock.Of<ILogger<ReportService>>(), Mock.Of<IRequestClient<FilterReportRequestContract>>());

        _mediatorMock = new Mock<IMediator>();
        _endpoint = new ReportShowReport(_mediatorMock.Object, Mock.Of<IConfiguration>());
    }

    [Fact]
    public async Task ShowReport_WithSameId_ResultRedirectResult()
    {
        // Arrange

        var report = new Entities.Report() { Name = "Report", Category = Entities.Enum.ReportCategory.SALES, State = true, GuidId = Guid.NewGuid() };

        var rep = await _repository.CreateAsync(report);

        string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjA2OGNiNzRhLWVjOTgtNDA5Yi04M2U5LWIxMGJhZWY0YjU0OCIsInVzZXJlbWFpbCI6ImNoZW5yeWhhYmFuYTIwNSttYW5hZ2VydGVzdGN1YmFAZ21haWwuY29tIiwidXNlciI6Im1hbmFnZXJkZXYiLCJ1c2Vycm9sZSI6ImRlZmluZVJvbGUiLCJzY29wZSI6InRlc3RjdWJhIiwibmJmIjoxNjkwODI3NjMzLCJleHAiOjE2OTA5MTQwMzMsImlzcyI6Imh0dHBzOi8vaWRlbnRpdHkuZ3JldGF0ZXN0LmNvbSIsImF1ZCI6Imh0dHBzOi8vaWRlbnRpdHkuZ3JldGF0ZXN0LmNvbSJ9.zjc_Rgeml3eSpet4GLtHZHuXYhq1GBd4eNPi23ZymW8";

        string parameters = "";

        var request = new ReportShowReportRequest() { Id = (int)rep, Parameters = parameters, Token = token };

        var handler = new ReportGetByIdHandler(_service, TestsSingleton.Mapper);

        var query = new ReportGetByIdQuery((int)rep);

        var cancellationToken = new CancellationToken();

        _mediatorMock.Setup(x => x.Send(It.IsAny<ReportGetByIdQuery>(), cancellationToken)).Returns(handler.Handle(query, cancellationToken));

        //Act

        var result = await _endpoint.HandleAsync(request, cancellationToken);

        // Assert

        _mediatorMock.Verify(x => x.Send(It.IsAny<ReportGetByIdQuery>(), cancellationToken), Times.Once);

        Assert.IsType<ActionResult<ReportGetByIdResponse>>(result);
        Assert.IsType<RedirectResult>(result.Result);
    }

    [Fact]
    public async Task ShowReport_WithNotFoundId_ResultBadRequest()
    {
        // Arrange

        var report = new Entities.Report() { Name = "Report", Category = Entities.Enum.ReportCategory.SALES, State = true, GuidId = Guid.NewGuid() };

        await _repository.CreateAsync(report);

        string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjA2OGNiNzRhLWVjOTgtNDA5Yi04M2U5LWIxMGJhZWY0YjU0OCIsInVzZXJlbWFpbCI6ImNoZW5yeWhhYmFuYTIwNSttYW5hZ2VydGVzdGN1YmFAZ21haWwuY29tIiwidXNlciI6Im1hbmFnZXJkZXYiLCJ1c2Vycm9sZSI6ImRlZmluZVJvbGUiLCJzY29wZSI6InRlc3RjdWJhIiwibmJmIjoxNjkwODI3NjMzLCJleHAiOjE2OTA5MTQwMzMsImlzcyI6Imh0dHBzOi8vaWRlbnRpdHkuZ3JldGF0ZXN0LmNvbSIsImF1ZCI6Imh0dHBzOi8vaWRlbnRpdHkuZ3JldGF0ZXN0LmNvbSJ9.zjc_Rgeml3eSpet4GLtHZHuXYhq1GBd4eNPi23ZymW8";

        string parameters = "";

        var request = new ReportShowReportRequest() { Id = 8, Parameters = parameters, Token = token };

        var handler = new ReportGetByIdHandler(_service, TestsSingleton.Mapper);

        var query = new ReportGetByIdQuery(8);

        var cancellationToken = new CancellationToken();

        _mediatorMock.Setup(x => x.Send(It.IsAny<ReportGetByIdQuery>(), cancellationToken)).Returns(handler.Handle(query, cancellationToken));

        //Act

        var result = await _endpoint.HandleAsync(request, cancellationToken);

        // Assert

        _mediatorMock.Verify(x => x.Send(It.IsAny<ReportGetByIdQuery>(), cancellationToken), Times.Once);

        Assert.NotNull(result.Result);
        Assert.IsType<BadRequestResult>(result.Result);
        Assert.Equal(StatusCodes.Status400BadRequest, result.Result.GetPropValue("StatusCode"));
    }
}