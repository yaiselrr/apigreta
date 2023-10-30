using Ardalis.Specification;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Specifications.MixAndMatchSpecs;


/// <inheritdoc/>
public sealed class MixAndMatchGetByIdSpec:Specification<MixAndMatch>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    public MixAndMatchGetByIdSpec(long id)
    {
        Query.Where(x => x.Id == id);

        Query.Include(x => x.ProductBuy);
        Query.Include(x => x.Products);
        Query.Include(x => x.Families);        
    }
}