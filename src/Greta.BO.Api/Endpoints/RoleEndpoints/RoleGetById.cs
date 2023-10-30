using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Role;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.RoleEndpoints;

[Route("api/Role")]
public class RoleGetById : EndpointBaseAsync.WithRequest<RoleGetByIdRequest>.WithActionResult<RoleGetByIdResponse>
{
    private readonly IMediator _mediator;

    public RoleGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get Entity by id",
        Description = "Get Entity by id",
        OperationId = "Role.GetById",
        Tags = new[] { "Role" })
    ]
    [ProducesResponseType(typeof(RoleGetByIdResponse), 200)]
    public override async Task<ActionResult<RoleGetByIdResponse>> HandleAsync(
        [FromMultiSource] RoleGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new RoleGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}