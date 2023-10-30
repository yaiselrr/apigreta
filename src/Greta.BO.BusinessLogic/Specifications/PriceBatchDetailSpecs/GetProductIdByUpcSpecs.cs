using Ardalis.Specification;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Specifications.PriceBatchDetailSpecs;

/// <summary>
/// <inheritdoc/>
/// </summary>
public sealed class GetProductIdByUpcSpecs : Specification<Product>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="upc">Upc</param>
    public GetProductIdByUpcSpecs(string upc)
    {
        Query.Where(e => e.UPC == upc);
    }
}