using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.Department;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.DepartmentEndpoints;
[Route("api/Department")]
public class DepartmentChangeState : EndpointBaseAsync.WithRequest<DepartmentChangeStateRequest>.WithActionResult<DepartmentChangeStateResponse>
{
    private readonly IMediator _mediator;

    public DepartmentChangeState(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ChangeState/{entityId}/{state}")]
    [SwaggerOperation(
        Summary = "Change the state of the department entity",
        Description = "Change the state of the department entity",
        OperationId = "Department.ChangeState",
        Tags = new[] { "Department" })
    ]
    [ProducesResponseType(typeof(DepartmentChangeStateResponse), 200)]
    public override async Task<ActionResult<DepartmentChangeStateResponse>> HandleAsync(
        [FromRoute] DepartmentChangeStateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Id < 1)
            return NotFound();
        return await _mediator.Send(new DepartmentChangeStateCommand(request.Id, request.State), cancellationToken);
    }
}