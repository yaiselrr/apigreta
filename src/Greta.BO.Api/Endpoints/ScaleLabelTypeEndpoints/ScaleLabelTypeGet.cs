using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.ScaleLabelType;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ScaleLabelTypeEndpoints;
[Route("api/ScaleLabelType")]
public class ScaleLabelTypeGet: EndpointBaseAsync.WithoutRequest.WithActionResult<ScaleLabelTypeGetAllResponse>
{
    private readonly IMediator _mediator;

    public ScaleLabelTypeGet(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get all scale label type entities",
        Description = "Get all scale label type entities",
        OperationId = "ScaleLabelType.Get",
        Tags = new[] { "ScaleLabelType" })
    ]
    [ProducesResponseType(typeof(ScaleLabelTypeGetAllResponse), 200)]
    public override async Task<ActionResult<ScaleLabelTypeGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new ScaleLabelTypeGetAllQuery(), cancellationToken));
    }
}