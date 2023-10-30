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
public class RoleUpdate: EndpointBaseAsync.WithRequest<RoleUpdateRequest>.WithResult<RoleUpdateResponse>
{
    private readonly IMediator _mediator;

    public RoleUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update a Role by Id",
        Description = "Update a Role by Id",
        OperationId = "Role.Update",
        Tags = new[] { "Role" })
    ]
    [ProducesResponseType(typeof(RoleGetByIdResponse), 200)]
    public override async Task<RoleUpdateResponse> HandleAsync(
        [FromMultiSource] RoleUpdateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new RoleUpdateCommand(request.Id, request.EntityDto), cancellationToken);
    }
}