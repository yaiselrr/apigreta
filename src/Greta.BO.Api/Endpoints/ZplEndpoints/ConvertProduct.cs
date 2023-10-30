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
public class ConvertProduct : EndpointBaseAsync.WithRequest<ProductToZplModel>.WithActionResult<ProcessProductToZplResponse>
{
    private readonly IMediator _mediator;

    public ConvertProduct(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("ConvertProduct")]
    [SwaggerOperation(
        Summary = "Create a zpl code for a product.",
        Description = "Create a zpl code for a product.",
        OperationId = "Zpl.ConvertProduct",
        Tags = new[] { "Zpl" })
    ]
    [ProducesResponseType(typeof(ProcessProductToZplResponse), 200)]
    public override async Task<ActionResult<ProcessProductToZplResponse>> HandleAsync(
        [FromBody] ProductToZplModel request,
        CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new ProcessProductToZplCommand(request), cancellationToken));
    }
}