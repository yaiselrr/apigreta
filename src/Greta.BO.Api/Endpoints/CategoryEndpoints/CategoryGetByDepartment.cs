using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Queries.Category;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.CategoryEndpoints;

[Route("api/Category")]
public class CategoryGetByDepartment : EndpointBaseAsync.WithRequest<CategoryGetByDepartmentRequest>.WithActionResult<CategoryGetByDepartmentResponse>
{
    private readonly IMediator _mediator;

    public CategoryGetByDepartment(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetBydepartment/{departmentId:int}")]
    [SwaggerOperation(
        Summary = "Get list category entity by department",
        Description = "Get list category entity by department",
        OperationId = "Category.GetBydepartment",
        Tags = new[] { "Category" })
    ]
    [ProducesResponseType(typeof(CategoryGetByDepartmentResponse), 200)]
    public override async Task<ActionResult<CategoryGetByDepartmentResponse>> HandleAsync(
        [FromMultiSource] CategoryGetByDepartmentRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = await _mediator.Send(new CategoryGetByDepartmentQuery(request.DepartmentId), cancellationToken);
        return data != null ? data : NotFound();
    }
}