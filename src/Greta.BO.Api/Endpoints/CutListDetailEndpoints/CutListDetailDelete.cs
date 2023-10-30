using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.CutListDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CutListDetailEndpoints;

[Route("api/CutListDetail")]
public class CutListDetailDelete : EndpointBaseAsync.WithRequest<CutListDetailDeleteRequest>.WithActionResult<
    CutListDetailDeleteResponse>
{
    private readonly IMediator _mediator;

    public CutListDetailDelete(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpDelete("{entityId}")]
    [SwaggerOperation(
        Summary = "Delete a Cut list detail by Id",
        Description = "Delete a Cut list detail by Id",
        OperationId = "CutListDetail.Delete",
        Tags = new[] { "CutListDetail" })
    ]
    [ProducesResponseType(typeof(CutListDetailDeleteResponse), 200)]
    public override async Task<ActionResult<CutListDetailDeleteResponse>> HandleAsync(
        [FromMultiSource] CutListDetailDeleteRequest request, CancellationToken cancellationToken = default)
    {
        return request.Id < 1
            ? NotFound()
            : await _mediator.Send(new CutListDetailDeleteCommand(request.Id), cancellationToken);
    }
}