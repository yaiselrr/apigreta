using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.PriceBatchDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.PriceBatchDetailEndpoints;
[Route("api/PriceBatchDetail")]
public class PriceBatchDetailDelete : EndpointBaseAsync.WithRequest<PriceBatchDetailDeleteRequest>.WithActionResult<PriceBatchDetailDeleteResponse>
{
    private readonly IMediator _mediator;

    public PriceBatchDetailDelete(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete("{entityId}")]
    [SwaggerOperation(
        Summary = "Delete a price batch detail entity by Id",
        Description = "Delete a price batch detail entity by Id",
        OperationId = "PriceBatchDetail.Delete",
        Tags = new[] { "PriceBatchDetail" })
    ]
    [ProducesResponseType(typeof(PriceBatchDetailDeleteResponse), 200)]
    public override async Task<ActionResult<PriceBatchDetailDeleteResponse>> HandleAsync(
        [FromMultiSource] PriceBatchDetailDeleteRequest request, CancellationToken cancellationToken = default)
    {
        return request.Id < 1 ? NotFound() : await _mediator.Send(new PriceBatchDetailDeleteCommand(request.Id), cancellationToken);
    }
}