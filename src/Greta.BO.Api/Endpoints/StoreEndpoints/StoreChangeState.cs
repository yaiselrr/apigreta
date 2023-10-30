using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.Store;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.StoreEndpoints;
[Route("api/Store")]
public class StoreChangeState : EndpointBaseAsync.WithRequest<StoreChangeStateRequest>.WithActionResult<StoreChangeStateResponse>
{
    private readonly IMediator _mediator;

    public StoreChangeState(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ChangeState/{entityId}/{state}")]
    [SwaggerOperation(
        Summary = "Change the state of the store entity",
        Description = "Change the state of the store entity",
        OperationId = "Store.ChangeState",
        Tags = new[] { "Store" })
    ]
    [ProducesResponseType(typeof(StoreChangeStateResponse), 200)]
    public override async Task<ActionResult<StoreChangeStateResponse>> HandleAsync(
        [FromRoute] StoreChangeStateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Id < 1)
            return NotFound();
        return await _mediator.Send(new StoreChangeStateCommand(request.Id, request.State), cancellationToken);
    }
}