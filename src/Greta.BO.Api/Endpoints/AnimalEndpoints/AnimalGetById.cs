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
public class AnimalGetById : EndpointBaseAsync.WithRequest<AnimalGetByIdRequest>.WithActionResult<AnimalGetByIdResponse>
{
    private readonly IMediator _mediator;

    public AnimalGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get Animal entity by id",
        Description = "Get Animal entity by id",
        OperationId = "Animal.GetById",
        Tags = new[] { "Animal" })
    ]
    [ProducesResponseType(typeof(AnimalGetByIdResponse), 200)]
    public override async Task<ActionResult<AnimalGetByIdResponse>> HandleAsync(
        [FromMultiSource] AnimalGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new AnimalGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}