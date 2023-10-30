using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Employee;
using Greta.BO.BusinessLogic.Handlers.Queries.TimeKeepingQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.TimeKeeping;

[Route("api/TimeKeeping")]
public class EmployeeNotification : EndpointBaseAsync.WithRequest<EmployeeNotificationRequest>.WithActionResult<
    EmployeeNotificationsResponse>
{
    private readonly IMediator _mediator;

    public EmployeeNotification(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("EmployeeNotification/{storeId}/{employeeId}")]
    [SwaggerOperation(
        Summary = "Get a list of employee notifications for one store",
        Description = "Get a list of employee notifications for one store",
        OperationId = "TimeKeeping.EmployeeNotification",
        Tags = new[] { "TimeKeeping" })
    ]
    [ProducesResponseType(typeof(TimeKeepingFilterResponse), 200)]
    public override async Task<ActionResult<EmployeeNotificationsResponse>> HandleAsync(
        [FromMultiSource] EmployeeNotificationRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new EmployeeNotificationsQuery(request.StoreId, request.EmployeeId),
            cancellationToken);
    }
}