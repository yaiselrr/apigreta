using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.VendorOrder;
using Greta.BO.BusinessLogic.Handlers.Queries.VendorOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.VendorOrderEndpoints;
[Route("api/VendorOrder")]
public class VendorOrderCreate: EndpointBaseAsync.WithRequest<VendorOrderCreateRequest>.WithResult<VendorOrderCreateResponse>
{
    private readonly IMediator _mediator;

    public VendorOrderCreate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Create a new Vendor Order",
        Description = "Create a new Vendor Order",
        OperationId = "VendorOrder.Create",
        Tags = new[] { "VendorOrder" })
    ]
    [ProducesResponseType(typeof(VendorOrderGetByIdResponse), 200)]
    public override async Task<VendorOrderCreateResponse> HandleAsync(
        [FromMultiSource] VendorOrderCreateRequest request, 
        CancellationToken cancellationToken = default)
    {
        if (request.EntityDto != null && request.EntityDto.IsDirectStoreDelivery)
            request.EntityDto.OrderedDate = request.EntityDto.ReceivedDate;
        return await _mediator.Send(new VendorOrderCreateCommand(request.EntityDto), cancellationToken);
    }
}