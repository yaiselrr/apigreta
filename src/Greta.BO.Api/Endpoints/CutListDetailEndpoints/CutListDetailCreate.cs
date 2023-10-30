using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.CutListDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CutListDetailEndpoints;
[Route("api/CutListDetail")]

public class CutListDetailCreate: EndpointBaseAsync.WithRequest<CutListDetailCreateRequest>.WithResult<CutListDetailCreateResponse>
{
    private readonly IMediator _mediator;

    public CutListDetailCreate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Create a new cut list detail entity",
        Description = "Create a new cut list detail entity",
        OperationId = "CutListDetail.Create",
        Tags = new[] { "CutListDetail" })
    ]
    [ProducesResponseType(typeof(CutListDetailCreateResponse), 200)]
    public override async Task<CutListDetailCreateResponse> HandleAsync(
        [FromMultiSource] CutListDetailCreateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new CutListDetailCreateCommand(request.EntityDto), cancellationToken);
    }
}