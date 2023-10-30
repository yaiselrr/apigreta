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
public class ScaleLabelTypeDelete : EndpointBaseAsync.WithRequest<ScaleLabelTypeDeleteRequest>.WithActionResult<ScaleLabelTypeDeleteResponse>
{
    private readonly IMediator _mediator;

    public ScaleLabelTypeDelete(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete("{entityId}")]
    [SwaggerOperation(
        Summary = "Delete a scale label type entity by Id",
        Description = "Delete a scale label type entity by Id",
        OperationId = "ScaleLabelType.Delete",
        Tags = new[] { "ScaleLabelType" })
    ]
    [ProducesResponseType(typeof(ScaleLabelTypeDeleteResponse), 200)]
    public override async Task<ActionResult<ScaleLabelTypeDeleteResponse>> HandleAsync(
        [FromMultiSource] ScaleLabelTypeDeleteRequest request, CancellationToken cancellationToken = default)
    {
        return request.Id < 1 ? NotFound() : await _mediator.Send(new ScaleLabelTypeDeleteCommand(request.Id), cancellationToken);
    }
}