using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.Profile;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ProfilesEndpoints;
[Route("api/Profiles")]
public class ProfilesChangeState : EndpointBaseAsync.WithRequest<ProfilesChangeStateRequest>.WithActionResult<ProfilesChangeStateResponse>
{
    private readonly IMediator _mediator;

    public ProfilesChangeState(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ChangeState/{entityId}/{state}")]
    [SwaggerOperation(
        Summary = "Change State of profile",
        Description = "Change State of profile",
        OperationId = "Profiles.ChangeState",
        Tags = new[] { "Profiles" })
    ]
    [ProducesResponseType(typeof(ProfilesChangeStateResponse), 200)]
    public override async Task<ActionResult<ProfilesChangeStateResponse>> HandleAsync(
        [FromRoute] ProfilesChangeStateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Id < 1)
            return NotFound();
        return await _mediator.Send(new ProfilesChangeStateCommand(request.Id, request.State), cancellationToken);
    }
}