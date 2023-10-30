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
public class DiscountDeleteRange: EndpointBaseAsync.WithRequest<DiscountDeleteRangeRequest>.WithResult<DiscountDeleteRangeResponse>
{
    private readonly IMediator _mediator;

    public DiscountDeleteRange(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("DeleteRange")]
    [SwaggerOperation(
        Summary = "Delete list of discount entities",
        Description = "Delete list of discount entities",
        OperationId = "Discount.DeleteRange",
        Tags = new[] { "Discount" })
    ]
    [ProducesResponseType(typeof(DiscountDeleteRangeResponse), 200)]
    public override async Task<DiscountDeleteRangeResponse> HandleAsync(
        [FromMultiSource] DiscountDeleteRangeRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new DiscountDeleteRangeCommand(request.Ids), cancellationToken);
    }
}