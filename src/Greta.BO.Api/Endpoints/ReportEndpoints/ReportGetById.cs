using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Report;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ReportEndpoints;

[Route("api/Report")]
public class ReportGetById : EndpointBaseAsync.WithRequest<ReportGetByIdRequest>.WithActionResult<ReportGetByIdResponse>
{
    private readonly IMediator _mediator;

    public ReportGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{reportId}")]
    [SwaggerOperation(
        Summary = "Get Report by id",
        Description = "Get Report by id",
        OperationId = "Report.GetById",
        Tags = new[] { "Report" })
    ]
    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(ReportGetByIdResponse), 200)]
    public override async Task<ActionResult<ReportGetByIdResponse>> HandleAsync(
        [FromMultiSource] ReportGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new ReportGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}