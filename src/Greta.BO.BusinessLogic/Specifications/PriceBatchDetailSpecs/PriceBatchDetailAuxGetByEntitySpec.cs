using Ardalis.Specification;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Specifications.PriceBatchDetailSpecs;

/// <summary>
/// <inheritdoc/>
/// </summary>
public sealed class PriceBatchDetailAuxGetByEntitySpec : Specification<PriceBatchDetail>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity">Entity</param>
    public PriceBatchDetailAuxGetByEntitySpec(PriceBatchDetail entity)
    {
        Query.Where(x => x.HeaderId == entity.HeaderId &&
                            ((entity.ProductId != null && x.ProductId == entity.ProductId) ||
                             (entity.FamilyId != null && x.FamilyId == entity.FamilyId) ||
                             (entity.CategoryId != null && x.CategoryId == entity.CategoryId)));
    }
}