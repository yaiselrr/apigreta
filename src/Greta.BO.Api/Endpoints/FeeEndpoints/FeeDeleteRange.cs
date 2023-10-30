using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Fee;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.FeeEndpoints;
[Route("api/Fee")]
public class FeeDeleteRange: EndpointBaseAsync.WithRequest<FeeDeleteRangeRequest>.WithResult<FeeDeleteRangeResponse>
{
    private readonly IMediator _mediator;

    public FeeDeleteRange(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("DeleteRange")]
    [SwaggerOperation(
        Summary = "Delete list of Fee",
        Description = "Delete list of Fee",
        OperationId = "Fee.DeleteRange",
        Tags = new[] { "Fee" })
    ]
    [ProducesResponseType(typeof(FeeDeleteRangeResponse), 200)]
    public override async Task<FeeDeleteRangeResponse> HandleAsync(
        [FromMultiSource] FeeDeleteRangeRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new FeeDeleteRangeCommand(request.Ids), cancellationToken);
    }
}