using System;
using System.Collections.Generic;
using Ardalis.Specification;

namespace Greta.BO.BusinessLogic.Specifications.ReportSpecs;

/// <summary>
/// Report Filter Specification
/// </summary>
public sealed class ReportGetSPAByFilterSpec : Specification<Api.Entities.Report>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="guid"></param>    
    public ReportGetSPAByFilterSpec(List<Guid> guid)
    {
        Query.Where(x => guid.Contains(x.GuidId));        
    }
}