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
public class InventoryUpdate: EndpointBaseAsync.WithRequest<InventoryUpdateRequest>.WithResult<InventoryUpdateResponse>
{
    private readonly IMediator _mediator;

    public InventoryUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPut()]
    [SwaggerOperation(
        Summary = "Update Inventory",
        Description = "Update Inventory",
        OperationId = "Inventory.Update",
        Tags = new[] { "Inventory" })
    ]
    [ProducesResponseType(typeof(InventoryCreateSuggestedOrderResponse), 200)]
    public override async Task<InventoryUpdateResponse> HandleAsync(
        [FromMultiSource] InventoryUpdateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new InventoryUpdateCommand(request.Filter), cancellationToken);
    }  
}