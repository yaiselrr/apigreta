using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Device;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static Greta.BO.BusinessLogic.Handlers.Queries.Device.DeviceGetSynchronizationStatusById;

namespace Greta.BO.Api.Endpoints.DeviceEndpoints;

[Route("api/Device")]
public class DeviceGetSynchronizationStatusById : EndpointBaseAsync.WithRequest<DeviceGetSynchronizationStatusByIdRequest>.WithActionResult<DeviceGetSynchronizationStatusByIdResponse>
{
    private readonly IMediator _mediator;

    public DeviceGetSynchronizationStatusById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetSynchronizationStatus/{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get synchronization status response",
        Description = "Get synchronization status response",
        OperationId = "Device.GetSynchronizationStatus",
        Tags = new[] { "Device" })
    ]
    [ProducesResponseType(typeof(DeviceGetSynchronizationStatusByIdResponse), 200)]
    public override async Task<ActionResult<DeviceGetSynchronizationStatusByIdResponse>> HandleAsync(
        [FromMultiSource] DeviceGetSynchronizationStatusByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new DeviceGetSynchronizationStatusByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}