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
public class DepartmentDeleteRange: EndpointBaseAsync.WithRequest<DepartmentDeleteRangeRequest>.WithResult<DepartmentDeleteRangeResponse>
{
    private readonly IMediator _mediator;

    public DepartmentDeleteRange(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("DeleteRange")]
    [SwaggerOperation(
        Summary = "Delete list of the department entities",
        Description = "Delete list of  the department entities",
        OperationId = "Department.DeleteRange",
        Tags = new[] { "Department" })
    ]
    [ProducesResponseType(typeof(DepartmentDeleteRangeResponse), 200)]
    public override async Task<DepartmentDeleteRangeResponse> HandleAsync(
        [FromMultiSource] DepartmentDeleteRangeRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new DepartmentDeleteRangeCommand(request.Ids), cancellationToken);
    }
}