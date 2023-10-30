using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Family;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.FamilyEndpoints;
[Route("api/Family")]
public class FamilyDelete : EndpointBaseAsync.WithRequest<FamilyDeleteRequest>.WithActionResult<FamilyDeleteResponse>
{
    private readonly IMediator _mediator;

    public FamilyDelete(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete("{entityId}")]
    [SwaggerOperation(
        Summary = "Delete a Family by Id",
        Description = "Delete a Family by Id",
        OperationId = "Family.Delete",
        Tags = new[] { "Family" })
    ]
    [ProducesResponseType(typeof(FamilyDeleteResponse), 200)]
    public override async Task<ActionResult<FamilyDeleteResponse>> HandleAsync(
        [FromMultiSource] FamilyDeleteRequest request, CancellationToken cancellationToken = default)
    {
        return request.Id < 1 ? NotFound() : await _mediator.Send(new FamilyDeleteCommand(request.Id), cancellationToken);
    }
}