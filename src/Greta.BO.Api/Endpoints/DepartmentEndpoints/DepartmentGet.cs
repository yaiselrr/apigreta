using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.Department;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.DepartmentEndpoints;
[Route("api/Department")]
public class DepartmentGet: EndpointBaseAsync.WithoutRequest.WithActionResult<DepartmentGetAllResponse>
{
    private readonly IMediator _mediator;

    public DepartmentGet(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get all department entities",
        Description = "Get all department entities",
        OperationId = "Department.Get",
        Tags = new[] { "Department" })
    ]
    [ProducesResponseType(typeof(DepartmentGetAllResponse), 200)]
    public override async Task<ActionResult<DepartmentGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new DepartmentGetAllQuery(), cancellationToken));
    }
}