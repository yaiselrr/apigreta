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
public class AnimalUpdate : EndpointBaseAsync.WithRequest<AnimalUpdateRequest>.WithResult<AnimalUpdateResponse>
{
    private readonly IMediator _mediator;

    public AnimalUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update a Animal entity by Id",
        Description = "Update a Animal entity by Id",
        OperationId = "Animal.Update",
        Tags = new[] { "Animal" })
    ]
    [ProducesResponseType(typeof(AnimalGetByIdResponse), 200)]
    public override async Task<AnimalUpdateResponse> HandleAsync(
        [FromMultiSource] AnimalUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new AnimalUpdateCommand(request.Id, request.EntityDto), cancellationToken);
    }
}