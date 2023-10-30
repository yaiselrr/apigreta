using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Tax;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.TaxEndpoints;

[Route("api/Tax")]
public class TaxGetByStore : EndpointBaseAsync.WithRequest<TaxGetByStoreRequest>.WithActionResult<TaxGetByStoreResponse>
{
    private readonly IMediator _mediator;

    public TaxGetByStore(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetByStore/{storeId:int}")]
    [SwaggerOperation(
        Summary = "Get Entity by store",
        Description = "Get Entity by store",
        OperationId = "Tax.GetByStore",
        Tags = new[] { "Tax" })
    ]
    [ProducesResponseType(typeof(TaxGetByStoreResponse), 200)]
    public override async Task<ActionResult<TaxGetByStoreResponse>> HandleAsync(
        [FromMultiSource] TaxGetByStoreRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new TaxGetByStoreQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}