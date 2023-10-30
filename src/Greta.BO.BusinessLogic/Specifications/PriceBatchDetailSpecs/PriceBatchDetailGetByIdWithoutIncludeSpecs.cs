using Ardalis.Specification;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Specifications.PriceBatchDetailSpecs;

/// <summary>
/// <inheritdoc/>
/// </summary>
public sealed class PriceBatchDetailGetByIdWithoutIncludeSpecs:Specification<PriceBatchDetail>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">Id</param>
    public PriceBatchDetailGetByIdWithoutIncludeSpecs(long id)
    {

        Query.Where(x => x.Id == id);
    }
}