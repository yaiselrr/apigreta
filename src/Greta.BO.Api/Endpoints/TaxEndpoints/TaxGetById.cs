using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Tax;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.TaxEndpoints;

[Route("api/Tax")]
public class TaxGetById : EndpointBaseAsync.WithRequest<TaxGetByIdRequest>.WithActionResult<TaxGetByIdResponse>
{
    private readonly IMediator _mediator;

    public TaxGetById(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{entityId:int}")]
    [SwaggerOperation(
        Summary = "Get Entity by id",
        Description = "Get Entity by id",
        OperationId = "Tax.GetById",
        Tags = new[] { "Tax" })
    ]
    [ProducesResponseType(typeof(TaxGetByIdResponse), 200)]
    public override async Task<ActionResult<TaxGetByIdResponse>> HandleAsync(
        [FromMultiSource] TaxGetByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new TaxGetByIdQuery(request.Id), cancellationToken);
        return data != null ? data : NotFound();
    }
}