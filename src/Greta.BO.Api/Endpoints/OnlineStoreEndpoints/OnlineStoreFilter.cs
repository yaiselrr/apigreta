using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.OnlineStore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.OnlineStoreEndpoints;

[Route("api/OnlineStore")]
public class OnlineStoreFilter: EndpointBaseAsync.WithRequest<OnlineStoreFilterRequest>.WithActionResult<OnlineStoreFilterResponse>
{
    private readonly IMediator _mediator;

    public OnlineStoreFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of the OnlineStore entity",
        Description = "Gets a paginated list of the OnlineStore entity",
        OperationId = "OnlineStore.Filter",
        Tags = new[] { "OnlineStore" })
    ]
    [ProducesResponseType(typeof(OnlineStoreFilterResponse), 200)]
    public override async Task<ActionResult<OnlineStoreFilterResponse>> HandleAsync(
        [FromMultiSource]OnlineStoreFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new OnlineStoreFilterQuery(request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}