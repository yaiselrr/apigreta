using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Store;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.StoreEndpoints;

[Route("api/Store")]
public class StoreFilter: EndpointBaseAsync.WithRequest<StoreFilterRequest>.WithActionResult<StoreFilterResponse>
{
    private readonly IMediator _mediator;

    public StoreFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of the store entity",
        Description = "Gets a paginated list of the store entity",
        OperationId = "Store.Filter",
        Tags = new[] { "Store" })
    ]
    [ProducesResponseType(typeof(StoreFilterResponse), 200)]
    public override async Task<ActionResult<StoreFilterResponse>> HandleAsync(
        [FromMultiSource]StoreFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new StoreFilterQuery(request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}