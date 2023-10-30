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
public class FeeUpdate: EndpointBaseAsync.WithRequest<FeeUpdateRequest>.WithResult<FeeUpdateResponse>
{
    private readonly IMediator _mediator;

    public FeeUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update a Fee by Id",
        Description = "Update a Fee by Id",
        OperationId = "Fee.Update",
        Tags = new[] { "Fee" })
    ]
    [ProducesResponseType(typeof(FeeGetByIdResponse), 200)]
    public override async Task<FeeUpdateResponse> HandleAsync(
        [FromMultiSource] FeeUpdateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new FeeUpdateCommand(request.Id, request.EntityDto), cancellationToken);
    }
}