using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Fee;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.FeeEndpoints;
[Route("api/Fee")]
public class FeeDelete : EndpointBaseAsync.WithRequest<FeeDeleteRequest>.WithActionResult<FeeDeleteResponse>
{
    private readonly IMediator _mediator;

    public FeeDelete(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete("{entityId}")]
    [SwaggerOperation(
        Summary = "Delete a Fee by Id",
        Description = "Delete a Fee by Id",
        OperationId = "Fee.Delete",
        Tags = new[] { "Fee" })
    ]
    [ProducesResponseType(typeof(FeeDeleteResponse), 200)]
    public override async Task<ActionResult<FeeDeleteResponse>> HandleAsync(
        [FromMultiSource] FeeDeleteRequest request, CancellationToken cancellationToken = default)
    {
        return request.Id < 1 ? NotFound() : await _mediator.Send(new FeeDeleteCommand(request.Id), cancellationToken);
    }
}