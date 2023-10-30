using System;
using Ardalis.Specification;

namespace Greta.BO.BusinessLogic.Specifications.ReportSpecs;

/// <summary>
/// Report Filter Specification
/// </summary>
public sealed class ReportGetByGuidSpec: Specification<Api.Entities.Report>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="guid"></param>    
    public ReportGetByGuidSpec(Guid guid)
    {
        Query.Where(x => x.GuidId == guid);        
    }
}