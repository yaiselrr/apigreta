using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using System.Collections.Generic;
using System.Linq;

namespace Greta.BO.BusinessLogic.Specifications.ScaleProductSpecs
    ;

/// <summary>
/// Get ScaleProduct by Upc and Plu
/// </summary>
public sealed class CutListGetScaleProductByUpcPluProductSpec : Specification<ScaleProduct>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cutlistTemplateId"></param>
    /// <param name="filter"></param>
    public CutListGetScaleProductByUpcPluProductSpec(long cutlistTemplateId, ScaleProductSearchModel filter)
    {
        Query.Include(x => x.CutListTemplates);

        Query.Where(x => !x.CutListTemplates.Any(c => c.Id == cutlistTemplateId) || x.CutListTemplates.Count() == 0);

        if (!string.IsNullOrEmpty(filter.Search))
        {
            Query.Search(c => c.Name, $"%{filter.Search}%");
        }

        if (!string.IsNullOrEmpty(filter.Upc))
        {
            Query.Where(x => x.UPC.Equals(filter.Upc));
        } 
         
        if (filter.Plu > 0)
        {
            Query.Where(x => x.PLUNumber == filter.Plu);
        }
        
        if (!string.IsNullOrEmpty(filter.Name))
        {
            Query.Where(x => x.Name.Equals(filter.Name));
        }
        
        var splited = string.IsNullOrEmpty(filter.Sort) ? new[] { "", "" } : filter.Sort.Split("_");

        ((List<OrderExpressionInfo<ScaleProduct>>)OrderExpressions).Add(new OrderExpressionInfo<ScaleProduct>(
            splited[0] switch
            {
                _ => f => f.Name
            },
            splited[1] == "desc" ? OrderTypeEnum.OrderByDescending : OrderTypeEnum.OrderBy
        ));
    }
}