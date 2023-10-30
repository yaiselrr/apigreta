using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.PriceBatchDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.PriceBatchDetailEndpoints;
[Route("api/PriceBatchDetail")]
public class PriceBatchDetailChangeState : EndpointBaseAsync.WithRequest<PriceBatchDetailChangeStateRequest>.WithActionResult<PriceBatchDetailChangeStateResponse>
{
    private readonly IMediator _mediator;

    public PriceBatchDetailChangeState(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ChangeState/{entityId}/{state}")]
    [SwaggerOperation(
        Summary = "Change the state of the price batch detail entity",
        Description = "Change the state of the price batch detail entity",
        OperationId = "PriceBatchDetail.ChangeState",
        Tags = new[] { "PriceBatchDetail" })
    ]
    [ProducesResponseType(typeof(PriceBatchDetailChangeStateResponse), 200)]
    public override async Task<ActionResult<PriceBatchDetailChangeStateResponse>> HandleAsync(
        [FromRoute] PriceBatchDetailChangeStateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Id < 1)
            return NotFound();
        return await _mediator.Send(new PriceBatchDetailChangeStateCommand(request.Id, request.State), cancellationToken);
    }
}