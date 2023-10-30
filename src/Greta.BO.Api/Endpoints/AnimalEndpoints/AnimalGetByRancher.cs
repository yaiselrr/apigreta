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
public class AnimalGetByRancher : EndpointBaseAsync.WithRequest<AnimalGetByRancherRequest>.WithActionResult<AnimalGetByRancherResponse>
{
    private readonly IMediator _mediator;

    public AnimalGetByRancher(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetByRancher/{rancherId:int}")]
    [SwaggerOperation(
        Summary = "Get Entity by rancher",
        Description = "Get Entity by rancher",
        OperationId = "Animal.GetByRancher",
        Tags = new[] { "Animal" })
    ]
    [ProducesResponseType(typeof(AnimalGetByRancherResponse), 200)]
    public override async Task<ActionResult<AnimalGetByRancherResponse>> HandleAsync(
        [FromMultiSource] AnimalGetByRancherRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new AnimalGetByRancherQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}