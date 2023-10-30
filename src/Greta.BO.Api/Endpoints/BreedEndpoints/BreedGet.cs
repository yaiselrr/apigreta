using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.Breed;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.BreedEndpoints;
[Route("api/Breed")]
public class BreedGet: EndpointBaseAsync.WithoutRequest.WithActionResult<BreedGetAllResponse>
{
    private readonly IMediator _mediator;

    public BreedGet(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get all Breed entities",
        Description = "Get all Breed entities",
        OperationId = "Breed.Get",
        Tags = new[] { "Breed" })
    ]
    [ProducesResponseType(typeof(BreedGetAllResponse), 200)]
    public override async Task<ActionResult<BreedGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new BreedGetAllQuery(), cancellationToken));
    }
}