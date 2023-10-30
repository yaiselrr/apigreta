using Ardalis.Specification;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Specifications.CutListDetailSpecs;

public sealed class CutListDetailGetSingleSpec: SingleResultSpecification<CutList>
{
    public CutListDetailGetSingleSpec(long animalId, long customerId, bool includeDetails = false)
    {
        Query
            .Where(x => x.AnimalId == animalId && x.CustomerId == customerId);

        if (includeDetails)
        {
            Query.Include(x => x.CutListDetails).ThenInclude(cd=> cd.Product);
        }
    }
}