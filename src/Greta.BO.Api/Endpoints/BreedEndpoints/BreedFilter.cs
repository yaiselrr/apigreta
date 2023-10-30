using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Breed;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.BreedEndpoints;

[Route("api/Breed")]
public class BreedFilter: EndpointBaseAsync.WithRequest<BreedFilterRequest>.WithActionResult<BreedFilterResponse>
{
    private readonly IMediator _mediator;

    public BreedFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of the Breed entity",
        Description = "Gets a paginated list of the Breed entity",
        OperationId = "Breed.Filter",
        Tags = new[] { "Breed" })
    ]
    [ProducesResponseType(typeof(BreedFilterResponse), 200)]
    public override async Task<ActionResult<BreedFilterResponse>> HandleAsync(
        [FromMultiSource]BreedFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new BreedFilterQuery(request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}