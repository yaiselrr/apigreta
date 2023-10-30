using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.LoyaltyDiscount;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.LoyaltyDiscountEndpoints;
[Route("api/LoyaltyDiscount")]
public class LoyaltyDiscountGet : EndpointBaseAsync.WithoutRequest.WithActionResult<LoyaltyDiscountGetAllResponse>
{
    private readonly IMediator _mediator;

    public LoyaltyDiscountGet(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get all LoyaltyDiscount Entities",
        Description = "Get all LoyaltyDiscount Entities",
        OperationId = "LoyaltyDiscount.Get",
        Tags = new[] { "LoyaltyDiscount" })
    ]
    [ProducesResponseType(typeof(LoyaltyDiscountGetAllResponse), 200)]
    public override async Task<ActionResult<LoyaltyDiscountGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new LoyaltyDiscountGetAllQuery(), cancellationToken));
    }
}