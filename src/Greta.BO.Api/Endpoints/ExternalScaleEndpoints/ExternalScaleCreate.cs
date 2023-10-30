using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.ExternalScale;
using Greta.BO.BusinessLogic.Handlers.Queries.ExternalScale;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ExternalScaleEndpoints;
[Route("api/ExternalScale")]
public class ExternalScaleCreate: EndpointBaseAsync.WithRequest<ExternalScaleCreateRequest>.WithResult<ExternalScaleCreateResponse>
{
    private readonly IMediator _mediator;

    public ExternalScaleCreate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Create a new external scale entity",
        Description = "Create a new external scale entity",
        OperationId = "ExternalScale.Create",
        Tags = new[] { "ExternalScale" })
    ]
    [ProducesResponseType(typeof(ExternalScaleGetByIdResponse), 200)]
    public override async Task<ExternalScaleCreateResponse> HandleAsync(
        [FromMultiSource] ExternalScaleCreateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new ExternalScaleCreateCommand(request.EntityDto), cancellationToken);
    }
}