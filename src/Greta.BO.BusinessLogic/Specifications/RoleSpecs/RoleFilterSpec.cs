using Amazon.S3.Model;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Greta.BO.BusinessLogic.Specifications.RoleSpecs
{
    public sealed class RoleFilterSpec:Specification<Role>
    {
        public RoleFilterSpec(RoleSearchModel filter)
        {
            if (!string.IsNullOrEmpty(filter.Search))
                Query.Search(c => c.Name, $"%{filter.Search}%");

            if (!string.IsNullOrEmpty(filter.Name))
                Query.Where(c => c.Name.Contains(filter.Name));
            var splited = string.IsNullOrEmpty(filter.Sort) ? new[] { "", "" } : filter.Sort.Split("_");

            ((List<OrderExpressionInfo<Role>>)OrderExpressions).Add(new OrderExpressionInfo<Role>(
                splited[0] switch
                {
                    _ => f => f.Name
                },
                splited[1] == "desc" ? OrderTypeEnum.OrderByDescending : OrderTypeEnum.OrderBy
            ));
        }
    }
}
