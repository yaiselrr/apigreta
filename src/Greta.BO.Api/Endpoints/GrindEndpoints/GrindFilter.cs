using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Grind;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.GrindEndpoints;

[Route("api/Grind")]
public class GrindFilter: EndpointBaseAsync.WithRequest<GrindFilterRequest>.WithActionResult<GrindFilterResponse>
{
    private readonly IMediator _mediator;

    public GrindFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of the Grind entity",
        Description = "Gets a paginated list of the Grind entity",
        OperationId = "Grind.Filter",
        Tags = new[] { "Grind" })
    ]
    [ProducesResponseType(typeof(GrindFilterResponse), 200)]
    public override async Task<ActionResult<GrindFilterResponse>> HandleAsync(
        [FromMultiSource]GrindFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new GrindFilterQuery(request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}