using Ardalis.Specification;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Specifications.PriceBatchDetailSpecs;

/// <summary>
/// <inheritdoc/>
/// </summary>
public sealed class PriceBatchDetailGetByIdSpecs:Specification<PriceBatchDetail>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">Id</param>
    public PriceBatchDetailGetByIdSpecs(long id)
    {
        Query.Include(x => x.Header).ThenInclude(x=> x.Stores);

        Query.Where(x => x.Id == id);
    }
}