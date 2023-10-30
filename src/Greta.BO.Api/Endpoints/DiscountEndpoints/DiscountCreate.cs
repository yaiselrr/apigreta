using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Discount;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.DiscountEndpoints;
[Route("api/Discount")]
public class DiscountCreate : EndpointBaseAsync.WithRequest<DiscountCreateRequest>.WithActionResult<DiscountCreateResponse>
{
    private readonly IMediator _mediator;

    public DiscountCreate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Create a new discount",
        Description = "Create a new discount",
        OperationId = "Discount.Create",
        Tags = new[] { "Discount" })
    ]
    [ProducesResponseType(typeof(DiscountCreateResponse), 201)]
    public override async Task<ActionResult<DiscountCreateResponse>> HandleAsync(
        [FromMultiSource] DiscountCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new DiscountCreateCommand(request.EntityDto), cancellationToken);
        return result.Data ? Created(string.Empty, result): BadRequest();
    }
}