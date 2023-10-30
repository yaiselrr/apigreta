using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Tax;
using Greta.BO.BusinessLogic.Handlers.Queries.Tax;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.TaxEndpoints;
[Route("api/Tax")]
public class TaxUpdate : EndpointBaseAsync.WithRequest<TaxUpdateRequest>.WithResult<TaxUpdateResponse>
{
    private readonly IMediator _mediator;

    public TaxUpdate(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{entityId}")]
    [SwaggerOperation(
        Summary = "Update a Tax by Id",
        Description = "Update a Tax by Id",
        OperationId = "Tax.Update",
        Tags = new[] { "Tax" })
    ]
    [ProducesResponseType(typeof(TaxGetByIdResponse), 200)]
    public override async Task<TaxUpdateResponse> HandleAsync(
        [FromMultiSource] TaxUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new TaxUpdateCommand(request.Id, request.EntityDto), cancellationToken);
    }
}