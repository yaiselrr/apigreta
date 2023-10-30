using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.LoyaltyDiscount;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.LoyaltyDiscountEndpoints;
[Route("api/LoyaltyDiscount")]
public class LoyaltyDiscountDelete : EndpointBaseAsync.WithRequest<LoyaltyDiscountDeleteRequest>.WithActionResult<LoyaltyDiscountDeleteResponse>
{
    private readonly IMediator _mediator;

    public LoyaltyDiscountDelete(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete("{entityId}")]
    [SwaggerOperation(
        Summary = "Delete a LoyaltyDiscount by Id",
        Description = "Delete a LoyaltyDiscount by Id",
        OperationId = "LoyaltyDiscount.Delete",
        Tags = new[] { "LoyaltyDiscount" })
    ]
    [ProducesResponseType(typeof(LoyaltyDiscountDeleteResponse), 200)]
    public override async Task<ActionResult<LoyaltyDiscountDeleteResponse>> HandleAsync(
        [FromMultiSource] LoyaltyDiscountDeleteRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new LoyaltyDiscountDeleteCommand(request.Id), cancellationToken);
    }
}