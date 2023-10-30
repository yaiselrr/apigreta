using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.VendorOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.VendorOrderEndpoints;
[Route("api/VendorOrder")]
public class VendorOrderGet: EndpointBaseAsync.WithoutRequest.WithActionResult<VendorOrderGetAllResponse>
{
    private readonly IMediator _mediator;

    public VendorOrderGet(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get all vendor order Entities",
        Description = "Get all vendor order Entities",
        OperationId = "VendorOrder.Get",
        Tags = new[] { "VendorOrder" })
    ]
    [ProducesResponseType(typeof(VendorOrderGetAllResponse), 200)]
    public override async Task<ActionResult<VendorOrderGetAllResponse>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new VendorOrderGetAllQuery(), cancellationToken));
    }
}