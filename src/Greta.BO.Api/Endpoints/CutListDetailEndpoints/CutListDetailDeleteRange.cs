using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.CutListDetail;
using Greta.BO.BusinessLogic.Handlers.Command.VendorOrderDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CutListDetailEndpoints;

[Route("api/CutListDetail")]
public class CutListDetailDeleteRange : EndpointBaseAsync.WithRequest<CutListDetailDeleteRangeRequest>.
    WithResult<CutListDetailDeleteRangeResponse>
{
    private readonly IMediator _mediator;

    public CutListDetailDeleteRange(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("DeleteRange")]
    [SwaggerOperation(
        Summary = "Delete list of Cut list detail",
        Description = "Delete list of Cut list detail",
        OperationId = "CutListDetail.DeleteRange",
        Tags = new[] { "CutListDetail" })
    ]
    [ProducesResponseType(typeof(CutListDetailDeleteRangeResponse), 200)]
    public override async Task<CutListDetailDeleteRangeResponse> HandleAsync(
        [FromMultiSource] CutListDetailDeleteRangeRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new CutListDetailDeleteRangeCommand(request.Ids), cancellationToken);
    }
}