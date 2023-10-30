using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Fee;
using Greta.BO.BusinessLogic.Handlers.Queries.Fee;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.FeeEndpoints;
[Route("api/Fee")]
public class FeeCreate: EndpointBaseAsync.WithRequest<FeeCreateRequest>.WithResult<FeeCreateResponse>
{
    private readonly IMediator _mediator;

    public FeeCreate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Create a new Fee",
        Description = "Create a new Fee",
        OperationId = "Fee.Create",
        Tags = new[] { "Fee" })
    ]
    [ProducesResponseType(typeof(FeeGetByIdResponse), 200)]
    public override async Task<FeeCreateResponse> HandleAsync(
        [FromMultiSource] FeeCreateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new FeeCreateCommand(request.EntityDto), cancellationToken);
    }
}