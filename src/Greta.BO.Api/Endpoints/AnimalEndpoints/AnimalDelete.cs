using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Animal;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.AnimalEndpoints;
[Route("api/Animal")]
public class AnimalDelete : EndpointBaseAsync.WithRequest<AnimalDeleteRequest>.WithActionResult<AnimalDeleteResponse>
{
    private readonly IMediator _mediator;

    public AnimalDelete(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete("{entityId}")]
    [SwaggerOperation(
        Summary = "Delete a Animal entity by Id",
        Description = "Delete a Animal entity by Id",
        OperationId = "Animal.Delete",
        Tags = new[] { "Animal" })
    ]
    [ProducesResponseType(typeof(AnimalDeleteResponse), 200)]
    public override async Task<ActionResult<AnimalDeleteResponse>> HandleAsync(
        [FromMultiSource] AnimalDeleteRequest request, CancellationToken cancellationToken = default)
    {
        return request.Id < 1 ? NotFound() : await _mediator.Send(new AnimalDeleteCommand(request.Id), cancellationToken);
    }
}