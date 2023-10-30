using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.Fee;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.FeeEndpoints;
[Route("api/Fee")]
public class FeeGet: EndpointBaseAsync.WithoutRequest.WithActionResult<FeeGetAllResponse>
{
    private readonly IMediator _mediator;

    public FeeGet(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get all Fee entities",
        Description = "Get all Fee entities",
        OperationId = "Fee.Get",
        Tags = new[] { "Fee" })
    ]
    [ProducesResponseType(typeof(FeeGetAllResponse), 200)]
    public override async Task<ActionResult<FeeGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new FeeGetAllQuery(), cancellationToken));
    }
}