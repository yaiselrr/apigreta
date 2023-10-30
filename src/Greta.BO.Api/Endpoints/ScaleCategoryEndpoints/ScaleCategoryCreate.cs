using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.ScaleCategory;
using Greta.BO.BusinessLogic.Handlers.Queries.ScaleCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ScaleCategoryEndpoints;
[Route("api/ScaleCategory")]
public class ScaleCategoryCreate: EndpointBaseAsync.WithRequest<ScaleCategoryCreateRequest>.WithResult<ScaleCategoryCreateResponse>
{
    private readonly IMediator _mediator;

    public ScaleCategoryCreate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Create a new scale category entity",
        Description = "Create a new scale category entity",
        OperationId = "ScaleCategory.Create",
        Tags = new[] { "ScaleCategory" })
    ]
    [ProducesResponseType(typeof(ScaleCategoryGetByIdResponse), 200)]
    public override async Task<ScaleCategoryCreateResponse> HandleAsync(
        [FromMultiSource] ScaleCategoryCreateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new ScaleCategoryCreateCommand(request.EntityDto), cancellationToken);
    }
}