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
public class TaxFilter: EndpointBaseAsync.WithRequest<TaxFilterRequest>.WithActionResult<TaxFilterResponse>
{
    private readonly IMediator _mediator;

    public TaxFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of tax entity",
        Description = "Gets a paginated list of tax entity",
        OperationId = "Tax.Filter",
        Tags = new[] { "Tax" })
    ]
    [ProducesResponseType(typeof(TaxFilterResponse), 200)]
    public override async Task<ActionResult<TaxFilterResponse>> HandleAsync(
        [FromMultiSource]TaxFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new TaxFilterQuery(request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}