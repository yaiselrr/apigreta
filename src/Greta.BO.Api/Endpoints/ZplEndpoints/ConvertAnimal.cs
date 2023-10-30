using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Zpl;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ZplEndpoints;
[Route("api/Zpl")]
public class ConvertAnimal: EndpointBaseAsync.WithRequest<ConvertAnimalRequest>.WithActionResult<ProcessBatchResponse>
{
    private readonly IMediator _mediator;

    public ConvertAnimal(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("ConvertAnimal/{animalId}/{tagId}")]
    [SwaggerOperation(
        Summary = "Create a zpl code for a animal.",
        Description = "Create a zpl code for a animal.",
        OperationId = "Zpl.ConvertAnimal",
        Tags = new[] { "Zpl" })
    ]
    [ProducesResponseType(typeof(ProcessBatchResponse), 200)]
    public override async Task<ActionResult<ProcessBatchResponse>> HandleAsync( [FromMultiSource] ConvertAnimalRequest request,
        CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new ProcessAnimalToZplCommand(request.AnimalId, request.TagId), cancellationToken));
    }
}