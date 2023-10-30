using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.ShelfTag;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ShelfTagEndpoints;
[Route("api/ShelfTag")]
public class ShelfTagGet: EndpointBaseAsync.WithoutRequest.WithActionResult<ShelfTagGetAllResponse>
{
    private readonly IMediator _mediator;

    public ShelfTagGet(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get all ShelfTag entities",
        Description = "Get all ShelfTag entities",
        OperationId = "ShelfTag.Get",
        Tags = new[] { "ShelfTag" })
    ]
    [ProducesResponseType(typeof(ShelfTagGetAllResponse), 200)]
    public override async Task<ActionResult<ShelfTagGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new ShelfTagGetAllQuery(), cancellationToken));
    }
}