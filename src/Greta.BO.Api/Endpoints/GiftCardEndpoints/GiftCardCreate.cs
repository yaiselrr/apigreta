using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.GiftCardCommands;
using Greta.BO.BusinessLogic.Models.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.GiftCardEndpoints;

[Route("api/giftCard")]
public class GiftCardCreate:EndpointBaseAsync.WithRequest<GiftCardModel>.WithResult<GiftCardCreateResponse>
{
    private readonly IMediator _mediator;

    public GiftCardCreate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Create a new GiftCard",
        Description = "Create a new GiftCard",
        OperationId = "GiftCard.Create",
        Tags = new[] { "GiftCard" })
    ]
    [ProducesResponseType(typeof(GiftCardCreateResponse), 200)]
    public override async Task<GiftCardCreateResponse> HandleAsync(
        [FromBody]GiftCardModel request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new GiftCardCreateCommand(request), cancellationToken);
    }
}