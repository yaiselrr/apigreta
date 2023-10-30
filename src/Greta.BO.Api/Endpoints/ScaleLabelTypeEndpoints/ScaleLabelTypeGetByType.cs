using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.ScaleLabelType;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ScaleLabelTypeEndpoints;

[Route("api/ScaleLabelType")]
public class ScaleLabelTypeGetByType : EndpointBaseAsync.WithRequest<ScaleLabelTypeGetByTypeRequest>.WithActionResult<ScaleLabelTypeGetByTypeResponse>
{
    private readonly IMediator _mediator;

    public ScaleLabelTypeGetByType(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetByType/{type:int}")]
    [SwaggerOperation(
        Summary = "Get list of the scale label type entity by type",
        Description = "Get list of the scale label type entity by type",
        OperationId = "ScaleLabelType.GetByType",
        Tags = new[] { "ScaleLabelType" })
    ]
    [ProducesResponseType(typeof(ScaleLabelTypeGetByTypeResponse), 200)]
    public override async Task<ActionResult<ScaleLabelTypeGetByTypeResponse>> HandleAsync(
        [FromMultiSource] ScaleLabelTypeGetByTypeRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new ScaleLabelTypeGetByTypeQuery(request.type), cancellationToken);
        return data != null ? data : NotFound();
    }
}