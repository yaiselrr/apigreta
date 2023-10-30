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
public class TaxDeleteRange: EndpointBaseAsync.WithRequest<TaxDeleteRangeRequest>.WithResult<TaxDeleteRangeResponse>
{
    private readonly IMediator _mediator;

    public TaxDeleteRange(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("DeleteRange")]
    [SwaggerOperation(
        Summary = "Delete list of Tax",
        Description = "Delete list of Tax",
        OperationId = "Tax.DeleteRange",
        Tags = new[] { "Tax" })
    ]
    [ProducesResponseType(typeof(TaxDeleteRangeResponse), 200)]
    public override async Task<TaxDeleteRangeResponse> HandleAsync(
        [FromMultiSource] TaxDeleteRangeRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new TaxDeleteRangeCommand(request.Ids), cancellationToken);
    }
}