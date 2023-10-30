using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Fee;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.FeeEndpoints;

[Route("api/Fee")]
public class FeeFilter: EndpointBaseAsync.WithRequest<FeeFilterRequest>.WithActionResult<FeeFilterResponse>
{
    private readonly IMediator _mediator;

    public FeeFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of Fee entity",
        Description = "Gets a paginated list of Fee entity",
        OperationId = "Fee.Filter",
        Tags = new[] { "Fee" })
    ]
    [ProducesResponseType(typeof(FeeFilterResponse), 200)]
    public override async Task<ActionResult<FeeFilterResponse>> HandleAsync(
        [FromMultiSource]FeeFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new FeeFilterQuery(request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}