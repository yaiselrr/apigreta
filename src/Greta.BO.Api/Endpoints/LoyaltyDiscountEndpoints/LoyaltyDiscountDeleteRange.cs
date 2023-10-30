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
public class LoyaltyDiscountDeleteRange: EndpointBaseAsync.WithRequest<LoyaltyDiscountDeleteRangeRequest>.WithResult<LoyaltyDiscountDeleteRangeResponse>
{
    private readonly IMediator _mediator;

    public LoyaltyDiscountDeleteRange(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("DeleteRange")]
    [SwaggerOperation(
        Summary = "Delete list of LoyaltyDiscount",
        Description = "Delete list of LoyaltyDiscount",
        OperationId = "LoyaltyDiscount.DeleteRange",
        Tags = new[] { "LoyaltyDiscount" })
    ]
    [ProducesResponseType(typeof(LoyaltyDiscountDeleteRangeResponse), 200)]
    public override async Task<LoyaltyDiscountDeleteRangeResponse> HandleAsync(
        [FromMultiSource] LoyaltyDiscountDeleteRangeRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new LoyaltyDiscountDeleteRangeCommand(request.Ids), cancellationToken);
    }
}