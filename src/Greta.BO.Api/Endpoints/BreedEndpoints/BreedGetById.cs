using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Breed;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.BreedEndpoints;

[Route("api/Breed")]
public class BreedGetById : EndpointBaseAsync.WithRequest<BreedGetByIdRequest>.WithActionResult<BreedGetByIdResponse>
{
    private readonly IMediator _mediator;

    public BreedGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get Breed entity by id",
        Description = "Get Breed entity by id",
        OperationId = "Breed.GetById",
        Tags = new[] { "Breed" })
    ]
    [ProducesResponseType(typeof(BreedGetByIdResponse), 200)]
    public override async Task<ActionResult<BreedGetByIdResponse>> HandleAsync(
        [FromMultiSource] BreedGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new BreedGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}