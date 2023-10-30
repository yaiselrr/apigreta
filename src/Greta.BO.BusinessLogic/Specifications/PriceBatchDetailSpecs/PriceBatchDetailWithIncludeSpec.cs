using Ardalis.Specification;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Specifications.PriceBatchDetailSpecs;

/// <summary>
/// <inheritdoc/>
/// </summary>
public sealed class PriceBatchDetailWithIncludeSpec:Specification<PriceBatchDetail>
{
    /// <summary>
    /// 
    /// </summary>
    public PriceBatchDetailWithIncludeSpec()
    { 
        Query.Include(x => x.Header);
    }
}