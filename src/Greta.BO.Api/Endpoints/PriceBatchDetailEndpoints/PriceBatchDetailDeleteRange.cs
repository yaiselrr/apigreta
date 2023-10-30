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
public class PriceBatchDetailDeleteRange: EndpointBaseAsync.WithRequest<PriceBatchDetailDeleteRangeRequest>.WithResult<PriceBatchDetailDeleteRangeResponse>
{
    private readonly IMediator _mediator;

    public PriceBatchDetailDeleteRange(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("DeleteRange")]
    [SwaggerOperation(
        Summary = "Delete list of price batch detail entities",
        Description = "Delete list of price batch detail entities",
        OperationId = "PriceBatchDetail.DeleteRange",
        Tags = new[] { "PriceBatchDetail" })
    ]
    [ProducesResponseType(typeof(PriceBatchDetailDeleteRangeResponse), 200)]
    public override async Task<PriceBatchDetailDeleteRangeResponse> HandleAsync(
        [FromMultiSource] PriceBatchDetailDeleteRangeRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new PriceBatchDetailDeleteRangeCommand(request.Ids), cancellationToken);
    }
}