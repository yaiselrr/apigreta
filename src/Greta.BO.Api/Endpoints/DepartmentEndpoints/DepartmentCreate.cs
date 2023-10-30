using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Department;
using Greta.BO.BusinessLogic.Handlers.Queries.Department;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.DepartmentEndpoints;
[Route("api/Department")]
public class DepartmentCreate: EndpointBaseAsync.WithRequest<DepartmentCreateRequest>.WithResult<DepartmentCreateResponse>
{
    private readonly IMediator _mediator;

    public DepartmentCreate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Create a new department entity",
        Description = "Create a new department entity",
        OperationId = "Department.Create",
        Tags = new[] { "Department" })
    ]
    [ProducesResponseType(typeof(DepartmentGetByIdResponse), 200)]
    public override async Task<DepartmentCreateResponse> HandleAsync(
        [FromMultiSource] DepartmentCreateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new DepartmentCreateCommand(request.EntityDto), cancellationToken);
    }
}