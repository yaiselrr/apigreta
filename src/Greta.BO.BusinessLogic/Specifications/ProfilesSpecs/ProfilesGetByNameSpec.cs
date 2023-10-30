using Ardalis.Specification;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Specifications.ProfilesSpecs;

/// <summary>
/// <inheritdoc/>
/// </summary>
public sealed class ProfilesGetByNameSpec:Specification<Profiles>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name">Profile name</param>
    public ProfilesGetByNameSpec(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Query.Where(x => x.Name.ToUpper().Contains(name.ToUpper()));
        }     
    }
}