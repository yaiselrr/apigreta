using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Breed;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.BreedEndpoints;
[Route("api/Breed")]
public class BreedDelete : EndpointBaseAsync.WithRequest<BreedDeleteRequest>.WithActionResult<BreedDeleteResponse>
{
    private readonly IMediator _mediator;

    public BreedDelete(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete("{entityId}")]
    [SwaggerOperation(
        Summary = "Delete a Breed entity by Id",
        Description = "Delete a Breed entity by Id",
        OperationId = "Breed.Delete",
        Tags = new[] { "Breed" })
    ]
    [ProducesResponseType(typeof(BreedDeleteResponse), 200)]
    public override async Task<ActionResult<BreedDeleteResponse>> HandleAsync(
        [FromMultiSource] BreedDeleteRequest request, CancellationToken cancellationToken = default)
    {
        return request.Id < 1 ? NotFound() : await _mediator.Send(new BreedDeleteCommand(request.Id), cancellationToken);
    }
}