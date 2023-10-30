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
public class AnimalValidateForDay: EndpointBaseAsync.WithRequest<AnimalValidateForDayRequest>.WithResult<AnimalValidateForDayResponse>
{
    private readonly IMediator _mediator;

    public AnimalValidateForDay(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("ValidateForDay")]
    [SwaggerOperation(
        Summary = "Get select animal for day",
        Description = "Get select animal for day",
        OperationId = "Animal.ValidateForDay",
        Tags = new[] { "Animal" })
    ]
    [ProducesResponseType(typeof(AnimalGetByIdResponse), 200)]
    public override async Task<AnimalValidateForDayResponse> HandleAsync(
        [FromMultiSource] AnimalValidateForDayRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new AnimalValidateForDayQuery(request.model), cancellationToken);
    }
}