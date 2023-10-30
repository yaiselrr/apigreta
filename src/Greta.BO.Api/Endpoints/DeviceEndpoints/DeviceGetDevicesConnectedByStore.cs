using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Device;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static Greta.BO.BusinessLogic.Handlers.Queries.Device.GetDevicesConnectedByStore;

namespace Greta.BO.Api.Endpoints.DeviceEndpoints;

[Route("api/Device")]
public class DeviceGetDevicesConnectedByStore : EndpointBaseAsync.WithRequest<DeviceGetDevicesConnectedByStoreRequest>.WithActionResult<List<Api.Entities.Device>>
{
    private readonly IMediator _mediator;

    public DeviceGetDevicesConnectedByStore(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetDevicesConnectedByStore/{storeId:int}")]
    [SwaggerOperation(
        Summary = "Get devices connected by store",
        Description = "Get devices connected by store",
        OperationId = "Device.GetDevicesConnectedByStore",
        Tags = new[] { "Device" })
    ]
    [ProducesResponseType(typeof(List<Api.Entities.Device>), 200)]
    public override async Task<ActionResult<List<Api.Entities.Device>>> HandleAsync(
        [FromMultiSource] DeviceGetDevicesConnectedByStoreRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new DeviceGetDevicesConnectedByStoreQuery(request.StoreId), cancellationToken);
        return data != null ? data : NotFound();
    }
}