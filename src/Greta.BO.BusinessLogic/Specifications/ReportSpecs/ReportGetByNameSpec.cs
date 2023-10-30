using Ardalis.Specification;

namespace Greta.BO.BusinessLogic.Specifications.ReportSpecs;

/// <summary>
/// Report Filter Specification
/// </summary>
public sealed class ReportGetByNameSpec: Specification<Api.Entities.Report>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="id"></param>
    public ReportGetByNameSpec(string name, long id)
    {
        Query.Where(x => x.Name == name);
        if (id != -1)
        {
            Query.Where(x => x.Id != id);
        }
    }
}