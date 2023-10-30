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
public class LoyaltyDiscountFilter: EndpointBaseAsync.WithRequest<LoyaltyDiscountFilterRequest>.WithActionResult<LoyaltyDiscountFilterResponse>
{
    private readonly IMediator _mediator;

    public LoyaltyDiscountFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of LoyaltyDiscount entity",
        Description = "Gets a paginated list of LoyaltyDiscount entity",
        OperationId = "LoyaltyDiscount.Filter",
        Tags = new[] { "LoyaltyDiscount" })
    ]
    [ProducesResponseType(typeof(LoyaltyDiscountFilterResponse), 200)]
    public override async Task<ActionResult<LoyaltyDiscountFilterResponse>> HandleAsync(
        [FromMultiSource]LoyaltyDiscountFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new LoyaltyDiscountFilterQuery(request.CurrentPage, request.PageSize, request.Filter), cancellationToken));
    }
}