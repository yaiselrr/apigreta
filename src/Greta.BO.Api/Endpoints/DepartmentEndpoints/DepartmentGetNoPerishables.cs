using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.Department;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.DepartmentEndpoints;
[Route("api/Department")]
public class DepartmentGetNoPerishables: EndpointBaseAsync.WithoutRequest.WithActionResult<DepartmentGetPerishablesResponse>
{
    private readonly IMediator _mediator;

    public DepartmentGetNoPerishables(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("GetNoPerishables")]
    [SwaggerOperation(
        Summary = "Get all department entities no perishables",
        Description = "Get all department entities no perishables",
        OperationId = "Department.GetNoPerishables",
        Tags = new[] { "Department" })
    ]
    [ProducesResponseType(typeof(DepartmentGetPerishablesResponse), 200)]
    public override async Task<ActionResult<DepartmentGetPerishablesResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new DepartmentGetPerishablesQuery(false), cancellationToken));
    }
}