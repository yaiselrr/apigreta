using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Animal;
using Greta.BO.BusinessLogic.Handlers.Queries.Animal;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.AnimalEndpoints;
[Route("api/Animal")]
public class AnimalGetSelectAnimalForDay: EndpointBaseAsync.WithRequest<AnimalGetSelectAnimalForDayRequest>.WithResult<AnimalGetSelectAnimalForDayResponse>
{
    private readonly IMediator _mediator;

    public AnimalGetSelectAnimalForDay(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("GetSelectAnimalForDay")]
    [SwaggerOperation(
        Summary = "Get select animal for day",
        Description = "Get select animal for day",
        OperationId = "Animal.GetSelectAnimalForDay",
        Tags = new[] { "Animal" })
    ]
    [ProducesResponseType(typeof(AnimalGetByIdResponse), 200)]
    public override async Task<AnimalGetSelectAnimalForDayResponse> HandleAsync(
        [FromMultiSource] AnimalGetSelectAnimalForDayRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new AnimalGetSelectAnimalForDayQuery(request.model), cancellationToken);
    }
}