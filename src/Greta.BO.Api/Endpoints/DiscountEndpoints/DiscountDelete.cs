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
public class DiscountDelete : EndpointBaseAsync.WithRequest<DiscountDeleteRequest>.WithActionResult<DiscountDeleteResponse>
{
    private readonly IMediator _mediator;

    public DiscountDelete(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete("{entityId}")]
    [SwaggerOperation(
        Summary = "Delete a discount entity by Id",
        Description = "Delete a discount entity by Id",
        OperationId = "Discount.Delete",
        Tags = new[] { "Discount" })
    ]
    [ProducesResponseType(typeof(DiscountDeleteResponse), 200)]
    public override async Task<ActionResult<DiscountDeleteResponse>> HandleAsync(
        [FromMultiSource] DiscountDeleteRequest request, CancellationToken cancellationToken = default)
    {
        return request.Id < 1 ? NotFound() : await _mediator.Send(new DiscountDeleteCommand(request.Id), cancellationToken);
    }
}