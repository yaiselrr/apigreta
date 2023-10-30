using System.Collections.Generic;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.BusinessLogic.Specifications.ScaleCategorySpecs;

/// <summary>
/// Scale Category Filter Specification
/// </summary>
public sealed class ScaleCategoryFilterSpec: Specification<ScaleCategory>
{
    /// <inheritdoc />
    public ScaleCategoryFilterSpec(ScaleCategorySearchModel filter)
    {
        Query.Include(x => x.Department);
        Query.Include(x => x.Parent); //.IgnoreAutoIncludes();


        if (!string.IsNullOrEmpty(filter.Search))
        {
            Query.Search(c => c.Name, $"%{filter.Search}%");
            Query.Search(c => c.Department.Name, $"%{filter.Search}%");
            Query.Search(c => c.Parent.Name, $"%{filter.Search}%");
            Query.Search(c => c.CategoryId.ToString(), $"%{filter.Search}%");
        }

        if(!string.IsNullOrEmpty(filter.Name))
            Query.Where(c => c.Name.Contains(filter.Name));
        if(filter.DepartmentId > 0)
            Query.Where(c => c.DepartmentId == filter.DepartmentId);

        var splited = string.IsNullOrEmpty(filter.Sort) ? new[] {"", ""} : filter.Sort.Split("_");

        ((List<OrderExpressionInfo<ScaleCategory>>)OrderExpressions).Add(new OrderExpressionInfo<ScaleCategory>(
            splited[0] switch
            {
                "name" => f => f.Name,
                "departmentId" => f => f.Department.Name, 
                "categoryId" => f => f.CategoryId, 
                _ => f => f.Name
            }, 
            splited[1] == "desc" ? OrderTypeEnum.OrderByDescending: OrderTypeEnum.OrderBy
        ));

    }
}