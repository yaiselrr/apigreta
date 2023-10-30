using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Device;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.DeviceEndpoints;

[Route("api/Device")]
public class DeviceForceFullSynchro : EndpointBaseAsync.WithRequest<DeviceForceFullSynchroRequest>.WithActionResult<DeviceForceFullSynchroResponse>
{
    private readonly IMediator _mediator;

    public DeviceForceFullSynchro(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("DeviceForceFullSynchro/{deviceId:int}")]
    [SwaggerOperation(
        Summary = "Device force full synchro",
        Description = "Device force full synchro",
        OperationId = "Device.DeviceForceFullSynchro",
        Tags = new[] { "Device" })
    ]
    [ProducesResponseType(typeof(DeviceForceFullSynchroResponse), 200)]
    public override async Task<ActionResult<DeviceForceFullSynchroResponse>> HandleAsync(
        [FromMultiSource] DeviceForceFullSynchroRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new DeviceForceFullSynchroCommand(request.DeviceId), cancellationToken);
        return data != null ? data : NotFound();
    }
}