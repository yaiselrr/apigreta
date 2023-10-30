using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.PriceBatchDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.PriceBatchDetailEndpoints;
[Route("api/PriceBatchDetail")]
public class PriceBatchDetailCreate : EndpointBaseAsync.WithRequest<PriceBatchDetailCreateRequest>.WithResult<PriceBatchDetailCreateResponse>
{
    private readonly IMediator _mediator;

    public PriceBatchDetailCreate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Create a new price batch detail entity",
        Description = "Create a new price batch detail entity",
        OperationId = "PriceBatchDetail.Create",
        Tags = new[] { "PriceBatchDetail" })
    ]
    [ProducesResponseType(typeof(PriceBatchDetailCreateResponse), 200)]
    public override async Task<PriceBatchDetailCreateResponse> HandleAsync(
        [FromMultiSource] PriceBatchDetailCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new PriceBatchDetailCreateCommand(request.EntityDto));
    }
}