using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.Api.Endpoints.VendorOrderDetailEndpoints;
using Greta.BO.BusinessLogic.Handlers.Queries.VendorOrderDetail;
using Greta.BO.BusinessLogic.Handlers.Queries.VendorOrderDetailCredit;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.VendorOrderDetailCreditEndpoints;

[Route("api/VendorOrderDetailCredit")]
public class VendorOrderDetailCreditFilter : EndpointBaseAsync.WithRequest<VendorOrderDetailCreditFilterRequest>.WithActionResult<
    VendorOrderDetailCreditFilterResponse>
{
    private readonly IMediator _mediator;

    public VendorOrderDetailCreditFilter(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Filter")]
    [SwaggerOperation(
        Summary = "Gets a list of vendor order detail entity",
        Description = "Gets a list of vendor order detail entity",
        OperationId = "VendorOrderDetail.Filter",
        Tags = new[] { "VendorOrderDetail" })
    ]
    [ProducesResponseType(typeof(VendorOrderDetailCreditFilterResponse), 200)]
    public override async Task<ActionResult<VendorOrderDetailCreditFilterResponse>> HandleAsync(
        [FromMultiSource] VendorOrderDetailCreditFilterRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new VendorOrderDetailCreditFilterQuery(request.Filter),
            cancellationToken);
    }
}