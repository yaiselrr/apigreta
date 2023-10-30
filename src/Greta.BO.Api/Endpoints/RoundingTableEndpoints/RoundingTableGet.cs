using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.RoundingTableQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.RoundingTableEndpoints;
[Route("api/RoundingTable")]
public class RoundingTableGet: EndpointBaseAsync.WithoutRequest.WithActionResult<RoundingTableGetAllResponse>
{
    private readonly IMediator _mediator;

    public RoundingTableGet(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get all Rounding  Table Entities",
        Description = "Get all Rounding Table Entities",
        OperationId = "RoundingTable.Get",
        Tags = new[] { "RoundingTable" })
    ]
    [ProducesResponseType(typeof(RoundingTableGetAllResponse), 200)]
    public override async Task<ActionResult<RoundingTableGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new RoundingTableGetAllQuery(), cancellationToken));
    }
}