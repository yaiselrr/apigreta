using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.VendorOrderDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.VendorOrderDetailEndpoints;

[Route("api/VendorOrderDetail")]
public class VendorOrderDetailAmountUpdate : EndpointBaseAsync.WithRequest<VendorOrderDetailAmountUpdateRequest>.WithActionResult<
    VendorOrderDetailAmountUpdateResponse>
{
    private readonly IMediator _mediator;

    public VendorOrderDetailAmountUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("EditDetailPurchaseOrder/{entityId}")]
    [SwaggerOperation(
        Summary = "Update order amount a Vendor Order detail by Id",
        Description = "Update order amount a Vendor Order detail by Id",
        OperationId = "VendorOrderDetail.UpdateAmount",
        Tags = new[] { "VendorOrderDetail" })
    ]
    [ProducesResponseType(typeof(ActionResult<VendorOrderDetailAmountUpdateResponse>), 200)]
    [ProducesResponseType(404)]
    public override async Task<ActionResult<VendorOrderDetailAmountUpdateResponse>> HandleAsync(
        [FromMultiSource] VendorOrderDetailAmountUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return request.Id < 1
            ? NotFound()
            : await _mediator.Send(new VendorOrderDetailAmountUpdateCommand(request.Id, request.OrderAmount.OrderAmount),
                cancellationToken);
    }
}