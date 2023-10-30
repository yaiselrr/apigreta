using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Animal;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.AnimalEndpoints;

[Route("api/Animal")]
public class AnimalGetByBreed : EndpointBaseAsync.WithRequest<AnimalGetByBreedRequest>.WithActionResult<AnimalGetByBreedResponse>
{
    private readonly IMediator _mediator;

    public AnimalGetByBreed(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetByBreed/{breedId:int}")]
    [SwaggerOperation(
        Summary = "Get Entity by breed",
        Description = "Get Entity by breed",
        OperationId = "Animal.GetByBreed",
        Tags = new[] { "Animal" })
    ]
    [ProducesResponseType(typeof(AnimalGetByBreedResponse), 200)]
    public override async Task<ActionResult<AnimalGetByBreedResponse>> HandleAsync(
        [FromMultiSource] AnimalGetByBreedRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new AnimalGetByBreedQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}