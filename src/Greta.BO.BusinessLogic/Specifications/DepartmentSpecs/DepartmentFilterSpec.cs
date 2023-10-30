using System.Collections.Generic;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.BusinessLogic.Specifications.DepartmentSpecs;

/// <summary>
/// Department Filter Specification
/// </summary>
public sealed class DepartmentFilterSpec: Specification<Department>
{
    /// <inheritdoc />
    public DepartmentFilterSpec(DepartmentSearchModel filter)
    {
        if (!string.IsNullOrEmpty(filter.Search))
        {
            Query.Search(c => c.Name, $"%{filter.Search}%");
            Query.Search(c => (c.DepartmentId).ToString(), $"%{filter.Search}%");
        }
       
        if(!string.IsNullOrEmpty(filter.Name))
            Query.Where(c => c.Name.Contains(filter.Name));
        if(filter.DepartmentId > 0)
            Query.Where(c => c.DepartmentId == filter.DepartmentId);

        var splited = string.IsNullOrEmpty(filter.Sort) ? new[] {"", ""} : filter.Sort.Split("_");
        
        ((List<OrderExpressionInfo<Department>>)OrderExpressions).Add(new OrderExpressionInfo<Department>(
            splited[0] switch
            {
                "name" => f => f.Name,
                "departmentId" => f => f.DepartmentId,
                _ => f => f.Name
            }, 
            splited[1] == "desc" ? OrderTypeEnum.OrderByDescending: OrderTypeEnum.OrderBy
        ));

    }
}