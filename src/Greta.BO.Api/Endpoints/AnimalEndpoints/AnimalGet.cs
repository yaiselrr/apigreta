using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.Animal;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.AnimalEndpoints;
[Route("api/Animal")]
public class AnimalGet: EndpointBaseAsync.WithoutRequest.WithActionResult<AnimalGetAllResponse>
{
    private readonly IMediator _mediator;

    public AnimalGet(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get all Animal entities",
        Description = "Get all Animal entities",
        OperationId = "Animal.Get",
        Tags = new[] { "Animal" })
    ]
    [ProducesResponseType(typeof(AnimalGetAllResponse), 200)]
    public override async Task<ActionResult<AnimalGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new AnimalGetAllQuery(), cancellationToken));
    }
}