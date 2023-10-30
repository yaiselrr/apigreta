using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.Device;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.DeviceEndpoints;
[Route("api/Device")]
public class DeviceChangeState : EndpointBaseAsync.WithRequest<DeviceChangeStateRequest>.WithActionResult<DeviceChangeStateResponse>
{
    private readonly IMediator _mediator;

    public DeviceChangeState(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ChangeState/{entityId}/{state}")]
    [SwaggerOperation(
        Summary = "Change the state of the Device entity",
        Description = "Change the state of the Device entity",
        OperationId = "Device.ChangeState",
        Tags = new[] { "Device" })
    ]
    [ProducesResponseType(typeof(DeviceChangeStateResponse), 200)]
    public override async Task<ActionResult<DeviceChangeStateResponse>> HandleAsync(
        [FromRoute] DeviceChangeStateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Id < 1)
            return NotFound();
        return await _mediator.Send(new DeviceChangeStateCommand(request.Id, request.State), cancellationToken);
    }
}