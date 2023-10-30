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
public class ExternalScaleDelete : EndpointBaseAsync.WithRequest<ExternalScaleDeleteRequest>.WithActionResult<ExternalScaleDeleteResponse>
{
    private readonly IMediator _mediator;

    public ExternalScaleDelete(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete("{entityId}")]
    [SwaggerOperation(
        Summary = "Delete a external scale entity by Id",
        Description = "Delete a external scale entity by Id",
        OperationId = "ExternalScale.Delete",
        Tags = new[] { "ExternalScale" })
    ]
    [ProducesResponseType(typeof(ExternalScaleDeleteResponse), 200)]
    public override async Task<ActionResult<ExternalScaleDeleteResponse>> HandleAsync(
        [FromMultiSource] ExternalScaleDeleteRequest request, CancellationToken cancellationToken = default)
    {
        return request.Id < 1 ? NotFound() : await _mediator.Send(new ExternalScaleDeleteCommand(request.Id), cancellationToken);
    }
}