using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.Fee;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.FeeEndpoints;
[Route("api/Fee")]
public class FeeChangeState : EndpointBaseAsync.WithRequest<FeeChangeStateRequest>.WithActionResult<FeeChangeStateResponse>
{
    private readonly IMediator _mediator;

    public FeeChangeState(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ChangeState/{entityId}/{state}")]
    [SwaggerOperation(
        Summary = "Change State of Fee",
        Description = "Change State of Fee",
        OperationId = "Fee.ChangeState",
        Tags = new[] { "Fee" })
    ]
    [ProducesResponseType(typeof(FeeChangeStateResponse), 200)]
    public override async Task<ActionResult<FeeChangeStateResponse>> HandleAsync(
        [FromRoute] FeeChangeStateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Id < 1)
            return NotFound();
        return await _mediator.Send(new FeeChangeStateCommand(request.Id, request.State), cancellationToken);
    }
}