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
public class MixAndMatchDelete : EndpointBaseAsync.WithRequest<MixAndMatchDeleteRequest>.WithActionResult<MixAndMatchDeleteResponse>
{
    private readonly IMediator _mediator;

    public MixAndMatchDelete(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete("{entityId}")]
    [SwaggerOperation(
        Summary = "Delete a MixAndMatch by Id",
        Description = "Delete a MixAndMatch by Id",
        OperationId = "MixAndMatch.Delete",
        Tags = new[] { "MixAndMatch" })
    ]
    [ProducesResponseType(typeof(MixAndMatchDeleteResponse), 200)]
    public override async Task<ActionResult<MixAndMatchDeleteResponse>> HandleAsync(
        [FromMultiSource] MixAndMatchDeleteRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new MixAndMatchDeleteCommand(request.Id), cancellationToken);
    }
}