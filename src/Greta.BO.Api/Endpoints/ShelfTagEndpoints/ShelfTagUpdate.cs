using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.ShelfTag;
using Greta.BO.BusinessLogic.Handlers.Queries.ShelfTag;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ShelfTagEndpoints;
[Route("api/ShelfTag")]
public class ShelfTagUpdate : EndpointBaseAsync.WithRequest<ShelfTagUpdateRequest>.WithResult<ShelfTagUpdateResponse>
{
    private readonly IMediator _mediator;

    public ShelfTagUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update a ShelfTag entity by Id",
        Description = "Update a ShelfTag entity by Id",
        OperationId = "ShelfTag.Update",
        Tags = new[] { "ShelfTag" })
    ]
    [ProducesResponseType(typeof(ShelfTagGetByIdResponse), 200)]
    public override async Task<ShelfTagUpdateResponse> HandleAsync(
        [FromMultiSource] ShelfTagUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new ShelfTagUpdateCommand(request.Id, request.EntityDto.QTYToPrint), cancellationToken);
    }
}