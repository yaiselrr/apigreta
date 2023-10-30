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
public class DeleteFamilyProduct: EndpointBaseAsync.WithRequest<DeleteFamilyProductRequest>.WithActionResult<FamilyDeleteProductResponse>
{
    private readonly IMediator _mediator;

    public DeleteFamilyProduct(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete("DeleteFamilyProduct/{entityId:long}/{productId:long}")]
    [SwaggerOperation(
        Summary = "Remove a product from family",
        Description = "Remove a product from family",
        OperationId = "Family.DeleteFamilyProduct",
        Tags = new[] { "Family" })
    ]
    [ProducesResponseType(typeof(FamilyDeleteProductResponse), 200)]
    public override async Task<ActionResult<FamilyDeleteProductResponse>> HandleAsync(
        [FromMultiSource]DeleteFamilyProductRequest request, CancellationToken cancellationToken = default)
    {
        return request.EntityId < 1 ? 
            NotFound() : 
            Ok(await _mediator.Send(new FamilyDeleteProductCommand(request.EntityId, request.ProductId), cancellationToken));
    }
}