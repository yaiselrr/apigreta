using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Inventory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.InventoryEndpoints;
[Route("api/Inventory")]
public class InventoryCreateSuggestedOrder: EndpointBaseAsync.WithRequest<InventoryCreateSuggestedOrderRequest>.WithResult<InventoryCreateSuggestedOrderResponse>
{
    private readonly IMediator _mediator;

    public InventoryCreateSuggestedOrder(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPut("{storeId}/{vendorId}")]
    [SwaggerOperation(
        Summary = "Create suggested order Inventory",
        Description = "Create suggested order Inventory",
        OperationId = "Inventory.CreateSuggested",
        Tags = new[] { "Inventory" })
    ]
    [ProducesResponseType(typeof(InventoryCreateSuggestedOrderResponse), 200)]
    public override async Task<InventoryCreateSuggestedOrderResponse> HandleAsync(
        [FromMultiSource] InventoryCreateSuggestedOrderRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new InventoryCreateSuggestedOrderCommand(request.StoreId, request.VendorId, request.Filter), cancellationToken);
    }
}