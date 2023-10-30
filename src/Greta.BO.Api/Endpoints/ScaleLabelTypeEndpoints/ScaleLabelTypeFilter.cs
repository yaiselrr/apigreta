using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.ScaleLabelType;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ScaleLabelTypeEndpoints;

[Route("api/ScaleLabelType")]
public class ScaleLabelTypeFilter: EndpointBaseAsync.WithRequest<ScaleLabelTypeFilterRequest>.WithResult<ScaleLabelTypeFilterResponse>
{
    private readonly IMediator _mediator;

    public ScaleLabelTypeFilter(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("{currentPage}/{pageSize}")]
    [SwaggerOperation(
        Summary = "Gets a paginated list of the scale label type entity",
        Description = "Gets a paginated list of the scale label type entity",
        OperationId = "ScaleLabelType.Filter",
        Tags = new[] { "ScaleLabelType" })
    ]
    [ProducesResponseType(typeof(ScaleLabelTypeFilterResponse), 200)]
    public override async Task<ScaleLabelTypeFilterResponse> HandleAsync(
        [FromMultiSource]ScaleLabelTypeFilterRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new ScaleLabelTypeFilterQuery(request.CurrentPage, request.PageSize, request.Filter), cancellationToken);
    }
}