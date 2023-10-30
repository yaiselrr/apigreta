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
public class GetStatusPurchaseOrder : EndpointBaseAsync.WithRequest<GetStatusPurchaseOrderRequest>.WithActionResult<
    GetStatusPurchaseOrderResponse>
{
    private readonly IMediator _mediator;

    public GetStatusPurchaseOrder(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetStatusPurchaseOrder/{vendorOrderId}")]
    [SwaggerOperation(
        Summary = "Get status of purchase order",
        Description = "Get status of purchase order",
        OperationId = "VendorOrderDetail.GetStatusPurchaseOrder",
        Tags = new[] { "VendorOrderDetail" })
    ]
    [ProducesResponseType(typeof(GetStatusPurchaseOrderResponse), 200)]
    [ProducesResponseType(404)]
    public override async Task<ActionResult<GetStatusPurchaseOrderResponse>> HandleAsync(
        [FromMultiSource] GetStatusPurchaseOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new GetStatusPurchaseOrderQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}