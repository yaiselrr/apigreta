using System.Collections.Generic;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.BusinessLogic.Specifications.CutListTemplateSpecs;

/// <summary>
/// CutListTemplate Filter Specification
/// </summary>
public sealed class CutListTemplateGetByIdSpec: Specification<CutListTemplate>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    public CutListTemplateGetByIdSpec(long id)
    {
        Query.Include(x => x.ScaleProducts);
        Query.Where(x => x.Id == id);        
    }
}