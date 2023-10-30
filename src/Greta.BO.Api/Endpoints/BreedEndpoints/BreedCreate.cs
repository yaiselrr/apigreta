using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Breed;
using Greta.BO.BusinessLogic.Handlers.Queries.Breed;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.BreedEndpoints;
[Route("api/Breed")]
public class BreedCreate: EndpointBaseAsync.WithRequest<BreedCreateRequest>.WithResult<BreedCreateResponse>
{
    private readonly IMediator _mediator;

    public BreedCreate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Create a new Breed entity",
        Description = "Create a new Breed entity",
        OperationId = "Breed.Create",
        Tags = new[] { "Breed" })
    ]
    [ProducesResponseType(typeof(BreedGetByIdResponse), 200)]
    public override async Task<BreedCreateResponse> HandleAsync(
        [FromMultiSource] BreedCreateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new BreedCreateCommand(request.EntityDto), cancellationToken);
    }
}