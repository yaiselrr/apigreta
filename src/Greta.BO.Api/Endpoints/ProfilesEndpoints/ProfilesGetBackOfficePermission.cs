using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.FunctionGroup;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ProfilesEndpoints;
[Route("api/Profiles")]
public class ProfilesGetBoPermissions: EndpointBaseAsync.WithoutRequest.WithActionResult<FunctionGroupGetAllResponse>
{
    private readonly IMediator _mediator;

    public ProfilesGetBoPermissions(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("GetBackOfficePermissions")]
    [SwaggerOperation(
        Summary = "Get all Back Office permissions",
        Description = "Get all Back Office permissions",
        OperationId = "Profiles.GetBackOfficePermissions",
        Tags = new[] { "Profiles" })
    ]
    [ProducesResponseType(typeof(FunctionGroupGetAllResponse), 200)]
    public override async Task<ActionResult<FunctionGroupGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var data = await _mediator.Send(new FunctionGroupGetAllQuery(1), cancellationToken);
        return data != null ? Ok(data) : NotFound();        
    }
}