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
public class CutListTemplateGetById : EndpointBaseAsync.WithRequest<CutListTemplateGetByIdRequest>.WithActionResult<CutListTemplateGetByIdResponse>
{
    private readonly IMediator _mediator;

    public CutListTemplateGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get CutListTemplate by id",
        Description = "Get CutListTemplate by id",
        OperationId = "CutListTemplate.GetById",
        Tags = new[] { "CutListTemplate" })
    ]
    [ProducesResponseType(typeof(CutListTemplateGetByIdResponse), 200)]
    public override async Task<ActionResult<CutListTemplateGetByIdResponse>> HandleAsync(
        [FromMultiSource] CutListTemplateGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new CutListTemplateGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}