using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.VendorOrder;
using Greta.BO.BusinessLogic.Handlers.Queries.VendorOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.VendorOrderEndpoints;

[Route("api/VendorOrder")]
public class UpdateReceivePurchase : EndpointBaseAsync.WithRequest<VendorOrderReceivedRequest>.WithActionResult<VendorOrderReceivedResponse>
{
    private readonly IMediator _mediator;

    public UpdateReceivePurchase(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("UpdateReceivePurchase/{entityId:long}")]
    [SwaggerOperation(
        Summary = "Update a Vendor Order received",
        Description = "Update a Vendor Order received",
        OperationId = "VendorOrder.UpdateReceive",
        Tags = new[] { "VendorOrder" })
    ]
    [ProducesResponseType(typeof(VendorOrderReceivedResponse), 200)]
    public override async Task<ActionResult<VendorOrderReceivedResponse>> HandleAsync(
        [FromMultiSource] VendorOrderReceivedRequest request,
        CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new VendorOrderReceivedCommand(request.Id, request.EntityDto), cancellationToken));
    }
}