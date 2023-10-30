using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.ShelfTag;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ShelfTagEndpoints;

[Route("api/ShelfTag")]
public class ShelfTagFilter: EndpointBaseAsync.WithRequest<ShelfTagFilterRequest>.WithActionResult<ShelfTagFilterResponse>
{
    private readonly IMediator _mediator;

    public ShelfTagFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of the ShelfTag entity",
        Description = "Gets a paginated list of the ShelfTag entity",
        OperationId = "ShelfTag.Filter",
        Tags = new[] { "ShelfTag" })
    ]
    [ProducesResponseType(typeof(ShelfTagFilterResponse), 200)]
    public override async Task<ActionResult<ShelfTagFilterResponse>> HandleAsync(
        [FromMultiSource]ShelfTagFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new ShelfTagFilterQuery(request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}