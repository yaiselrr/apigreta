using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.Department;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.DepartmentEndpoints;
[Route("api/Department")]
public class DepartmentGetPerishables: EndpointBaseAsync.WithoutRequest.WithActionResult<DepartmentGetPerishablesResponse>
{
    private readonly IMediator _mediator;

    public DepartmentGetPerishables(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("GetPerishables")]
    [SwaggerOperation(
        Summary = "Get all department entities perishables",
        Description = "Get all department entities perishables",
        OperationId = "Department.GetPerishables",
        Tags = new[] { "Department" })
    ]
    [ProducesResponseType(typeof(DepartmentGetPerishablesResponse), 200)]
    public override async Task<ActionResult<DepartmentGetPerishablesResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
       return Ok(await _mediator.Send(new DepartmentGetPerishablesQuery(true), cancellationToken));
    }
}