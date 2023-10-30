using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.MixAndMatch;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace Greta.BO.Api.Endpoints.MixAndMatchEndpoints;

[Route("api/MixAndMatch")]
public class MixAndMatchChangeState:EndpointBaseAsync.WithRequest<MixAndMatchChangeStateRequest>.WithActionResult<MixAndMatchChangeStateResponse>
{
    private readonly IMediator _mediator;

    public MixAndMatchChangeState(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ChangeState/{entityId}/{state}")]
    [SwaggerOperation(
        Summary = "Change State of MixAndMatch",
        Description = "Change State of MixAndMatch",
        OperationId = "MixAndMatch.ChangeState",
        Tags = new[] { "MixAndMatch" })
    ]
    [ProducesResponseType(typeof(MixAndMatchChangeStateResponse), 200)]
    public override async Task<ActionResult<MixAndMatchChangeStateResponse>> HandleAsync(
        [FromRoute] MixAndMatchChangeStateRequest request,
        CancellationToken cancellationToken = default)
    {        
        return Ok(await _mediator.Send(new MixAndMatchChangeStateCommand(request.Id, request.State), cancellationToken));
    }
}
