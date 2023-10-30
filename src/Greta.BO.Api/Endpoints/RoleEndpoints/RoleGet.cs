using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.Role;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.RoleEndpoints;
[Route("api/Role")]
public class RoleGet: EndpointBaseAsync.WithoutRequest.WithActionResult<RoleGetAllResponse>
{
    private readonly IMediator _mediator;

    public RoleGet(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get all role Entities",
        Description = "Get all role Entities",
        OperationId = "Role.Get",
        Tags = new[] { "Role" })
    ]
    [ProducesResponseType(typeof(RoleGetAllResponse), 200)]
    public override async Task<ActionResult<RoleGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new RoleGetAllQuery(), cancellationToken));
    }
}