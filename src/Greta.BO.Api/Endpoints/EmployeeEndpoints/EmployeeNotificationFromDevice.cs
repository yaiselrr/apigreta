using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.Employee;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.EmployeeEndpoints;

[Route("api/Employee")]
public class EmployeeNotificationFromDevice : EndpointBaseAsync.WithRequest<EmployeeNotificationFromDeviceRequest>.WithResult<EmployeeNotificationFromDeviceResponse>
{
    private readonly IMediator _mediator;

    public EmployeeNotificationFromDevice(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("EmployeeNotificationFromDevice")]
    [SwaggerOperation(
        Summary = "Employee Notification From Device",
        Description = "Employee Notification From Device",
        OperationId = "Employee.EmployeeNotificationFromDevice",
        Tags = new[] { "Employee" })
    ]
    [ProducesResponseType(typeof(EmployeeNotificationFromDeviceResponse), 200)]
    public override async Task<EmployeeNotificationFromDeviceResponse> HandleAsync(
        EmployeeNotificationFromDeviceRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new EmployeeNotificationFromDeviceQuery(
            request.Filter.DeviceLicenceCode,
            request.Filter.EmployeeId
        ), cancellationToken);
    }
}