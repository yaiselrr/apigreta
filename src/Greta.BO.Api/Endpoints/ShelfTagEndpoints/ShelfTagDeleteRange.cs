using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.ShelfTag;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ShelfTagEndpoints;
[Route("api/ShelfTag")]
public class ShelfTagDeleteRange: EndpointBaseAsync.WithRequest<ShelfTagDeleteRangeRequest>.WithResult<ShelfTagDeleteRangeResponse>
{
    private readonly IMediator _mediator;

    public ShelfTagDeleteRange(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("DeleteShelfTags")]
    [SwaggerOperation(
        Summary = "Delete list of ShelfTag entities",
        Description = "Delete list of ShelfTag entities",
        OperationId = "ShelfTag.DeleteShelfTags",
        Tags = new[] { "ShelfTag" })
    ]
    [ProducesResponseType(typeof(ShelfTagDeleteRangeResponse), 200)]
    public override async Task<ShelfTagDeleteRangeResponse> HandleAsync(
        [FromMultiSource] ShelfTagDeleteRangeRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new ShelfTagDeleteRangeCommand(request.Ids), cancellationToken);
    }
}