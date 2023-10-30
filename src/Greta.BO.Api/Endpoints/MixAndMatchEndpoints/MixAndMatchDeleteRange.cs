using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.MixAndMatch;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.MixAndMatchEndpoints;
[Route("api/MixAndMatch")]
public class MixAndMatchDeleteRange: EndpointBaseAsync.WithRequest<MixAndMatchDeleteRangeRequest>.WithResult<MixAndMatchDeleteRangeResponse>
{
    private readonly IMediator _mediator;

    public MixAndMatchDeleteRange(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("DeleteRange")]
    [SwaggerOperation(
        Summary = "Delete list of MixAndMatch",
        Description = "Delete list of MixAndMatch",
        OperationId = "MixAndMatch.DeleteRange",
        Tags = new[] { "MixAndMatch" })
    ]
    [ProducesResponseType(typeof(MixAndMatchDeleteRangeResponse), 200)]
    public override async Task<MixAndMatchDeleteRangeResponse> HandleAsync(
        [FromMultiSource] MixAndMatchDeleteRangeRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new MixAndMatchDeleteRangeCommand(request.Ids), cancellationToken);
    }
}