using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.ExternalScale;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ExternalScaleEndpoints;

[Route("api/ExternalScale")]
public class ExternalScaleFilter: EndpointBaseAsync.WithRequest<ExternalScaleFilterRequest>.WithActionResult<ExternalScaleFilterResponse>
{
    private readonly IMediator _mediator;

    public ExternalScaleFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of the external scale entity",
        Description = "Gets a paginated list of the external scale entity",
        OperationId = "ExternalScale.Filter",
        Tags = new[] { "ExternalScale" })
    ]
    [ProducesResponseType(typeof(ExternalScaleFilterResponse), 200)]
    public override async Task<ActionResult<ExternalScaleFilterResponse>> HandleAsync(
        [FromMultiSource]ExternalScaleFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new ExternalScaleFilterQuery(request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}