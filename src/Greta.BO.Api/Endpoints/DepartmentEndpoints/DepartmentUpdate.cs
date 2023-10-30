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
public class DepartmentUpdate : EndpointBaseAsync.WithRequest<DepartmentUpdateRequest>.WithResult<DepartmentUpdateResponse>
{
    private readonly IMediator _mediator;

    public DepartmentUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update a department entity by Id",
        Description = "Update a department entity by Id",
        OperationId = "Department.Update",
        Tags = new[] { "Department" })
    ]
    [ProducesResponseType(typeof(DepartmentGetByIdResponse), 200)]
    public override async Task<DepartmentUpdateResponse> HandleAsync(
        [FromMultiSource] DepartmentUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new DepartmentUpdateCommand(request.Id, request.EntityDto), cancellationToken);
    }
}