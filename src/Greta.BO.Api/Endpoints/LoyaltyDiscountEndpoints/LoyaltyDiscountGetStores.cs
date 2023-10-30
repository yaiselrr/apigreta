using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.LoyaltyDiscount;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.LoyaltyDiscountEndpoints;
[Route("api/LoyaltyDiscount")]
public class LoyaltyDiscountGetStores : EndpointBaseAsync.WithoutRequest.WithActionResult<LoyaltyDiscountRemainStoresResponse>
{
    private readonly IMediator _mediator;

    public LoyaltyDiscountGetStores(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetStores")]
    [SwaggerOperation(
        Summary = "Get all Stores not assigned to any loyalty discount",
        Description = "Get all Stores not assigned to any loyalty discount",
        OperationId = "LoyaltyDiscount.Get",
        Tags = new[] { "LoyaltyDiscount" })
    ]
    [ProducesResponseType(typeof(LoyaltyDiscountRemainStoresResponse), 200)]
    public override async Task<ActionResult<LoyaltyDiscountRemainStoresResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return await _mediator.Send(new LoyaltyDiscountRemainStoresQuery(), cancellationToken);
    }
}