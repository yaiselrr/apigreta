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
public class InventoryFiscalPreProcess: EndpointBaseAsync.WithRequest<InventoryFiscalPreProcessRequest>.WithResult<InventoryFiscalPreProcessResponse>
{
    private readonly IMediator _mediator;

    public InventoryFiscalPreProcess(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPut("FiscalPreProcess")]
    [SwaggerOperation(
        Summary = "Create fiscal pre process Inventory",
        Description = "Create fiscal pre process Inventory",
        OperationId = "Inventory.FiscalPreProcess",
        Tags = new[] { "Inventory" })
    ]
    [ProducesResponseType(typeof(InventoryCreateSuggestedOrderResponse), 200)]
    public override async Task<InventoryFiscalPreProcessResponse> HandleAsync(
        [FromMultiSource] InventoryFiscalPreProcessRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new InventoryFiscalPreProcessCommand(request.Filter), cancellationToken);
    }  
}