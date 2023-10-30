using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.FunctionGroup;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ProfilesEndpoints;
[Route("api/Profiles")]
public class ProfilesGetPosPermission: EndpointBaseAsync.WithoutRequest.WithActionResult<FunctionGroupGetAllResponse>
{
    private readonly IMediator _mediator;

    public ProfilesGetPosPermission(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("GetPOSPermissions")]
    [SwaggerOperation(
        Summary = "Get all POS permissions",
        Description = "Get all POS permissions",
        OperationId = "Profiles.GetPOSPermissions",
        Tags = new[] { "Profiles" })
    ]
    [ProducesResponseType(typeof(FunctionGroupGetAllResponse), 200)]
    public override async Task<ActionResult<FunctionGroupGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var data = await _mediator.Send(new FunctionGroupGetAllQuery(2), cancellationToken);
        return data != null ? Ok(data) : NotFound();        
    }
}