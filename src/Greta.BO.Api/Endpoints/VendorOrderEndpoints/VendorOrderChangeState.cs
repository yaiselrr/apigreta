using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.Family;
using Greta.BO.BusinessLogic.Handlers.Command.VendorOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.VendorOrderEndpoints;
[Route("api/VendorOrder")]
public class VendorOrderChangeState : EndpointBaseAsync.WithRequest<ChangeStateRequest>.WithActionResult<VendorOrderChangeStateResponse>
{
    private readonly IMediator _mediator;

    public VendorOrderChangeState(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ChangeState/{entityId}/{state}")]
    [SwaggerOperation(
        Summary = "Change State of VendorOrder",
        Description = "Change State of VendorOrder",
        OperationId = "VendorOrder.ChangeState",
        Tags = new[] { "VendorOrder" })
    ]
    [ProducesResponseType(typeof(VendorOrderChangeStateResponse), 200)]
    public override async Task<ActionResult<VendorOrderChangeStateResponse>> HandleAsync(
        [FromRoute] ChangeStateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Id < 1)
            return NotFound();
        return await _mediator.Send(new VendorOrderChangeStateCommand(request.Id, request.State), cancellationToken);
    }
}