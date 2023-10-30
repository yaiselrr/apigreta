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
public class ReportFilter: EndpointBaseAsync.WithRequest<ReportFilterRequest>.WithActionResult<ReportFilterResponse>
{
    private readonly IMediator _mediator;

    public ReportFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of report",
        Description = "Gets a paginated list of report",
        OperationId = "Report.Filter",
        Tags = new[] { "Report" })
    ]
    [ProducesResponseType(typeof(ReportFilterResponse), 200)]
    public override async Task<ActionResult<ReportFilterResponse>> HandleAsync(
        [FromMultiSource]ReportFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new ReportFilterQuery(request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}