using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.ExternalScale;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ExternalScaleEndpoints;
[Route("api/ExternalScale")]
public class ExternalScaleDeleteRange: EndpointBaseAsync.WithRequest<ExternalScaleDeleteRangeRequest>.WithResult<ExternalScaleDeleteRangeResponse>
{
    private readonly IMediator _mediator;

    public ExternalScaleDeleteRange(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("DeleteRange")]
    [SwaggerOperation(
        Summary = "Delete list of external scale entities",
        Description = "Delete list of external scale entities",
        OperationId = "ExternalScale.DeleteRange",
        Tags = new[] { "ExternalScale" })
    ]
    [ProducesResponseType(typeof(ExternalScaleDeleteRangeResponse), 200)]
    public override async Task<ExternalScaleDeleteRangeResponse> HandleAsync(
        [FromMultiSource] ExternalScaleDeleteRangeRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new ExternalScaleDeleteRangeCommand(request.Ids), cancellationToken);
    }
}