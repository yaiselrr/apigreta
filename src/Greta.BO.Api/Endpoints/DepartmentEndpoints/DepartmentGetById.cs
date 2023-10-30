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
public class DepartmentGetById : EndpointBaseAsync.WithRequest<DepartmentGetByIdRequest>.WithActionResult<DepartmentGetByIdResponse>
{
    private readonly IMediator _mediator;

    public DepartmentGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get department entity by id",
        Description = "Get department entity by id",
        OperationId = "Department.GetById",
        Tags = new[] { "Department" })
    ]
    [ProducesResponseType(typeof(DepartmentGetByIdResponse), 200)]
    public override async Task<ActionResult<DepartmentGetByIdResponse>> HandleAsync(
        [FromMultiSource] DepartmentGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new DepartmentGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}