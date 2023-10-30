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
public class AnimalCreate: EndpointBaseAsync.WithRequest<AnimalCreateRequest>.WithResult<AnimalCreateResponse>
{
    private readonly IMediator _mediator;

    public AnimalCreate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Create a new Animal entity",
        Description = "Create a new Animal entity",
        OperationId = "Animal.Create",
        Tags = new[] { "Animal" })
    ]
    [ProducesResponseType(typeof(AnimalGetByIdResponse), 200)]
    public override async Task<AnimalCreateResponse> HandleAsync(
        [FromMultiSource] AnimalCreateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new AnimalCreateCommand(request.EntityDto), cancellationToken);
    }
}