using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.CutList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CutListEndpoints;

[Route("api/CutList")]
public class CutListGetCustomerByAnimal : EndpointBaseAsync.WithRequest<CutListGetCustomerByAnimalRequest>.
    WithActionResult<CutListGetCustomerByAnimalResponse>
{
    private readonly IMediator _mediator;

    public CutListGetCustomerByAnimal(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetCustomerByAnimal/{animalId:int}")]
    [SwaggerOperation(
        Summary = "Get customers by animal",
        Description = "Get customers by animal",
        OperationId = "CutList.GetCustomerByAnimal",
        Tags = new[] { "CutList" })
    ]
    [ProducesResponseType(typeof(CutListGetCustomerByAnimalResponse), 200)]
    public override async Task<ActionResult<CutListGetCustomerByAnimalResponse>> HandleAsync(
        [FromMultiSource] CutListGetCustomerByAnimalRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new CutListGetCustomerByAnimalQuery(request.AnimalId), cancellationToken);
    }
}