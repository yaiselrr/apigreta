using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Grind;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.GrindEndpoints;
[Route("api/Grind")]
public class GrindDelete : EndpointBaseAsync.WithRequest<GrindDeleteRequest>.WithActionResult<GrindDeleteResponse>
{
    private readonly IMediator _mediator;

    public GrindDelete(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete("{entityId}")]
    [SwaggerOperation(
        Summary = "Delete a Grind entity by Id",
        Description = "Delete a Grind entity by Id",
        OperationId = "Grind.Delete",
        Tags = new[] { "Grind" })
    ]
    [ProducesResponseType(typeof(GrindDeleteResponse), 200)]
    public override async Task<ActionResult<GrindDeleteResponse>> HandleAsync(
        [FromMultiSource] GrindDeleteRequest request, CancellationToken cancellationToken = default)
    {
        return request.Id < 1 ? NotFound() : await _mediator.Send(new GrindDeleteCommand(request.Id), cancellationToken);
    }
}