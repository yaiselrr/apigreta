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
public class DeviceFilter: EndpointBaseAsync.WithRequest<DeviceFilterRequest>.WithResult<DeviceFilterResponse>
{
    private readonly IMediator _mediator;

    public DeviceFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of the device entity",
        Description = "Gets a paginated list of the device entity",
        OperationId = "Device.Filter",
        Tags = new[] { "Device" })
    ]
    [ProducesResponseType(typeof(DeviceFilterResponse), 200)]
    public override async Task<DeviceFilterResponse> HandleAsync(
        [FromMultiSource]DeviceFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new DeviceFilterQuery(null, request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}