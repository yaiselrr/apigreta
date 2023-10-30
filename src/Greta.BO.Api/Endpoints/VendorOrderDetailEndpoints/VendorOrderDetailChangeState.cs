using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.VendorOrder;
using Greta.BO.BusinessLogic.Handlers.Command.VendorOrderDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.VendorOrderDetailEndpoints;

[Route("api/VendorOrderDetail")]
public class VendorOrderDetailChangeState : EndpointBaseAsync.WithRequest<ChangeStateRequest>.WithActionResult<
    VendorOrderDetailChangeStateResponse>
{
    private readonly IMediator _mediator;

    public VendorOrderDetailChangeState(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ChangeState/{entityId}/{state}")]
    [SwaggerOperation(
        Summary = "Change State of Vendor Order detail",
        Description = "Change State of Vendor Order detail",
        OperationId = "VendorOrderDetail.ChangeState",
        Tags = new[] { "VendorOrderDetail" })
    ]
    [ProducesResponseType(typeof(VendorOrderChangeStateResponse), 200)]
    public override async Task<ActionResult<VendorOrderDetailChangeStateResponse>> HandleAsync(
        [FromRoute] ChangeStateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Id < 1)
            return NotFound();
        return await _mediator.Send(new VendorOrderDetailChangeStateCommand(request.Id, request.State),
            cancellationToken);
    }
}