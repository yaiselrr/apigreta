using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Role;
using Greta.BO.BusinessLogic.Handlers.Queries.Role;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.RoleEndpoints;
[Route("api/Role")]
public class RoleCreate: EndpointBaseAsync.WithRequest<RoleCreateRequest>.WithResult<RoleCreateResponse>
{
    private readonly IMediator _mediator;

    public RoleCreate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Create a new Role",
        Description = "Create a new Role",
        OperationId = "Role.Create",
        Tags = new[] { "Role" })
    ]
    [ProducesResponseType(typeof(RoleGetByIdResponse), 200)]
    public override async Task<RoleCreateResponse> HandleAsync(
        [FromMultiSource] RoleCreateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new RoleCreateCommand(request.EntityDto), cancellationToken);
    }
}