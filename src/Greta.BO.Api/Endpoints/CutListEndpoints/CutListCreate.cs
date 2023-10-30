using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.CutList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CutListEndpoints;
[Route("api/CutList")]
public class CutListCreate: EndpointBaseAsync.WithRequest<CutListCreateRequest>.WithResult<CutListCreateResponse>
{
    private readonly IMediator _mediator;

    public CutListCreate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Create a new cut list entity",
        Description = "Create a new cut list entity",
        OperationId = "CutList.Create",
        Tags = new[] { "CutList" })
    ]
    [ProducesResponseType(typeof(CutListCreateResponse), 200)]
    public override async Task<CutListCreateResponse> HandleAsync(
        [FromMultiSource] CutListCreateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new CutListCreateCommand(request.EntityDto), cancellationToken);
    }
}