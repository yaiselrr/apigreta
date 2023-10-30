using Ardalis.Specification;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Specifications.AnimalSpecs;

/// <summary>
/// Get animal with all childrens
/// </summary>
public sealed class AnimalGetByIdWithChildrenSpecs : SingleResultSpecification<Animal>
{
    /// <inheritdoc />
    public AnimalGetByIdWithChildrenSpecs(long animalId)
    {
        Query.Where(x => x.Id == animalId);

        Query.Include(x => x.Rancher)
            .Include(x => x.Breed)
            .Include(x => x.Store)
            .Include(x => x.Customers);
    }
}