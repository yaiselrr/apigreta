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
public class InventoryFiscalProcess: EndpointBaseAsync.WithRequest<InventoryFiscalProcessRequest>.WithResult<InventoryFiscalProcessResponse>
{
    private readonly IMediator _mediator;

    public InventoryFiscalProcess(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPut("FiscalProcess")]
    [SwaggerOperation(
        Summary = "Create fiscal process Inventory",
        Description = "Create fiscal process Inventory",
        OperationId = "Inventory.FiscalProcess",
        Tags = new[] { "Inventory" })
    ]
    [ProducesResponseType(typeof(InventoryCreateSuggestedOrderResponse), 200)]
    public override async Task<InventoryFiscalProcessResponse> HandleAsync(
        [FromMultiSource] InventoryFiscalProcessRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new InventoryFiscalProcessCommand(request.Filter), cancellationToken);
    }  
}