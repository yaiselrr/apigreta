using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.VendorOrderDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.VendorOrderDetailEndpoints;

[Route("api/VendorOrderDetail")]
public class GetStoreProductByUPC : EndpointBaseAsync.WithRequest<GetStoreProductByUPCRequest>.WithActionResult<GetProductByUpcResponse>
{
    private readonly IMediator _mediator;

    public GetStoreProductByUPC(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("GetStoreProductByUPC/{storeId}/{vendorId}/{upc}")]
    [SwaggerOperation(
        Summary = "Get Store Products by store and UPC",
        Description = "Get Store Products by store and UPC",
        OperationId = "VendorOrderDetail.GetStoreProductByUPC",
        Tags = new[] { "VendorOrderDetail" })
    ]
    [ProducesResponseType(typeof(GetProductByUpcResponse), 200)]
    [ProducesResponseType(404)]
    public override async Task<ActionResult<GetProductByUpcResponse>> HandleAsync(
        [FromMultiSource] GetStoreProductByUPCRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new GetProductByUpcQuery(request.StoreId, request.VendorId, request.UPC), cancellationToken);
        return data != null ? data : NotFound();
    }
}