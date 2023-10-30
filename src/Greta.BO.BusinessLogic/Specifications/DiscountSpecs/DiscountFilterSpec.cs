using System.Collections.Generic;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.BusinessLogic.Specifications.DiscountSpecs;

/// <summary>
/// Discount Filter Specification
/// </summary>
public sealed class DiscountFilterSpec : Specification<Discount>
{
    /// <inheritdoc />
    public DiscountFilterSpec(DiscountSearchModel filter)
    {
        if (!string.IsNullOrEmpty(filter.Search))
        {
            Query.Search(c => c.Name, $"%{filter.Search}%");
            Query.Search(c => c.Department.Name, $"%{filter.Search}%");
            Query.Search(c => c.Category.Name, $"%{filter.Search}%");
        }

        if (!string.IsNullOrEmpty(filter.Name))
            Query.Where(c => c.Name.Contains(filter.Name));
        if (filter.Type >= 0)
            Query.Where(c => c.Type == filter.Type);

        var splited = string.IsNullOrEmpty(filter.Sort) ? new[] { "", "" } : filter.Sort.Split("_");

        ((List<OrderExpressionInfo<Discount>>)OrderExpressions).Add(new OrderExpressionInfo<Discount>(
            splited[0] switch
            {
                "name" => f => f.Name,
                "value" => f => f.Value,
                "type" => f => f.Type,
                _ => f => f.Name
            },
            splited[1] == "desc" ? OrderTypeEnum.OrderByDescending : OrderTypeEnum.OrderBy
        ));

    }
}