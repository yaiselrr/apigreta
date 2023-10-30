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
public class ExternalScaleGetById : EndpointBaseAsync.WithRequest<ExternalScaleGetByIdRequest>.WithActionResult<ExternalScaleGetByIdResponse>
{
    private readonly IMediator _mediator;

    public ExternalScaleGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get external scale entity by id",
        Description = "Get external scale entity by id",
        OperationId = "ExternalScale.GetById",
        Tags = new[] { "ExternalScale" })
    ]
    [ProducesResponseType(typeof(ExternalScaleGetByIdResponse), 200)]
    public override async Task<ActionResult<ExternalScaleGetByIdResponse>> HandleAsync(
        [FromMultiSource] ExternalScaleGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new ExternalScaleGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}