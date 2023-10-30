using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.VendorOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.VendorOrderEndpoints;

[Route("api/VendorOrder")]
public class VendorOrderHasProducts : EndpointBaseAsync.WithRequest<VendorOrderHasProductsRequest>.WithActionResult<VendorOrderHasProductsResponse>
{
    private readonly IMediator _mediator;

    public VendorOrderHasProducts(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("HasProducts/{vendorOrderId:long}")]
    [SwaggerOperation(
        Summary = "Verify if the vendor order has assigned products",
        Description = "Return true if the vendor order have products ",
        OperationId = "VendorOrder.HasProducts",
        Tags = new[] { "VendorOrder" })
    ]
    [ProducesResponseType(typeof(ActionResult<VendorOrderHasProductsResponse>), 200)]
    [ProducesResponseType(404)]
    public override async Task<ActionResult<VendorOrderHasProductsResponse>> HandleAsync(
        [FromMultiSource] VendorOrderHasProductsRequest request, CancellationToken cancellationToken = default)
    {
        return (request.VendorOrderId < 1)? NotFound() : Ok(await _mediator.Send(new VendorOrderHasProductsQuery(request.VendorOrderId), cancellationToken));
    }
}