using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.ExternalScale;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ExternalScaleEndpoints;

[Route("api/ExternalScale")]
public class ExternalScaleGetByStore : EndpointBaseAsync.WithRequest<ExternalScaleGetByStoreRequest>.WithActionResult<ExternalScaleGetByStoreResponse>
{
    private readonly IMediator _mediator;

    public ExternalScaleGetByStore(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetByStore/{storeId:long}")]
    [SwaggerOperation(
        Summary = "Get external scale entity by store",
        Description = "Get external scale entity by store",
        OperationId = "ExternalScale.GetByStore",
        Tags = new[] { "ExternalScale" })
    ]
    [ProducesResponseType(typeof(ExternalScaleGetByStoreResponse), 200)]
    public override async Task<ActionResult<ExternalScaleGetByStoreResponse>> HandleAsync(
        [FromMultiSource] ExternalScaleGetByStoreRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new ExternalScaleGetByStoreQuery(request.storeId), cancellationToken);
        return data != null ? data : NotFound();
    }
}