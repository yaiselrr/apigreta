using Ardalis.Specification;
using Greta.BO.Api.Entities;
using System.Linq;

namespace Greta.BO.BusinessLogic.Specifications.CutListDetailSpecs;

/// <summary>
/// Get ScaleProduct by upc and plu
/// </summary>
public sealed class CutListGetScaleProductOfTemplateSpec : SingleResultSpecification<ScaleProduct>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="idTemplate"></param>
    /// <param name="storeIdAnimal">Store id of Animal selected</param>
    public CutListGetScaleProductOfTemplateSpec(long idTemplate, long storeIdAnimal)
    {
        Query.Include(x => x.CutListTemplates);
        Query.Include(x => x.StoreProducts);
        Query.Where(x => x.CutListTemplates.Any(x=>x.Id == idTemplate) && x.StoreProducts.Any(s => s.StoreId == storeIdAnimal));
    }
}