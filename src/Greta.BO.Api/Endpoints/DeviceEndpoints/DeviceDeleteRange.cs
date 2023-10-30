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
public class DeviceDeleteRange: EndpointBaseAsync.WithRequest<DeviceDeleteRangeRequest>.WithResult<DeviceDeleteRangeResponse>
{
    private readonly IMediator _mediator;

    public DeviceDeleteRange(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("DeleteRange")]
    [SwaggerOperation(
        Summary = "Delete list of Device entities",
        Description = "Delete list of Device entities",
        OperationId = "Device.DeleteRange",
        Tags = new[] { "Device" })
    ]
    [ProducesResponseType(typeof(DeviceDeleteRangeResponse), 200)]
    public override async Task<DeviceDeleteRangeResponse> HandleAsync(
        [FromMultiSource] DeviceDeleteRangeRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new DeviceDeleteRangeCommand(request.Ids), cancellationToken);
    }
}