using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.CutListTemplate;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CutListTemplateEndpoints;
[Route("api/CutListTemplate")]
public class CutListTemplateDeleteRange: EndpointBaseAsync.WithRequest<CutListTemplateDeleteRangeRequest>.WithResult<CutListTemplateDeleteRangeResponse>
{
    private readonly IMediator _mediator;

    public CutListTemplateDeleteRange(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("DeleteRange")]
    [SwaggerOperation(
        Summary = "Delete list of CutListTemplate",
        Description = "Delete list of CutListTemplate",
        OperationId = "CutListTemplate.DeleteRange",
        Tags = new[] { "CutListTemplate" })
    ]
    [ProducesResponseType(typeof(CutListTemplateDeleteRangeResponse), 200)]
    public override async Task<CutListTemplateDeleteRangeResponse> HandleAsync(
        [FromMultiSource] CutListTemplateDeleteRangeRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new CutListTemplateDeleteRangeCommand(request.Ids), cancellationToken);
    }
}