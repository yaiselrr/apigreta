using System.Linq;
using Ardalis.Specification;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Specifications.OnlineStoreSpec;


/// <inheritdoc/>
public sealed class OnlineStoreGetStoreTokensIdSpec : Specification<OnlineStore>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="storeId"></param>
    public OnlineStoreGetStoreTokensIdSpec(long storeId)
    {
        Query.Where(x => x.Id == storeId && x.State && x.RefreshToken != null && x.Isdeleted == false);
    }
}