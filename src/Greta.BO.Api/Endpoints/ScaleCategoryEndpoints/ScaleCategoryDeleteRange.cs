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
public class ScaleCategoryDeleteRange: EndpointBaseAsync.WithRequest<ScaleCategoryDeleteRangeRequest>.WithResult<ScaleCategoryDeleteRangeResponse>
{
    private readonly IMediator _mediator;

    public ScaleCategoryDeleteRange(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("DeleteRange")]
    [SwaggerOperation(
        Summary = "Delete list of the scale category entities",
        Description = "Delete list of the scale category entities",
        OperationId = "ScaleCategory.DeleteRange",
        Tags = new[] { "ScaleCategory" })
    ]
    [ProducesResponseType(typeof(ScaleCategoryDeleteRangeResponse), 200)]
    public override async Task<ScaleCategoryDeleteRangeResponse> HandleAsync(
        [FromMultiSource] ScaleCategoryDeleteRangeRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new ScaleCategoryDeleteRangeCommand(request.Ids), cancellationToken);
    }
}