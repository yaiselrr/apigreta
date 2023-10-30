using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.CutListTemplate;
using Greta.BO.BusinessLogic.Handlers.Queries.CutListTemplate;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CutListTemplateEndpoints;
[Route("api/CutListTemplate")]
public class CutListTemplateUpdate: EndpointBaseAsync.WithRequest<CutListTemplateUpdateRequest>.WithResult<CutListTemplateUpdateResponse>
{
    private readonly IMediator _mediator;

    public CutListTemplateUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update a CutListTemplate by Id",
        Description = "Update a CutListTemplate by Id",
        OperationId = "CutListTemplate.Update",
        Tags = new[] { "CutListTemplate" })
    ]
    [ProducesResponseType(typeof(CutListTemplateGetByIdResponse), 200)]
    public override async Task<CutListTemplateUpdateResponse> HandleAsync(
        [FromMultiSource] CutListTemplateUpdateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new CutListTemplateUpdateCommand(request.Id, request.EntityDto), cancellationToken);
    }
}