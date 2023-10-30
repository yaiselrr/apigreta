using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.ScaleLabelType;
using Greta.BO.BusinessLogic.Handlers.Queries.ScaleLabelType;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ScaleLabelTypeEndpoints;
[Route("api/ScaleLabelType")]
public class ScaleLabelTypeCreate: EndpointBaseAsync.WithRequest<ScaleLabelTypeCreateRequest>.WithResult<ScaleLabelTypeCreateResponse>
{
    private readonly IMediator _mediator;

    public ScaleLabelTypeCreate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Create a new scale label type entity",
        Description = "Create a new scale label type entity",
        OperationId = "ScaleLabelType.Create",
        Tags = new[] { "ScaleLabelType" })
    ]
    [ProducesResponseType(typeof(ScaleLabelTypeGetByIdResponse), 200)]
    public override async Task<ScaleLabelTypeCreateResponse> HandleAsync(
        [FromMultiSource] ScaleLabelTypeCreateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new ScaleLabelTypeCreateCommand(request.EntityDto), cancellationToken);
    }
}