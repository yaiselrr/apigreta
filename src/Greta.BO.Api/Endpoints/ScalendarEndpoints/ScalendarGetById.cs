using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Scalendar;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ScalendarEndpoints;

[Route("api/Scalendar")]
public class ScalendarGetById : EndpointBaseAsync.WithRequest<ScalendarGetByIdRequest>.WithActionResult<ScalendarGetByIdResponse>
{
    private readonly IMediator _mediator;

    public ScalendarGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get Scalendar entity by id",
        Description = "Get Scalendar entity by id",
        OperationId = "Scalendar.GetById",
        Tags = new[] { "Scalendar" })
    ]
    [ProducesResponseType(typeof(ScalendarGetByIdResponse), 200)]
    public override async Task<ActionResult<ScalendarGetByIdResponse>> HandleAsync(
        [FromMultiSource] ScalendarGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new ScalendarGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}