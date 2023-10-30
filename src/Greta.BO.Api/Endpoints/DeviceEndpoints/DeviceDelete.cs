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
public class DeviceDelete : EndpointBaseAsync.WithRequest<DeviceDeleteRequest>.WithActionResult<DeviceDeleteResponse>
{
    private readonly IMediator _mediator;

    public DeviceDelete(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete("{entityId}")]
    [SwaggerOperation(
        Summary = "Delete a device entity by Id",
        Description = "Delete a device entity by Id",
        OperationId = "Device.Delete",
        Tags = new[] { "Device" })
    ]
    [ProducesResponseType(typeof(DeviceDeleteResponse), 200)]
    public override async Task<ActionResult<DeviceDeleteResponse>> HandleAsync(
        [FromMultiSource] DeviceDeleteRequest request, CancellationToken cancellationToken = default)
    {
        return request.Id < 1 ? NotFound() : await _mediator.Send(new DeviceDeleteCommand(request.Id), cancellationToken);
    }
}