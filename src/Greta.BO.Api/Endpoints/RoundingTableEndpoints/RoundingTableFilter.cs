using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.RoundingTableQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.RoundingTableEndpoints;

[Route("api/RoundingTable")]
public class RoundingTableFilter: EndpointBaseAsync.WithRequest<RoundingTableFilterRequest>.WithActionResult<RoundingTableFilterResponse>
{
    private readonly IMediator _mediator;

    public RoundingTableFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of rounding table entity",
        Description = "Gets a paginated list of rounding table entity",
        OperationId = "RoundingTable.Filter",
        Tags = new[] { "RoundingTable" })
    ]
    [ProducesResponseType(typeof(RoundingTableFilterResponse), 200)]
    public override async Task<ActionResult<RoundingTableFilterResponse>> HandleAsync(
        [FromMultiSource]RoundingTableFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new RoundingTableFilterQuery(request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}