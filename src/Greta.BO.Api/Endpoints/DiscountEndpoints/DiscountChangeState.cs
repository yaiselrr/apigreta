using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.Discount;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.DiscountEndpoints;
[Route("api/Discount")]
public class DiscountChangeState : EndpointBaseAsync.WithRequest<DiscountChangeStateRequest>.WithActionResult<DiscountChangeStateResponse>
{
    private readonly IMediator _mediator;

    public DiscountChangeState(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ChangeState/{entityId}/{state}")]
    [SwaggerOperation(
        Summary = "Change the state of the Discount entity",
        Description = "Change the state of the Discount entity",
        OperationId = "Discount.ChangeState",
        Tags = new[] { "Discount" })
    ]
    [ProducesResponseType(typeof(DiscountChangeStateResponse), 200)]
    public override async Task<ActionResult<DiscountChangeStateResponse>> HandleAsync(
        [FromRoute] DiscountChangeStateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Id < 1)
            return NotFound();
        return await _mediator.Send(new DiscountChangeStateCommand(request.Id, request.State), cancellationToken);
    }
}