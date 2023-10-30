using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Animal;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.AnimalEndpoints;

[Route("api/Animal")]
public class AnimalFilter: EndpointBaseAsync.WithRequest<AnimalFilterRequest>.WithActionResult<AnimalFilterResponse>
{
    private readonly IMediator _mediator;

    public AnimalFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of the Animal entity",
        Description = "Gets a paginated list of the Animal entity",
        OperationId = "Animal.Filter",
        Tags = new[] { "Animal" })
    ]
    [ProducesResponseType(typeof(AnimalFilterResponse), 200)]
    public override async Task<ActionResult<AnimalFilterResponse>> HandleAsync(
        [FromMultiSource]AnimalFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new AnimalFilterQuery(request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}