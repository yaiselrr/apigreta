using Ardalis.Specification;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Specifications.ProfilesSpecs;

/// <summary>
/// <inheritdoc/>
/// </summary>
public sealed class ProfilesGetByIdSpec:Specification<Profiles>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    public ProfilesGetByIdSpec(long id)
    { 
        Query.Include(x => x.Application);
        Query.Include(x => x.Permissions);

        Query.Where(x => x.Id == id);
    }
}