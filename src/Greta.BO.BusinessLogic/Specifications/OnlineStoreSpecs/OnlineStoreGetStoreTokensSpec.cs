using System.Collections.Generic;
using System.Linq;
using Ardalis.Specification;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Specifications.OnlineStoreSpecs;


/// <inheritdoc/>
public sealed class OnlineStoreGetStoreTokensSpec : Specification<OnlineStore>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="stores"></param>
    /// <param name="departmentId"></param>
    public OnlineStoreGetStoreTokensSpec(IReadOnlyList<long> stores, long departmentId)
    {
        if (stores == null)
        {
            Query.Where(x => 
                x.State && 
                x.RefreshToken != null && 
                x.Departments.Any(d => d.Id == departmentId));
        }
        else
        {
            Query.Where(x => 
                x.State && 
                x.Isdeleted == false && 
                x.RefreshToken != null && 
                stores.Contains(x.StoreId) &&
                x.Departments.Any(d => d.Id == departmentId));
        }
    }
}