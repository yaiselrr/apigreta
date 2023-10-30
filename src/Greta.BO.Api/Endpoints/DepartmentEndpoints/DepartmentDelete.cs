using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Department;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.DepartmentEndpoints;
[Route("api/Department")]
public class DepartmentDelete : EndpointBaseAsync.WithRequest<DepartmentDeleteRequest>.WithActionResult<DepartmentDeleteResponse>
{
    private readonly IMediator _mediator;

    public DepartmentDelete(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete("{entityId}")]
    [SwaggerOperation(
        Summary = "Delete a department entity by Id",
        Description = "Delete a department entity by Id",
        OperationId = "Department.Delete",
        Tags = new[] { "Department" })
    ]
    [ProducesResponseType(typeof(DepartmentDeleteResponse), 200)]
    public override async Task<ActionResult<DepartmentDeleteResponse>> HandleAsync(
        [FromMultiSource] DepartmentDeleteRequest request, CancellationToken cancellationToken = default)
    {
        return request.Id < 1 ? NotFound() : await _mediator.Send(new DepartmentDeleteCommand(request.Id), cancellationToken);
    }
}