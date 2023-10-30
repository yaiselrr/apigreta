using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.Grind;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.GrindEndpoints;
[Route("api/Grind")]
public class GrindGet: EndpointBaseAsync.WithoutRequest.WithActionResult<GrindGetAllResponse>
{
    private readonly IMediator _mediator;

    public GrindGet(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get all Grind entities",
        Description = "Get all Grind entities",
        OperationId = "Grind.Get",
        Tags = new[] { "Grind" })
    ]
    [ProducesResponseType(typeof(GrindGetAllResponse), 200)]
    public override async Task<ActionResult<GrindGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new GrindGetAllQuery(), cancellationToken));
    }
}