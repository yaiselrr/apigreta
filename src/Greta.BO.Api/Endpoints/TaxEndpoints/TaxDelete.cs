using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Tax;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.TaxEndpoints;
[Route("api/Tax")]
public class TaxDelete : EndpointBaseAsync.WithRequest<TaxDeleteRequest>.WithActionResult<TaxDeleteResponse>
{
    private readonly IMediator _mediator;

    public TaxDelete(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete("{entityId}")]
    [SwaggerOperation(
        Summary = "Delete a Tax by Id",
        Description = "Delete a Tax by Id",
        OperationId = "Tax.Delete",
        Tags = new[] { "Tax" })
    ]
    [ProducesResponseType(typeof(TaxDeleteResponse), 200)]
    public override async Task<ActionResult<TaxDeleteResponse>> HandleAsync(
        [FromMultiSource] TaxDeleteRequest request, CancellationToken cancellationToken = default)
    {
        return request.Id < 1 ? NotFound() : await _mediator.Send(new TaxDeleteCommand(request.Id), cancellationToken);
    }
}