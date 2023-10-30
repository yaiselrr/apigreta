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
public class RoleDeleteRange: EndpointBaseAsync.WithRequest<RoleDeleteRangeRequest>.WithResult<RoleDeleteRangeResponse>
{
    private readonly IMediator _mediator;

    public RoleDeleteRange(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("DeleteRange")]
    [SwaggerOperation(
        Summary = "Delete list of Role",
        Description = "Delete list of Role",
        OperationId = "Role.DeleteRange",
        Tags = new[] { "Role" })
    ]
    [ProducesResponseType(typeof(RoleDeleteRangeResponse), 200)]
    public override async Task<RoleDeleteRangeResponse> HandleAsync(
        [FromMultiSource] RoleDeleteRangeRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new RoleDeleteRangeCommand(request.Ids), cancellationToken);
    }
}