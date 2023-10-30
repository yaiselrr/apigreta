using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Region;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.RegionEndpoints;
[Route("api/Region")]
public class RegionDelete : EndpointBaseAsync.WithRequest<RegionDeleteRequest>.WithActionResult<RegionDeleteResponse>
{
    private readonly IMediator _mediator;

    public RegionDelete(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete("{entityId}")]
    [SwaggerOperation(
        Summary = "Delete a region entity by Id",
        Description = "Delete a region entity by Id",
        OperationId = "Region.Delete",
        Tags = new[] { "Region" })
    ]
    [ProducesResponseType(typeof(RegionDeleteResponse), 200)]
    public override async Task<ActionResult<RegionDeleteResponse>> HandleAsync(
        [FromMultiSource] RegionDeleteRequest request, CancellationToken cancellationToken = default)
    {
        return request.Id < 1 ? NotFound() : await _mediator.Send(new RegionDeleteCommand(request.Id), cancellationToken);
    }
}