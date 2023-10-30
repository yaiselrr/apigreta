using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Report;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ReportEndpoints;

[Route("api/Report")]
public class ReportShowReport : EndpointBaseAsync.WithRequest<ReportShowReportRequest>.WithActionResult<ReportGetByIdResponse>
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;

    public ReportShowReport(IMediator mediator, IConfiguration configuration)
    {
        _mediator = mediator;
        _configuration = configuration;
    }
        
    [HttpGet("ShowReport/{reportId}/{parameters}")]
    [SwaggerOperation(
        Summary = "Show Report File by Id",
        Description = "Show Report File by Id",
        OperationId = "Report.ShowById",        
        Tags = new[] { "Report" })
    ]
    [ProducesResponseType(308)]
    [ProducesResponseType(404)]
    public override async Task<ActionResult<ReportGetByIdResponse>> HandleAsync(
        [FromMultiSource] ReportShowReportRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var report = (await _mediator.Send(new ReportGetByIdQuery(request.Id), cancellationToken)).Data;

            var url =
                    $@"{_configuration["Enterprise:ReportUrl"]}/Home/Show?&clientCode={_configuration["Company:CompanyCode"]}&ReportGuidId={report?.GuidId}&requestFromCorporate=true&token={request.Token}" +
                    (string.IsNullOrEmpty(request.Parameters) ? "" : $"&parameters={request.Parameters}");
            return RedirectPreserveMethod(url);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }
}