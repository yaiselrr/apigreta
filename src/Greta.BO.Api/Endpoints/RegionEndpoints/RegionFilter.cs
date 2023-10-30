using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Region;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.RegionEndpoints;

[Route("api/Region")]
public class RegionFilter: EndpointBaseAsync.WithRequest<RegionFilterRequest>.WithActionResult<RegionFilterResponse>
{
    private readonly IMediator _mediator;

    public RegionFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of the region entity",
        Description = "Gets a paginated list of the region entity",
        OperationId = "Region.Filter",
        Tags = new[] { "Region" })
    ]
    [ProducesResponseType(typeof(RegionFilterResponse), 200)]
    public override async Task<ActionResult<RegionFilterResponse>> HandleAsync(
        [FromMultiSource]RegionFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new RegionFilterQuery(request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}