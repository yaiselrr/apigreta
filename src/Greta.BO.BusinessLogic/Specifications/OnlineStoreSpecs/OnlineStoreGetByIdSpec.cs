using Ardalis.Specification;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Specifications.OnlineStoreSpec;


/// <inheritdoc/>
public sealed class OnlineStoreGetByIdSpec : Specification<OnlineStore>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    public OnlineStoreGetByIdSpec(long id)
    {
        Query.Include(x => x.Store);
        Query.Include(x => x.Departments);

        Query.Where(x => x.Id == id && x.Isdeleted == false);
    }
}