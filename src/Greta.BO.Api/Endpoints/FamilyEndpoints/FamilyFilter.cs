using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Family;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.FamilyEndpoints;

[Route("api/Family")]
public class FamilyFilter: EndpointBaseAsync.WithRequest<FamilyFilterRequest>.WithActionResult<FamilyFilterResponse>
{
    private readonly IMediator _mediator;

    public FamilyFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of family entity",
        Description = "Gets a paginated list of family entity",
        OperationId = "Family.Filter",
        Tags = new[] { "Family" })
    ]
    [ProducesResponseType(typeof(FamilyFilterResponse), 200)]
    public override async Task<ActionResult<FamilyFilterResponse>> HandleAsync(
        [FromMultiSource]FamilyFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new FamilyFilterQuery(request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}