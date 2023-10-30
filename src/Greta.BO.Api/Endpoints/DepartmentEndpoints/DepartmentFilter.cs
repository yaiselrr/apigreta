using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Department;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.DepartmentEndpoints;

[Route("api/Department")]
public class DepartmentFilter: EndpointBaseAsync.WithRequest<DepartmentFilterRequest>.WithActionResult<DepartmentFilterResponse>
{
    private readonly IMediator _mediator;

    public DepartmentFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of the department entity",
        Description = "Gets a paginated list of the department entity",
        OperationId = "Department.Filter",
        Tags = new[] { "Department" })
    ]
    [ProducesResponseType(typeof(DepartmentFilterResponse), 200)]
    public override async Task<ActionResult<DepartmentFilterResponse>> HandleAsync(
        [FromMultiSource]DepartmentFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new DepartmentFilterQuery(request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}