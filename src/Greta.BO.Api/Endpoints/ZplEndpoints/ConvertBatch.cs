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
public class
    ConvertBatch : EndpointBaseAsync.WithRequest<ConvertBatchRequest>.WithActionResult<ProcessBatchResponse>
{
    private readonly IMediator _mediator;

    public ConvertBatch(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("ConvertBatch/{store}/{batchId}/{labelId}")]
    [SwaggerOperation(
        Summary = "Create a zpl code for a batch.",
        Description = "Create a zpl code for a batch.",
        OperationId = "Zpl.ConvertBatch",
        Tags = new[] { "Zpl" })
    ]
    [ProducesResponseType(typeof(ProcessBatchResponse), 200)]
    public override async Task<ActionResult<ProcessBatchResponse>> HandleAsync( [FromMultiSource] ConvertBatchRequest request,
        CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new ProcessBatchCommand(request.Store, request.BatchId, request.LabelId), cancellationToken));
    }
}