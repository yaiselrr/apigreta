using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.Region;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.RegionEndpoints;
[Route("api/Region")]
public class RegionGet: EndpointBaseAsync.WithoutRequest.WithActionResult<RegionGetAllResponse>
{
    private readonly IMediator _mediator;

    public RegionGet(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get all region entities",
        Description = "Get all region entities",
        OperationId = "Region.Get",
        Tags = new[] { "Region" })
    ]
    [ProducesResponseType(typeof(RegionGetAllResponse), 200)]
    public override async Task<ActionResult<RegionGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new RegionGetAllQuery(), cancellationToken));
    }
}