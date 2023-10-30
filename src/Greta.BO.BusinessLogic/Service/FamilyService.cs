using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.BO.BusinessLogic.Specifications.FamilySpecs;
using Greta.Sdk.Core.Models.Pager;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;

/// <summary>
/// Service layer for family entity
/// </summary>
public interface IFamilyService : IGenericBaseService<Family>
{
    /// <summary>
    /// Determine if this family entity can be deleted
    /// </summary>
    /// <param name="id">Family Id</param>
    /// <returns>Return true if this family don't have any product associated</returns>
    Task<bool> CanDeleted(long id);

    /// <summary>
    /// Add a list of product to this family
    /// </summary>
    /// <param name="id">Family Id</param>
    /// <param name="uPCs">List of upc of the product to add</param>
    /// <returns></returns>
    Task<string> AddProductsToFamily(long id, List<string> uPCs);

    /// <summary>
    /// filter product associated to one family id
    /// </summary>
    /// <param name="currentPage">Current page</param>
    /// <param name="pageSize">Page Size</param>
    /// <param name="spec">specification</param>
    /// <returns></returns>
    Task<Pager<Product>> FilterFamily(
        int currentPage,
        int pageSize,
        ProductByFamilyIdSpec spec
    );

    /// <summary>
    /// Remove association of one product from one family
    /// </summary>
    /// <param name="familyId">Family Id</param>
    /// <param name="productId">Product Id</param>
    /// <returns></returns>
    Task<bool> DeleteProduct(long familyId, long productId);

    /// <summary>
    /// Remove a list of products from a family
    /// </summary>
    /// <param name="familyId">Family Id</param>
    /// <param name="productIds">List of product id</param>
    /// <returns></returns>
    Task<bool> DeleteRangeProduct(long familyId, List<long> productIds);
}

/// <inheritdoc cref="Greta.BO.BusinessLogic.Service.IFamilyService" />
public class FamilyService : BaseService<IFamilyRepository, Family>, IFamilyService
{
    /// <inheritdoc />
    public FamilyService(IFamilyRepository familyRepository, ILogger<FamilyService> logger,
        ISynchroService synchroService)
        : base(familyRepository, logger, synchroService, Converter)
    {
    }

    private static object Converter(Family from) => (LiteFamily.Convert(from));
    
    /// <inheritdoc />
    public async Task<bool> CanDeleted(long id)
    {
        return await _repository.GetEntity<Family>()
            .AnyAsync(e => e.Id == id && !e.Products.Any());
    }

    /// <inheritdoc />
    public async Task<string> AddProductsToFamily(long id, List<string> uPCs)
    {
        var productsIncludeFamily = await _repository.GetEntity<Product>()
            .Where(x => uPCs.Contains(x.UPC))
            .ToListAsync();

        foreach (var item in productsIncludeFamily)
        {
            item.FamilyId = id;
        }

        _repository.GetEntity<Product>().UpdateRange(productsIncludeFamily);

        await _repository.GetContext<SqlServerContext>().SaveChangesAsync();
        return productsIncludeFamily.Count == uPCs.Count
            ? null
            : "Some products could not be found in the selected store.";
    }

    /// <inheritdoc />
    public async Task<Pager<Product>> FilterFamily(
        int currentPage,
        int pageSize,
        ProductByFamilyIdSpec spec
    )
    {
        if (currentPage >= 1 && pageSize >= 1)
            return await _repository.GetEntity<Product>().WithSpecification(spec)
                .ToPageAsync(currentPage, pageSize);
        _logger.LogError("Page parameter (currentPage or pageSize) out of bounds");
        throw new BusinessLogicException("Page parameter out of bounds.");
    }

    /// <inheritdoc />
    public async Task<bool> DeleteProduct(long familyId, long productId)
    {
        var elem = await _repository.GetEntity<Product>()
            .Where(x => x.Id == productId && x.FamilyId == familyId)
            .FirstOrDefaultAsync();
        if (elem != null)
        {
            elem.FamilyId = null;
            _repository.GetEntity<Product>().Update(elem);
            await _repository.GetContext<SqlServerContext>().SaveChangesAsync();
        }

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteRangeProduct(long familyId, List<long> productIds)
    {
        var elems = await _repository.GetEntity<Product>()
            .Where(x => productIds.Contains(x.Id) && x.FamilyId == familyId)
            .ToListAsync();
        foreach (var e in elems)
        {
            e.FamilyId = null;
        }

        _repository.GetEntity<Product>().UpdateRange(elems);
        await _repository.GetContext<SqlServerContext>().SaveChangesAsync();
        return true;
    }
}