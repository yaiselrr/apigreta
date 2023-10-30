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
public class BreedDeleteRange: EndpointBaseAsync.WithRequest<BreedDeleteRangeRequest>.WithResult<BreedDeleteRangeResponse>
{
    private readonly IMediator _mediator;

    public BreedDeleteRange(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("DeleteRange")]
    [SwaggerOperation(
        Summary = "Delete list of Breed entities",
        Description = "Delete list of Breed entities",
        OperationId = "Breed.DeleteRange",
        Tags = new[] { "Breed" })
    ]
    [ProducesResponseType(typeof(BreedDeleteRangeResponse), 200)]
    public override async Task<BreedDeleteRangeResponse> HandleAsync(
        [FromMultiSource] BreedDeleteRangeRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new BreedDeleteRangeCommand(request.Ids), cancellationToken);
    }
}