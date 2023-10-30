using System.Collections.Generic;
using Ardalis.Specification;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Specifications.PriceBatchDetailSpecs;

/// <summary>
/// <inheritdoc/>
/// </summary>
public sealed class PriceBatchDetailGetByListIdsWithoutIncludeSpecs:Specification<PriceBatchDetail>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ids">Ids</param>
    public PriceBatchDetailGetByListIdsWithoutIncludeSpecs(List<long> ids)
    {

        Query.Where(x => ids.Contains(x.Id));
    }
}