using Ardalis.Specification;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Specifications.ProfilesSpecs;

/// <summary>
/// <inheritdoc/>
/// </summary>
public sealed class ProfilesCheckExistWithThisApplicationSpec : Specification<Profiles>
{
    /// <summary>
    /// 
    /// </summary>
    /// /// <param name="id">Profile Id</param>
    /// <param name="applicationId">Application Id</param>
    public ProfilesCheckExistWithThisApplicationSpec(long id,long applicationId)
    {
        Query.Where(x => x.ApplicationId == applicationId && x.Id == id);           
    }
}