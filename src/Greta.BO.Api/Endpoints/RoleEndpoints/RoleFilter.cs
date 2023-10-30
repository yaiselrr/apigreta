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
public class RoleFilter: EndpointBaseAsync.WithRequest<RoleFilterRequest>.WithActionResult<RoleFilterResponse>
{
    private readonly IMediator _mediator;

    public RoleFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of family entity",
        Description = "Gets a paginated list of family entity",
        OperationId = "Role.Filter",
        Tags = new[] { "Role" })
    ]
    [ProducesResponseType(typeof(RoleFilterResponse), 200)]
    public override async Task<ActionResult<RoleFilterResponse>> HandleAsync(
        [FromMultiSource]RoleFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new RoleFilterQuery(request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}