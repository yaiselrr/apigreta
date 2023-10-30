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
public class ScaleCategoryUpdate : EndpointBaseAsync.WithRequest<ScaleCategoryUpdateRequest>.WithResult<ScaleCategoryUpdateResponse>
{
    private readonly IMediator _mediator;

    public ScaleCategoryUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update a scale category entity by Id",
        Description = "Update a scale category entity by Id",
        OperationId = "ScaleCategory.Update",
        Tags = new[] { "ScaleCategory" })
    ]
    [ProducesResponseType(typeof(ScaleCategoryGetByIdResponse), 200)]
    public override async Task<ScaleCategoryUpdateResponse> HandleAsync(
        [FromMultiSource] ScaleCategoryUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new ScaleCategoryUpdateCommand(request.Id, request.EntityDto), cancellationToken);
    }
}