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
public class TaxCreate: EndpointBaseAsync.WithRequest<TaxCreateRequest>.WithResult<TaxCreateResponse>
{
    private readonly IMediator _mediator;

    public TaxCreate(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("")]
    [SwaggerOperation(
        Summary = "Create a new Tax",
        Description = "Create a new Tax",
        OperationId = "Tax.Create",
        Tags = new[] { "Tax" })
    ]
    [ProducesResponseType(typeof(TaxGetByIdResponse), 200)]
    public override async Task<TaxCreateResponse> HandleAsync(
        [FromMultiSource] TaxCreateRequest request, 
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new TaxCreateCommand(request.EntityDto), cancellationToken);
    }
}