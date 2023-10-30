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
public class InventoryFilter: EndpointBaseAsync.WithRequest<InventoryFilterRequest>.WithActionResult<InventoryFilterResponse>
{
    private readonly IMediator _mediator;

    public InventoryFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{storeId}/{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of Inventory entity",
        Description = "Gets a paginated list of Inventory entity",
        OperationId = "Inventory.Filter",
        Tags = new[] { "Inventory" })
    ]
    [ProducesResponseType(typeof(InventoryFilterResponse), 200)]
    public override async Task<ActionResult<InventoryFilterResponse>> HandleAsync(
        [FromMultiSource]InventoryFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new InventoryFilterQuery(request.StoreId, request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}