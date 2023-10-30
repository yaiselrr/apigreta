using System.Collections.Generic;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.BusinessLogic.Specifications.FeeSpecs;

/// <summary>
/// Fee Filter Specification
/// </summary>
public sealed class FeeGetByIdSpec : Specification<Fee>
{
    public FeeGetByIdSpec(long id)
    {
        Query.Where(x => x.Id == id);

        Query.Include(x => x.Products);
        Query.Include(x => x.Families);
        Query.Include(x => x.Categories);
    }
}