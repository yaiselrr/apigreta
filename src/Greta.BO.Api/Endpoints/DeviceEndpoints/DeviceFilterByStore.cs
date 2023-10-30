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
public class DeviceFilterByStore: EndpointBaseAsync.WithRequest<DeviceFilterByStoreRequest>.WithResult<DeviceFilterResponse>
{
    private readonly IMediator _mediator;

    public DeviceFilterByStore(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("FilterByStore/{storeid}/{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of the device entity",
        Description = "Gets a paginated list of the device entity",
        OperationId = "Device.FilterByStore",
        Tags = new[] { "Device" })
    ]
    [ProducesResponseType(typeof(DeviceFilterResponse), 200)]
    public override async Task<DeviceFilterResponse> HandleAsync(
        [FromMultiSource]DeviceFilterByStoreRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new DeviceFilterQuery(request.Storeid, request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}