using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.InventoryQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.InventoryEndpoints;

[Route("api/Inventory")]
public class InventorySuggestedFilter: EndpointBaseAsync.WithRequest<InventorySuggestedFilterRequest>.WithActionResult<InventorySuggestedFilterResponse>
{
    private readonly IMediator _mediator;

    public InventorySuggestedFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{storeId}/{vendorId}/{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of suggested Inventory",
        Description = "Gets a paginated list of suggested Inventory",
        OperationId = "Inventory.SuggestedFilter",
        Tags = new[] { "Inventory" })
    ]
    [ProducesResponseType(typeof(InventorySuggestedFilterResponse), 200)]
    public override async Task<ActionResult<InventorySuggestedFilterResponse>> HandleAsync(
        [FromMultiSource]InventorySuggestedFilterRequest request, 
        CancellationToken cancellationToken = default)
    {                                   
        return await _mediator.Send(new InventorySuggestedFilterQuery(request.StoreId, request.VendorId, request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}