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
public class RegionDeleteRange: EndpointBaseAsync.WithRequest<RegionDeleteRangeRequest>.WithResult<RegionDeleteRangeResponse>
{
    private readonly IMediator _mediator;

    public RegionDeleteRange(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("DeleteRange")]
    [SwaggerOperation(
        Summary = "Delete list of region entities",
        Description = "Delete list of region entities",
        OperationId = "Region.DeleteRange",
        Tags = new[] { "Region" })
    ]
    [ProducesResponseType(typeof(RegionDeleteRangeResponse), 200)]
    public override async Task<RegionDeleteRangeResponse> HandleAsync(
        [FromMultiSource] RegionDeleteRangeRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new RegionDeleteRangeCommand(request.Ids), cancellationToken);
    }
}