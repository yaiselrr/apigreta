using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.ExternalScale;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ExternalScaleEndpoints;
[Route("api/ExternalScale")]
public class ExternalScaleSendDataToScale : EndpointBaseAsync.WithRequest<ExternalScaleSendDataToScaleRequest>.WithActionResult<SendDataToExternalScaleResponse>
{
    private readonly IMediator _mediator;

    public ExternalScaleSendDataToScale(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("SendDataToScale/{store}/{department}/{type}/{partial}")]
    [SwaggerOperation(
        Summary = "Send Data To Scale State of entity",
        Description = "Send Data To Scale",
        OperationId = "ExternalScale.SendDataToScale",
        Tags = new[] { "ExternalScale" })
    ]
    [ProducesResponseType(typeof(SendDataToExternalScaleResponse), 200)]
    public override async Task<ActionResult<SendDataToExternalScaleResponse>> HandleAsync(
        [FromRoute] ExternalScaleSendDataToScaleRequest request,
        CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new SendDataToExternalScaleCommand(request.Store, request.Department, request.Type, request.Partial), cancellationToken));
    }
}