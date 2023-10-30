using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.ScaleCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ScaleCategoryEndpoints;
[Route("api/ScaleCategory")]
public class ScaleCategoryDelete : EndpointBaseAsync.WithRequest<ScaleCategoryDeleteRequest>.WithActionResult<ScaleCategoryDeleteResponse>
{
    private readonly IMediator _mediator;

    public ScaleCategoryDelete(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete("{entityId}")]
    [SwaggerOperation(
        Summary = "Delete a scale category entity by Id",
        Description = "Delete a scale category entity by Id",
        OperationId = "ScaleCategory.Delete",
        Tags = new[] { "ScaleCategory" })
    ]
    [ProducesResponseType(typeof(ScaleCategoryDeleteResponse), 200)]
    public override async Task<ActionResult<ScaleCategoryDeleteResponse>> HandleAsync(
        [FromMultiSource] ScaleCategoryDeleteRequest request, CancellationToken cancellationToken = default)
    {
        return request.Id < 1 ? NotFound() : await _mediator.Send(new ScaleCategoryDeleteCommand(request.Id), cancellationToken);
    }
}