using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Zpl;
using Greta.BO.BusinessLogic.Models.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ZplEndpoints;
[Route("api/Zpl")]
public class ConvertShelfTagSelected : EndpointBaseAsync.WithRequest<ProcessShelfTagSelectedModel>.WithActionResult<
    ProcessShelfTagsSelectedResponse>
{
    private readonly IMediator _mediator;

    public ConvertShelfTagSelected(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("ConvertShelfTagSelected")]
    [SwaggerOperation(
        Summary = "Create a zpl code for a batch selected.",
        Description = "Create a zpl code for a batch selected.",
        OperationId = "Zpl.ConvertShelfTagSelected",
        Tags = new[] { "Zpl" })
    ]
    [ProducesResponseType(typeof(ProcessShelfTagsSelectedResponse), 200)]
    public override async Task<ActionResult<ProcessShelfTagsSelectedResponse>> HandleAsync(
        [FromBody] ProcessShelfTagSelectedModel request,
        CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new ProcessShelfTagsSelectedCommand(request), cancellationToken));
    }
}