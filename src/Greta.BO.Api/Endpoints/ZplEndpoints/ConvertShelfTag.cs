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
public class ConvertShelfTag: EndpointBaseAsync.WithRequest<ProcessShelfTagModel>.WithActionResult<ProcessShelfTagsSelectedResponse>
{
    private readonly IMediator _mediator;

    public ConvertShelfTag(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("ConvertShelfTag")]
    [SwaggerOperation(
        Summary = "Create a zpl code for a batch.",
        Description = "Create a zpl code for a batch.",
        OperationId = "Zpl.ConvertShelfTag",
        Tags = new[] { "Zpl" })
    ]
    [ProducesResponseType(typeof(ProcessShelfTagsSelectedResponse), 200)]
    public override async Task<ActionResult<ProcessShelfTagsSelectedResponse>> HandleAsync( [FromBody] ProcessShelfTagModel request,
        CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new ProcessShelfTagsCommand(request), cancellationToken));
    }
}