using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.CutListTemplate;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CutListTemplateEndpoints;

[Route("api/CutListTemplate")]
public class CutListTemplateFilter: EndpointBaseAsync.WithRequest<CutListTemplateFilterRequest>.WithActionResult<CutListTemplateFilterResponse>
{
    private readonly IMediator _mediator;

    public CutListTemplateFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of CutListTemplate entity",
        Description = "Gets a paginated list of CutListTemplate entity",
        OperationId = "CutListTemplate.Filter",
        Tags = new[] { "CutListTemplate" })
    ]
    [ProducesResponseType(typeof(CutListTemplateFilterResponse), 200)]
    public override async Task<ActionResult<CutListTemplateFilterResponse>> HandleAsync(
        [FromMultiSource]CutListTemplateFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new CutListTemplateFilterQuery(request.CurrentPage, request.PageSize, request.Filter), cancellationToken));
    }
}