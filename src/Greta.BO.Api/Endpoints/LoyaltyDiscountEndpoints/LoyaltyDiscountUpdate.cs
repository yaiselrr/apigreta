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
public class LoyaltyDiscountUpdate: EndpointBaseAsync.WithRequest<LoyaltyDiscountUpdateRequest>.WithResult<LoyaltyDiscountUpdateResponse>
{
    private readonly IMediator _mediator;

    public LoyaltyDiscountUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update a LoyaltyDiscount by Id",
        Description = "Update a LoyaltyDiscount by Id",
        OperationId = "LoyaltyDiscount.Update",
        Tags = new[] { "LoyaltyDiscount" })
    ]
    [ProducesResponseType(typeof(LoyaltyDiscountGetByIdResponse), 200)]
    public override async Task<LoyaltyDiscountUpdateResponse> HandleAsync(
        [FromMultiSource] LoyaltyDiscountUpdateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new LoyaltyDiscountUpdateCommand(request.Id, request.EntityDto), cancellationToken);
    }
}