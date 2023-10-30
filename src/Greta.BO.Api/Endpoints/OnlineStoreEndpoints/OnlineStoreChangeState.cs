using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.OnlineStore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.OnlineStoreEndpoints;
[Route("api/OnlineStore")]
public class OnlineStoreChangeState : EndpointBaseAsync.WithRequest<OnlineStoreChangeStateRequest>.WithActionResult<OnlineStoreChangeStateResponse>
{
    private readonly IMediator _mediator;

    public OnlineStoreChangeState(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ChangeState/{entityId}/{state}")]
    [SwaggerOperation(
        Summary = "Change the state of the OnlineStore entity",
        Description = "Change the state of the OnlineStore entity",
        OperationId = "OnlineStore.ChangeState",
        Tags = new[] { "OnlineStore" })
    ]
    [ProducesResponseType(typeof(OnlineStoreChangeStateResponse), 200)]
    public override async Task<ActionResult<OnlineStoreChangeStateResponse>> HandleAsync(
        [FromRoute] OnlineStoreChangeStateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Id < 1)
            return NotFound();
        return await _mediator.Send(new OnlineStoreChangeStateCommand(request.Id, request.State), cancellationToken);
    }
}