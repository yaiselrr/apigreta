using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.LoyaltyDiscount;
using Greta.BO.BusinessLogic.Handlers.Queries.LoyaltyDiscount;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.LoyaltyDiscountEndpoints;
[Route("api/LoyaltyDiscount")]
public class LoyaltyDiscountCreate : EndpointBaseAsync.WithRequest<LoyaltyDiscountCreateRequest>.WithResult<LoyaltyDiscountCreateResponse>
{
    private readonly IMediator _mediator;

    public LoyaltyDiscountCreate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Create a new LoyaltyDiscount",
        Description = "Create a new LoyaltyDiscount",
        OperationId = "LoyaltyDiscount.Create",
        Tags = new[] { "LoyaltyDiscount" })
    ]
    [ProducesResponseType(typeof(LoyaltyDiscountGetByIdResponse), 200)]
    public override async Task<LoyaltyDiscountCreateResponse> HandleAsync(
        [FromMultiSource] LoyaltyDiscountCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new LoyaltyDiscountCreateCommand(request.EntityDto), cancellationToken);
    }
}