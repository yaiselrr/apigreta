using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.LoyaltyDiscount;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace Greta.BO.Api.Endpoints.LoyaltyDiscountEndpoints;

[Route("api/LoyaltyDiscount")]
public class LoyaltyDiscountChangeState:EndpointBaseAsync.WithRequest<LoyaltyDiscountChangeStateRequest>.WithActionResult<LoyaltyDiscountChangeStateResponse>
{
    private readonly IMediator _mediator;

    public LoyaltyDiscountChangeState(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ChangeState/{entityId}/{state}")]
    [SwaggerOperation(
        Summary = "Change State of LoyaltyDiscount",
        Description = "Change State of LoyaltyDiscount",
        OperationId = "LoyaltyDiscount.ChangeState",
        Tags = new[] { "LoyaltyDiscount" })
    ]
    [ProducesResponseType(typeof(LoyaltyDiscountChangeStateResponse), 200)]
    public override async Task<ActionResult<LoyaltyDiscountChangeStateResponse>> HandleAsync(
        [FromRoute] LoyaltyDiscountChangeStateRequest request,
        CancellationToken cancellationToken = default)
    {        
        return Ok(await _mediator.Send(new LoyaltyDiscountChangeStateCommand(request.Id, request.State), cancellationToken));
    }
}
