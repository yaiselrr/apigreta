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
public class BreedUpdate : EndpointBaseAsync.WithRequest<BreedUpdateRequest>.WithResult<BreedUpdateResponse>
{
    private readonly IMediator _mediator;

    public BreedUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update a Breed entity by Id",
        Description = "Update a Breed entity by Id",
        OperationId = "Breed.Update",
        Tags = new[] { "Breed" })
    ]
    [ProducesResponseType(typeof(BreedGetByIdResponse), 200)]
    public override async Task<BreedUpdateResponse> HandleAsync(
        [FromMultiSource] BreedUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new BreedUpdateCommand(request.Id, request.EntityDto), cancellationToken);
    }
}