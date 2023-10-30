using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Grind;
using Greta.BO.BusinessLogic.Handlers.Queries.Grind;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.GrindEndpoints;
[Route("api/Grind")]
public class GrindCreate: EndpointBaseAsync.WithRequest<GrindCreateRequest>.WithResult<GrindCreateResponse>
{
    private readonly IMediator _mediator;

    public GrindCreate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Create a new Grind entity",
        Description = "Create a new Grind entity",
        OperationId = "Grind.Create",
        Tags = new[] { "Grind" })
    ]
    [ProducesResponseType(typeof(GrindGetByIdResponse), 200)]
    public override async Task<GrindCreateResponse> HandleAsync(
        [FromMultiSource] GrindCreateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new GrindCreateCommand(request.EntityDto), cancellationToken);
    }
}