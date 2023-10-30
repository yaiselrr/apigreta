using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Device;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.DeviceEndpoints;

[Route("api/Device")]
public class DeviceGetById : EndpointBaseAsync.WithRequest<DeviceGetByIdRequest>.WithActionResult<DeviceGetByIdResponse>
{
    private readonly IMediator _mediator;

    public DeviceGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get Entity by id",
        Description = "Get Entity by id",
        OperationId = "Device.GetById",
        Tags = new[] { "Device" })
    ]
    [ProducesResponseType(typeof(DeviceGetByIdResponse), 200)]
    public override async Task<ActionResult<DeviceGetByIdResponse>> HandleAsync(
        [FromMultiSource] DeviceGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new DeviceGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}