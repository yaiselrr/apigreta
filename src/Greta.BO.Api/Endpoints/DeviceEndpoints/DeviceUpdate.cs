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
public class DeviceUpdate : EndpointBaseAsync.WithRequest<DeviceUpdateRequest>.WithResult<DeviceUpdateResponse>
{
    private readonly IMediator _mediator;

    public DeviceUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update a device entity by Id",
        Description = "Update a device entity by Id",
        OperationId = "Device.UpdateEntity",
        Tags = new[] { "Device" })
    ]
    [ProducesResponseType(typeof(DeviceGetByIdResponse), 200)]
    public override async Task<DeviceUpdateResponse> HandleAsync(
        [FromMultiSource] DeviceUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new DeviceUpdateCommand(request.Id, request.EntityDto), cancellationToken);
    }
}