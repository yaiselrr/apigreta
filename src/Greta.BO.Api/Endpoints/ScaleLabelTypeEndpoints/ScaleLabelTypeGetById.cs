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
public class ScaleLabelTypeGetById : EndpointBaseAsync.WithRequest<ScaleLabelTypeGetByIdRequest>.WithActionResult<ScaleLabelTypeGetByIdResponse>
{
    private readonly IMediator _mediator;

    public ScaleLabelTypeGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get scale label type entity by id",
        Description = "Get scale label type entity by id",
        OperationId = "ScaleLabelType.GetById",
        Tags = new[] { "ScaleLabelType" })
    ]
    [ProducesResponseType(typeof(ScaleLabelTypeGetByIdResponse), 200)]
    public override async Task<ActionResult<ScaleLabelTypeGetByIdResponse>> HandleAsync(
        [FromMultiSource] ScaleLabelTypeGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new ScaleLabelTypeGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}