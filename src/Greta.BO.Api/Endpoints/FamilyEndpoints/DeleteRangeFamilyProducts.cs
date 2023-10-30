using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Family;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.FamilyEndpoints;
[Route("api/Family")]
public class DeleteRangeFamilyProducts: EndpointBaseAsync.WithRequest<DeleteRangeFamilyProductsRequest>.WithActionResult<FamilyDeleteRangeProductsResponse>
{
    private readonly IMediator _mediator;

    public DeleteRangeFamilyProducts(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("DeleteProductsRange/{entityId}")]
    [SwaggerOperation(
        Summary = "Delete list of product from a Family",
        Description = "Delete list of  product from a Family",
        OperationId = "Family.DeleteFamilyProductRange",
        Tags = new[] { "Family" })
    ]
    [ProducesResponseType(typeof(FamilyDeleteRangeProductsResponse), 200)]
    public override async Task<ActionResult<FamilyDeleteRangeProductsResponse>> HandleAsync(
        [FromMultiSource]DeleteRangeFamilyProductsRequest request, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new FamilyDeleteRangeProductsCommand(request.EntityId, request.Ids), cancellationToken));
    }
}
