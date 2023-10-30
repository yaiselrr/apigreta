using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.Device;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.DeviceEndpoints;
[Route("api/Device")]
public class DeviceGet: EndpointBaseAsync.WithoutRequest.WithActionResult<DeviceGetAllResponse>
{
    private readonly IMediator _mediator;

    public DeviceGet(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get all device entities",
        Description = "Get all device entities",
        OperationId = "Device.Get",
        Tags = new[] { "Device" })
    ]
    [ProducesResponseType(typeof(DeviceGetAllResponse), 200)]
    public override async Task<ActionResult<DeviceGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new DeviceGetAllQuery(), cancellationToken));
    }
}