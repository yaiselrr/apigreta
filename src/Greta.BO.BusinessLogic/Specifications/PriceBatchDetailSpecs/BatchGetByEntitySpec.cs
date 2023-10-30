using Ardalis.Specification;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Specifications.PriceBatchDetailSpecs;

/// <summary>
/// <inheritdoc/>
/// </summary>
public sealed class BatchGetByEntitySpec:Specification<Batch>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="headerId">HeaderId</param>
    public BatchGetByEntitySpec(long headerId)
    { 
        Query.Include(x => x.Stores);

        Query.Where(x => x.Id == headerId);
    }
}