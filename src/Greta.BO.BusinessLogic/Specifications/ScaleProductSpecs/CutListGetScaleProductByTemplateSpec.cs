using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using System.Collections.Generic;
using System.Linq;

namespace Greta.BO.BusinessLogic.Specifications.ScaleProductSpecs;

/// <summary>
/// Get ScaleProduct by templateId
/// </summary>
public sealed class CutListGetScaleProductByTemplateSpec : Specification<ScaleProduct>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="idTemplate"></param>    
    public CutListGetScaleProductByTemplateSpec(long idTemplate)
    {
        Query.Include(x => x.CutListTemplates);        
        Query.Where(x => x.CutListTemplates.Any(x => x.Id == idTemplate));
    }
}