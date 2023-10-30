using Ardalis.Specification;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Specifications.ProfilesSpecs;

/// <summary>
/// <inheritdoc/>
/// </summary>
public sealed class ProfilesGetByNameDistinctIdSpec:Specification<Profiles>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name">Profile name</param>
    /// <param name="id">Profile id</param>
    public ProfilesGetByNameDistinctIdSpec(string name, long id)
    {
        if (string.IsNullOrEmpty(name))
        {
            Query.Where(x => x.Name.ToUpper().Contains(name.ToUpper()) && x.Id != id);
        }    
    }
}