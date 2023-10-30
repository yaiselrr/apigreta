using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.ScaleLabelType;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ScaleLabelTypeEndpoints;
[Route("api/ScaleLabelType")]
public class ScaleLabelTypeDeleteRange: EndpointBaseAsync.WithRequest<ScaleLabelTypeDeleteRangeRequest>.WithResult<ScaleLabelTypeDeleteRangeResponse>
{
    private readonly IMediator _mediator;

    public ScaleLabelTypeDeleteRange(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("DeleteRange")]
    [SwaggerOperation(
        Summary = "Delete list of the scale label type entities",
        Description = "Delete list of the scale label type entities",
        OperationId = "ScaleLabelType.DeleteRange",
        Tags = new[] { "ScaleLabelType" })
    ]
    [ProducesResponseType(typeof(ScaleLabelTypeDeleteRangeResponse), 200)]
    public override async Task<ScaleLabelTypeDeleteRangeResponse> HandleAsync(
        [FromMultiSource] ScaleLabelTypeDeleteRangeRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new ScaleLabelTypeDeleteRangeCommand(request.Ids), cancellationToken);
    }
}