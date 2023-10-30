using Ardalis.Specification;
using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.BusinessLogic.Specifications.ReportSpecs;

/// <summary>
/// Report Filter Specification
/// </summary>
public sealed class ReportGetByCategorySpec: Specification<Api.Entities.Report>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="category"></param>    
    public ReportGetByCategorySpec(ReportCategory category)
    {
        Query.Where(x => x.Category == category);        
    }
}