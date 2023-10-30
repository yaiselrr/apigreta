using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.Scalendar;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ScalendarEndpoints;
[Route("api/Scalendar")]
public class ScalendarGet: EndpointBaseAsync.WithoutRequest.WithActionResult<ScalendarGetAllResponse>
{
    private readonly IMediator _mediator;

    public ScalendarGet(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get all Scalendar entities",
        Description = "Get all Scalendar entities",
        OperationId = "Scalendar.Get",
        Tags = new[] { "Scalendar" })
    ]
    [ProducesResponseType(typeof(ScalendarGetAllResponse), 200)]
    public override async Task<ActionResult<ScalendarGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new ScalendarGetAllQuery(), cancellationToken));
    }
}