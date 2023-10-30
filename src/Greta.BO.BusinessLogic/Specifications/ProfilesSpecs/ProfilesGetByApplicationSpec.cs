using Ardalis.Specification;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Specifications.ProfilesSpecs;

/// <summary>
/// <inheritdoc/>
/// </summary>
public sealed class ProfilesGetByApplicationSpec:Specification<Profiles>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="applicationId">Application Id</param>
    public ProfilesGetByApplicationSpec(long applicationId)
    {
        Query.Where(x => x.ApplicationId == applicationId);

        Query.Include(x => x.Application);        
    }
}