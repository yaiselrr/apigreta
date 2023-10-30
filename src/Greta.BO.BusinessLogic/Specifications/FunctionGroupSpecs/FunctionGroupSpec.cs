using Ardalis.Specification;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Specifications.FunctionGroupSpecs;

/// <summary>
/// <inheritdoc/>
/// </summary>
public sealed class FunctionGroupSpec:Specification<FunctionGroup>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="applicationId"></param>
    public FunctionGroupSpec(long applicationId)
    {
        Query.Where(x => x.ClientApplicationId == applicationId);

        Query.Include(x => x.ClientApplication);
        Query.Include(x => x.Permissions);
    }
}