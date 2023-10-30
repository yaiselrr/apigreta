using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.Report;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ReportEndpoints;
[Route("api/Report")]
public class ReportGet: EndpointBaseAsync.WithoutRequest.WithActionResult<ReportGetAllResponse>
{
    private readonly IMediator _mediator;

    public ReportGet(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get all Report",
        Description = "Get all Report",
        OperationId = "Report.Get",
        Tags = new[] { "Report" })
    ]
    [ProducesResponseType(typeof(ReportGetAllResponse), 200)]
    public override async Task<ActionResult<ReportGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new ReportGetAllQuery(), cancellationToken));
    }
}