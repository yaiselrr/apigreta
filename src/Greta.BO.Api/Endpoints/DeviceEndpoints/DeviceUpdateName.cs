using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Device;
using Greta.BO.BusinessLogic.Handlers.Queries.Device;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.DeviceEndpoints;
[Route("api/Device")]
public class DeviceUpdateName : EndpointBaseAsync.WithRequest<DeviceUpdateNameRequest>.WithResult<DeviceUpdateNameResponse>
{
    private readonly IMediator _mediator;

    public DeviceUpdateName(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("UpdateDeviceName/{entityId}")]
    [SwaggerOperation(
        Summary = "Update name of a device entity by Id",
        Description = "Update name of a device entity by Id",
        OperationId = "Device.UpdateDeviceName",
        Tags = new[] { "Device" })
    ]
    [ProducesResponseType(typeof(DeviceGetByIdResponse), 200)]
    public override async Task<DeviceUpdateNameResponse> HandleAsync(
        [FromMultiSource] DeviceUpdateNameRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new DeviceUpdateNameCommand(request.Id, request.EntityDto), cancellationToken);
    }
}