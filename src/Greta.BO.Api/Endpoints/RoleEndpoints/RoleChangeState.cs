using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.Role;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.RoleEndpoints;
[Route("api/Role")]
public class RoleChangeState : EndpointBaseAsync.WithRequest<RoleChangeStateRequest>.WithActionResult<RoleChangeStateResponse>
{
    private readonly IMediator _mediator;

    public RoleChangeState(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ChangeState/{entityId}/{state}")]
    [SwaggerOperation(
        Summary = "Change State of entity",
        Description = "Change State of entity",
        OperationId = "Role.ChangeState",
        Tags = new[] { "Role" })
    ]
    [ProducesResponseType(typeof(RoleChangeStateResponse), 200)]
    public override async Task<ActionResult<RoleChangeStateResponse>> HandleAsync(
        [FromRoute] RoleChangeStateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Id < 1)
            return NotFound();
        return await _mediator.Send(new RoleChangeStateCommand(request.Id, request.State), cancellationToken);
    }
}