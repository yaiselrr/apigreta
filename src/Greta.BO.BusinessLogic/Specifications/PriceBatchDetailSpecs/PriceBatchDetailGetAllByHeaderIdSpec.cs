using Ardalis.Specification;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Specifications.PriceBatchDetailSpecs;

/// <summary>
/// <inheritdoc/>
/// </summary>
public sealed class PriceBatchDetailGetAllByHeaderIdSpec:Specification<PriceBatchDetail>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="headerId"></param>
    public PriceBatchDetailGetAllByHeaderIdSpec(long headerId)
    { 
        Query.Include(x => x.Product.Category);
        Query.Include(x => x.Product.Department);
        Query.Include(x => x.Product.DefaulShelfTag);
        Query.Include(x => x.Family.Products);

        Query.Where(x => x.HeaderId == headerId);
    }
}