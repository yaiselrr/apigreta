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
public class AnimalDeleteRange: EndpointBaseAsync.WithRequest<AnimalDeleteRangeRequest>.WithResult<AnimalDeleteRangeResponse>
{
    private readonly IMediator _mediator;

    public AnimalDeleteRange(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("DeleteRange")]
    [SwaggerOperation(
        Summary = "Delete list of Animal entities",
        Description = "Delete list of Animal entities",
        OperationId = "Animal.DeleteRange",
        Tags = new[] { "Animal" })
    ]
    [ProducesResponseType(typeof(AnimalDeleteRangeResponse), 200)]
    public override async Task<AnimalDeleteRangeResponse> HandleAsync(
        [FromMultiSource] AnimalDeleteRangeRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new AnimalDeleteRangeCommand(request.Ids), cancellationToken);
    }
}