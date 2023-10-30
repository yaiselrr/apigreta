using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.LoyaltyDiscount;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.LoyaltyDiscountEndpoints;

[Route("api/LoyaltyDiscount")]
public class LoyaltyDiscountGetById : EndpointBaseAsync.WithRequest<LoyaltyDiscountGetByIdRequest>.WithActionResult<LoyaltyDiscountGetByIdResponse>
{
    private readonly IMediator _mediator;

    public LoyaltyDiscountGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get LoyaltyDiscount by id",
        Description = "Get LoyaltyDiscount by id",
        OperationId = "LoyaltyDiscount.GetById",
        Tags = new[] { "LoyaltyDiscount" })
    ]
    [ProducesResponseType(typeof(LoyaltyDiscountGetByIdResponse), 200)]
    public override async Task<ActionResult<LoyaltyDiscountGetByIdResponse>> HandleAsync(
        [FromMultiSource] LoyaltyDiscountGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new LoyaltyDiscountGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}