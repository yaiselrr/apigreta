using Ardalis.Specification;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Specifications.MixAndMatchSpecs;


/// <inheritdoc/>
public sealed class MixAndMatchGetByIdUpdateSpec:Specification<MixAndMatch>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    public MixAndMatchGetByIdUpdateSpec(long id)
    {
        Query.Where(x => x.Id == id);

        Query.Include(x => x.Products);
        Query.Include(x => x.Families);        
    }
}