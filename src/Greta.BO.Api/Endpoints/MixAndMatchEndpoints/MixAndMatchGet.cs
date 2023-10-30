using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.MixAndMatch;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.MixAndMatchEndpoints;
[Route("api/MixAndMatch")]
public class MixAndMatchGet : EndpointBaseAsync.WithoutRequest.WithActionResult<MixAndMatchGetAllResponse>
{
    private readonly IMediator _mediator;

    public MixAndMatchGet(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get all MixAndMatch Entities",
        Description = "Get all MixAndMatch Entities",
        OperationId = "MixAndMatch.Get",
        Tags = new[] { "MixAndMatch" })
    ]
    [ProducesResponseType(typeof(MixAndMatchGetAllResponse), 200)]
    public override async Task<ActionResult<MixAndMatchGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new MixAndMatchGetAllQuery(), cancellationToken));
    }
}