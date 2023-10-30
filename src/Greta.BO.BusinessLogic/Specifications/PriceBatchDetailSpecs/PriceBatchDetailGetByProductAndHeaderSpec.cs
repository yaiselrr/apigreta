using Ardalis.Specification;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Specifications.PriceBatchDetailSpecs;

/// <summary>
/// <inheritdoc/>
/// </summary>
public sealed class PriceBatchDetailGetByProductAndHeaderSpec:Specification<PriceBatchDetail>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="productId">Product Id</param>
    /// <param name="headerId">Header Id</param>
    public PriceBatchDetailGetByProductAndHeaderSpec(long productId, long headerId)
    {
        Query.Where(e => e.ProductId == productId && e.HeaderId == headerId);
    }
}