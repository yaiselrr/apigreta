using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.VendorOrder;
using Greta.BO.BusinessLogic.Handlers.Queries.VendorOrderDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.VendorOrderDetailEndpoints;

[Route("api/VendorOrderDetail")]
public class VendorOrderDetailGet : EndpointBaseAsync.WithoutRequest.WithActionResult<VendorOrderDetailGetAllResponse>
{
    private readonly IMediator _mediator;

    public VendorOrderDetailGet(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get all vendor order detail Entities",
        Description = "Get all vendor order Entities detail",
        OperationId = "VendorOrderDetail.Get",
        Tags = new[] { "VendorOrderDetail" })
    ]
    [ProducesResponseType(typeof(VendorOrderDetailGetAllResponse), 200)]
    public override async Task<ActionResult<VendorOrderDetailGetAllResponse>> HandleAsync(
        CancellationToken cancellationToken = new CancellationToken())
    {
        return Ok(await _mediator.Send(new VendorOrderGetAllQuery(), cancellationToken));
    }
}