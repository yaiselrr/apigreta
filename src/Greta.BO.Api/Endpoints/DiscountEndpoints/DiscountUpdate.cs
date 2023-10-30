using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Discount;
using Greta.BO.BusinessLogic.Handlers.Queries.Discount;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.DiscountEndpoints;
[Route("api/Discount")]
public class DiscountUpdate: EndpointBaseAsync.WithRequest<DiscountUpdateRequest>.WithResult<DiscountUpdateResponse>
{
    private readonly IMediator _mediator;

    public DiscountUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update a discount by Id",
        Description = "Update a discount by Id",
        OperationId = "Discount.Update",
        Tags = new[] { "Discount" })
    ]
    [ProducesResponseType(typeof(DiscountGetByIdResponse), 200)]
    public override async Task<DiscountUpdateResponse> HandleAsync(
        [FromMultiSource] DiscountUpdateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new DiscountUpdateCommand(request.Id, request.EntityDto), cancellationToken);
    }
}