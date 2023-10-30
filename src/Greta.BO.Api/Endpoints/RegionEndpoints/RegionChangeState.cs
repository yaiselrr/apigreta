using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.Region;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.RegionEndpoints;
[Route("api/Region")]
public class RegionChangeState : EndpointBaseAsync.WithRequest<RegionChangeStateRequest>.WithActionResult<RegionChangeStateResponse>
{
    private readonly IMediator _mediator;

    public RegionChangeState(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ChangeState/{entityId}/{state}")]
    [SwaggerOperation(
        Summary = "Change the state of the region entity",
        Description = "Change the state of the region entity",
        OperationId = "Region.ChangeState",
        Tags = new[] { "Region" })
    ]
    [ProducesResponseType(typeof(RegionChangeStateResponse), 200)]
    public override async Task<ActionResult<RegionChangeStateResponse>> HandleAsync(
        [FromRoute] RegionChangeStateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Id < 1)
            return NotFound();
        return await _mediator.Send(new RegionChangeStateCommand(request.Id, request.State), cancellationToken);
    }
}