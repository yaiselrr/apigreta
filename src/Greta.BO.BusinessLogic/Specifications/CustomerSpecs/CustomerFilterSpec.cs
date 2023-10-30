using System.Collections.Generic;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.BusinessLogic.Specifications.CustomerSpecs;

/// <summary>
/// Customer Filter Specification
/// </summary>
public sealed class CustomerFilterSpec : Specification<Customer>
{
    /// <inheritdoc />
    public CustomerFilterSpec(CustomerSearchModel filter)
    {
        if (!string.IsNullOrEmpty(filter.Search))
        {
            Query.Search(c => c.LastName, $"%{filter.Search}%");
            Query.Search(c => c.Email, $"%{filter.Search}%");
            Query.Search(c => c.FirstName, $"%{filter.Search}%");
            Query.Search(c => c.Phone, $"%{filter.Search}%");
        }

        if (!string.IsNullOrEmpty(filter.LastName))
            Query.Where(c => c.LastName.Contains(filter.LastName));
        if (!string.IsNullOrEmpty(filter.Email))
            Query.Where(c => c.Email.Contains(filter.Email));
        if (!string.IsNullOrEmpty(filter.FirstName))
            Query.Where(c => c.FirstName.Contains(filter.FirstName));
        if (!string.IsNullOrEmpty(filter.Phone))
            Query.Where(c => c.Phone.Contains(filter.Phone));

        var splited = string.IsNullOrEmpty(filter.Sort) ? new[] { "", "" } : filter.Sort.Split("_");

        ((List<OrderExpressionInfo<Customer>>)OrderExpressions).Add(new OrderExpressionInfo<Customer>(
            splited[0] switch
            {
                "firstname" => f => f.FirstName,
                "email" => f => f.Email,
                "lastname" => f => f.LastName,
                "phone" => f => f.Phone,
                _ => f => f.FirstName
            },
            splited[1] == "desc" ? OrderTypeEnum.OrderByDescending : OrderTypeEnum.OrderBy
        ));

    }
}