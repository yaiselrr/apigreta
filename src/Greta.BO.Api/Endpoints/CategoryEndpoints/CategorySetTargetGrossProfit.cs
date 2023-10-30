using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Category;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CategoryEndpoints;
[Route("api/Category")]
public class CategorySetTargetGrossProfit : EndpointBaseAsync.WithRequest<CategorySetTargetGrossProfitRequest>.WithResult<CategorySetTargetGrossProfitResponse>
{
    private readonly IMediator _mediator;

    public CategorySetTargetGrossProfit(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("SetTargetGrossProfit/{entityId}")]
    [SwaggerOperation(
        Summary = "Update gross profit to products of Category",
        Description = "Update gross profit to products of Category",
        OperationId = "Category.SetTargetGrossProfit",
        Tags = new[] { "Category" })
    ]
    //[ProducesResponseType(typeof(CategoryGetByIdResponse), 200)]
    public override async Task<CategorySetTargetGrossProfitResponse> HandleAsync(
        [FromMultiSource] CategorySetTargetGrossProfitRequest request,
        CancellationToken cancellationToken = default)
    {
        //SetTargetGrossProfit
        return await _mediator.Send(new CategorySetTargetGrossProfitCommand(request.Id, request.CategoryTargetGrossProfitDto), cancellationToken);
    }
}