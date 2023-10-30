using System.Collections.Generic;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.BusinessLogic.Specifications.CategorySpecs;

/// <summary>
/// Category Filter Specification
/// </summary>
public sealed class CategoryFilterSpec : Specification<Category>
{
    /// <inheritdoc />
    public CategoryFilterSpec(CategorySearchModel filter)
    {
        Query.Include(x => x.Department);

        if (!string.IsNullOrEmpty(filter.Search))
        {
            Query.Search(c => c.Name, $"%{filter.Search}%");
            Query.Search(c => c.Description, $"%{filter.Search}%");
            Query.Search(c => c.Department.Name, $"%{filter.Search}%");
            Query.Search(c => c.CategoryId.ToString(), $"%{filter.Search}%");
        }

        if (!string.IsNullOrEmpty(filter.Name))
            Query.Where(c => c.Name.Contains(filter.Name));
        if (!string.IsNullOrEmpty(filter.Description))
            Query.Where(c => c.Description.Contains(filter.Description));
        if (filter.DepartmentId > 0)
            Query.Where(c => c.DepartmentId == filter.DepartmentId);

        var splited = string.IsNullOrEmpty(filter.Sort) ? new[] { "", "" } : filter.Sort.Split("_");

        ((List<OrderExpressionInfo<Category>>)OrderExpressions).Add(new OrderExpressionInfo<Category>(
            splited[0] switch
            {
                "name" => f => f.Name,
                "department" => f => f.Department.Name,
                "categoryId" => f => f.CategoryId,
                "description" => f => f.Description,
                _ => f => f.Name
            },
            splited[1] == "desc" ? OrderTypeEnum.OrderByDescending : OrderTypeEnum.OrderBy
        ));

    }
}