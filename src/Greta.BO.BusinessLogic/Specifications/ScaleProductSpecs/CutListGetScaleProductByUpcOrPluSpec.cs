using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using System.Collections.Generic;
using System.Linq;

namespace Greta.BO.BusinessLogic.Specifications.ScaleProductSpecs;

/// <summary>
/// Get ScaleProduct by upc and plu
/// </summary>
public sealed class CutListGetScaleProductByUpcOrPluSpec : Specification<ScaleProduct>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="upc"></param>   
    /// <param name="plu"></param>   
    public CutListGetScaleProductByUpcOrPluSpec(string filter)
    {       
        int plu = 0;

        int.TryParse(filter, out plu);

        if (!string.IsNullOrEmpty(filter))
        {
            if (plu>0)
            {
                Query.Where(x => x.UPC.Equals(filter) || x.PLUNumber == plu);
            }
            else
            {
                Query.Where(x => x.UPC.Equals(filter));
            }
        }
    }
}