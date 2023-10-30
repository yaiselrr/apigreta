using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Scalendar;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ScalendarEndpoints;

[Route("api/Scalendar")]
public class ScalendarFilter: EndpointBaseAsync.WithRequest<ScalendarFilterRequest>.WithActionResult<ScalendarFilterResponse>
{
    private readonly IMediator _mediator;

    public ScalendarFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of the Scalendar entity",
        Description = "Gets a paginated list of the Scalendar entity",
        OperationId = "Scalendar.Filter",
        Tags = new[] { "Scalendar" })
    ]
    [ProducesResponseType(typeof(ScalendarFilterResponse), 200)]
    public override async Task<ActionResult<ScalendarFilterResponse>> HandleAsync(
        [FromMultiSource]ScalendarFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new ScalendarFilterQuery(request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}