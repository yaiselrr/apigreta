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
public class CategoryDeleteRange: EndpointBaseAsync.WithRequest<CategoryDeleteRangeRequest>.WithResult<CategoryDeleteRangeResponse>
{
    private readonly IMediator _mediator;

    public CategoryDeleteRange(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("DeleteRange")]
    [SwaggerOperation(
        Summary = "Delete list of the category entities",
        Description = "Delete list of the category entities",
        OperationId = "Category.DeleteRange",
        Tags = new[] { "Category" })
    ]
    [ProducesResponseType(typeof(CategoryDeleteRangeResponse), 200)]
    public override async Task<CategoryDeleteRangeResponse> HandleAsync(
        [FromMultiSource] CategoryDeleteRangeRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new CategoryDeleteRangeCommand(request.Ids), cancellationToken);
    }
}