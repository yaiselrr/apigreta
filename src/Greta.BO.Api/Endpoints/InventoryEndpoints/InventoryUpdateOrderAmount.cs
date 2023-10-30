using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Inventory;
using Greta.BO.BusinessLogic.Handlers.Command.StoreProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.InventoryEndpoints;
[Route("api/Inventory")]
public class InventoryUpdateOrderAmount: EndpointBaseAsync.WithRequest<InventoryUpdateOrderAmountRequest>.WithResult<SuggestedUpdateResponse>
{
    private readonly IMediator _mediator;

    public InventoryUpdateOrderAmount(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update OrderAmount Inventory",
        Description = "Update OrderAmount Inventory",
        OperationId = "Inventory.UpdateOrderAmount",
        Tags = new[] { "Inventory" })
    ]
    [ProducesResponseType(typeof(InventoryCreateSuggestedOrderResponse), 200)]
    public override async Task<SuggestedUpdateResponse> HandleAsync(
        [FromMultiSource] InventoryUpdateOrderAmountRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new SuggestedUpdateCommand(request.Id, request.Entity.OrderAmount), cancellationToken);
    }  
}