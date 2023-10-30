using Ardalis.Specification;
using Greta.BO.Api.Entities;
using System.Linq;

namespace Greta.BO.BusinessLogic.Specifications.CutListDetailSpecs;

/// <summary>
/// Get ScaleProduct by upc and plu
/// </summary>
public sealed class CutListGetScaleProductByUpcAndPluSpec : SingleResultSpecification<ScaleProduct>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="upc"></param>
    /// <param name="plu"></param>
    /// <param name="storeIdAnimal">Store id of Animal selected</param>
    public CutListGetScaleProductByUpcAndPluSpec(string upc, int plu, long storeIdAnimal)
    {
        Query.Include(x => x.StoreProducts);
        Query.Where(x => x.UPC.Equals(upc) && x.PLUNumber == plu && x.StoreProducts.Any(s => s.StoreId == storeIdAnimal));
    }
}