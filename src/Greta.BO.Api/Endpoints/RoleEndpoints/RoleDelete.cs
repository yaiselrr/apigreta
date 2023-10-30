using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Role;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.RoleEndpoints;
[Route("api/Role")]
public class RoleDelete : EndpointBaseAsync.WithRequest<RoleDeleteRequest>.WithActionResult<RoleDeleteResponse>
{
    private readonly IMediator _mediator;

    public RoleDelete(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete("{entityId}")]
    [SwaggerOperation(
        Summary = "Delete a Role by Id",
        Description = "Delete a Role by Id",
        OperationId = "Role.Delete",
        Tags = new[] { "Role" })
    ]
    [ProducesResponseType(typeof(RoleDeleteResponse), 200)]
    public override async Task<ActionResult<RoleDeleteResponse>> HandleAsync(
        [FromMultiSource] RoleDeleteRequest request, CancellationToken cancellationToken = default)
    {
        return request.Id < 1 ? NotFound() : await _mediator.Send(new RoleDeleteCommand(request.Id), cancellationToken);
    }
}