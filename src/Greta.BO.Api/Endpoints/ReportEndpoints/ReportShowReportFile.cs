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
public class ReportShowReportFile : EndpointBaseAsync.WithRequest<ReportShowReportFileRequest>.WithActionResult<ReportGetByIdResponse>
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;

    public ReportShowReportFile(IMediator mediator, IConfiguration configuration)
    {
        _mediator = mediator;
        _configuration = configuration;
    }

    
    [HttpGet("ShowReportFile/{reportId}/{format}/{inline}/{parameters}")]
    [SwaggerOperation(
        Summary = "Show Report File by Id",
        Description = "Show Report File by Id",
        OperationId = "Report.ShowReportFile",        
        Tags = new[] { "Report" })
    ]
    [ProducesResponseType(308)]
    [ProducesResponseType(404)]
    public override async Task<ActionResult<ReportGetByIdResponse>> HandleAsync(
        [FromMultiSource] ReportShowReportFileRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var report = (await _mediator.Send(new ReportGetByIdQuery(request.Id), cancellationToken)).Data;

            var url = $@"{_configuration["Enterprise:ReportUrl"]}/api/Report/ShowReportFile?ReportGuidId={report?.GuidId}&clientCode={_configuration["Company:CompanyCode"]}&format={request.Format}&inline={request.Inline}&requestFromCorporate=true&token={request.Token}" +
                (string.IsNullOrEmpty(request.Parameters) ? "" : $"&parameters={request.Parameters}");
            return RedirectPermanentPreserveMethod(url);
        }
        catch (System.Exception)
        {

            return BadRequest();
        }
       
    }
}