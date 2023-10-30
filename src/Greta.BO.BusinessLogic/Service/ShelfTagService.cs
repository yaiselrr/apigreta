using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;

/// <summary>
/// Service layer for shelfTag entity
/// </summary>
public interface IShelfTagService : IGenericBaseService<ShelfTag>
{
    /// <summary>
    /// Create entities from Category Id
    /// </summary>
    /// <param name="categoryId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="BusinessLogicException"></exception>
    Task<bool> PostFromCategory(long categoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create entity from Storeroduct object
    /// </summary>
    /// <param name="storeProductId">StoreProductId</param>
    /// <param name="cancellationToken"></param>
    /// <returns>shelfTag object</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    Task<ShelfTag> PostFromStoreProduct(long storeProductId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create entity from Storeroduct object
    /// </summary>
    /// <param name="vendorProductId">VendorProductId</param>
    /// <param name="cancellationToken"></param>
    /// <returns>shelfTag object</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    Task<ShelfTag> PostFromVendorProduct(long vendorProductId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Update entity
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="qty"></param>
    /// <returns>Boolean success</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    Task<bool> PutByQty(long id, int qty);

    /// <summary>
    ///     Delete a list
    /// </summary>
    /// <param name="storeId">StoreId</param>
    /// <returns>Boolean success</returns>
    /// <exception cref="BusinessLogicException">If list is null or empty</exception>
    Task<bool> DeleteByStore(long storeId);
}

/// <inheritdoc cref="Greta.BO.BusinessLogic.Service.IShelfTagService" />
public class ShelfTagService : BaseService<IShelfTagRepository, ShelfTag>, IShelfTagService
{
    /// <inheritdoc />
    public ShelfTagService(IShelfTagRepository storeRepository, ILogger<ShelfTagService> logger)
        : base(storeRepository, logger)
    {
    }

    /// <inheritdoc />
    public async Task<bool> PostFromCategory(long categoryId, CancellationToken cancellationToken = default)
    {
        if (categoryId < 1)
        {
            this._logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        var storeProducts = await _repository.GetEntity<StoreProduct>()
            .Include(sp => sp.Store)
            .Include(sp => sp.Product)
            .ThenInclude(p => p.VendorProducts).ThenInclude(vp => vp.Vendor)
            .Include(sp => sp.Product.Category)
            .Include(sp => sp.Product.Department)
            .Include(sp => sp.BinLocation)
            .Where(sp => sp.Product.CategoryId == categoryId && sp.Product.State)
            .ToListAsync(cancellationToken: cancellationToken);

        var resultShelfTags = storeProducts.Select(x =>
        {
            VendorProduct vp = null;

            if (x.Product.VendorProducts is { Count: 1 })

                vp = x.Product.VendorProducts[0];
            else if (x.Product.VendorProducts != null)
                vp = x.Product.VendorProducts.FirstOrDefault(vp => vp.IsPrimary);

            return new ShelfTag()
            {
                StoreId = x.StoreId,
                StoreName = x.Store.Name,
                ProductId = x.ProductId,
                ProductName = x.Product.Name,
                UPC = x.Product.UPC,
                BinLocationId = (x.BinLocation != null) ? x.BinLocationId!.Value : 0,
                BinLocationName = x.BinLocation?.Name,
                Price = x.Price,
                DepartmentId = x.Product.Department?.Id ?? 0,
                DepartmentName = x.Product.Department?.Name,
                CategoryId = x.Product.Category?.Id ?? 0,
                CategoryName = x.Product.Category?.Name,
                VendorId = (vp != null) ? vp.Id : 0,
                VendorName = (vp != null) ? vp.Vendor.Name : string.Empty,
                ProductCode = (vp != null) ? vp.ProductCode : string.Empty,
                CasePack = (vp != null) ? vp.CasePack : 0,
            };
        }).ToList();
        var response = await _repository.CreateRangeAsync(resultShelfTags, cancellationToken);
        return response.Count == storeProducts.Count;
    }

    /// <inheritdoc />
    public async Task<ShelfTag> PostFromStoreProduct(long storeProductId,
        CancellationToken cancellationToken = default)
    {
        if (storeProductId < 1)
        {
            this._logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        // var storeProduct = await _repository.GetEntity<StoreProduct>()
        //     .Include(sp => sp.Store)
        //     .Include(sp => sp.Product)
        //     .ThenInclude(p => p.VendorProducts).ThenInclude(vp => vp.Vendor)
        //     .Include(sp => sp.Product.Category)
        //     .Include(sp => sp.Product.Department)
        //     .Include(sp => sp.BinLocation)
        //     .Where(sp => sp.Id == storeProductId)
        //     .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        
        var storeProduct = await _repository.GetEntity<StoreProduct>()
                .Where(x => x.Id == storeProductId)
                .Include(x => x.Product)
                .Include(sp => sp.Store)
                .FirstOrDefaultAsync();

        if (storeProduct != null)
        {
            VendorProduct vp = null;

            if (storeProduct.Product.VendorProducts is { Count: 1 })

                vp = storeProduct.Product.VendorProducts[0];
            else if (storeProduct.Product.VendorProducts != null)
                vp = storeProduct.Product.VendorProducts.FirstOrDefault(vp => vp.IsPrimary);

            var shelfTag = new ShelfTag()
            {
                StoreId = storeProduct.StoreId,
                StoreName = storeProduct.Store.Name,
                ProductId = storeProduct.ProductId,
                ProductName = storeProduct.Product.Name,
                UPC = storeProduct.Product.UPC,
                BinLocationId = (storeProduct.BinLocation != null) ? storeProduct.BinLocationId.Value : 0,
                BinLocationName = storeProduct.BinLocation?.Name,
                Price = storeProduct.Price,
                DepartmentId = storeProduct.Product.Department?.Id ?? 0,
                DepartmentName = storeProduct.Product.Department?.Name,
                CategoryId = storeProduct.Product.Category?.Id ?? 0,
                CategoryName = storeProduct.Product.Category?.Name,
                VendorId = (vp != null) ? vp.Id : 0,
                VendorName = (vp != null) ? vp.Vendor.Name : string.Empty,
                ProductCode = (vp != null) ? vp.ProductCode : string.Empty,
                CasePack = (vp != null) ? vp.CasePack : 0,
            };


            return await base.Post(shelfTag);
        }

        return null;
    }

    /// <inheritdoc />
    public async Task<ShelfTag> PostFromVendorProduct(long vendorProductId,
        CancellationToken cancellationToken = default)
    {
        if (vendorProductId < 1)
        {
            this._logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        var vendorProduct = await _repository.GetEntity<VendorProduct>()
            .Include(vp => vp.Product)
            .Include(vp => vp.Vendor)
            .Where(vp => vp.Id == vendorProductId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (vendorProduct != null)
        {
            var shelfTag = new ShelfTag()
            {
                ProductId = vendorProduct.ProductId,
                ProductName = vendorProduct.Product.Name,
                VendorId = vendorProduct.VendorId,
                VendorName = vendorProduct.Vendor.Name,
                ProductCode = vendorProduct.ProductCode,
                CasePack = vendorProduct.CasePack
            };

            return await base.Post(shelfTag);
        }

        return null;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteByStore(long storeId)
    {
        var entitiesDelete = await _repository.GetEntity<ShelfTag>()
            .Where(st => st.StoreId == storeId)
            .ToListAsync();

        if (entitiesDelete.Length() == 0)
        {
            _logger.LogError("List of ids is null or empty");
            throw new BusinessLogicException("List of ids is null or empty.");
        }

        // var data = await _repository.TransactionAsync(async context =>
        // {
        var success = await _repository.DeleteRangeAsync(entitiesDelete);
        return success;
        // });
        // return data;
    }

    /// <inheritdoc />
    public async Task<bool> PutByQty(long id, int qty)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        // return await _repository.TransactionAsync(async context =>
        // {
        var shelfTag = await _repository.GetEntity<ShelfTag>()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        shelfTag.QTYToPrint = qty;

        var response = await _repository.UpdateAsync(id, shelfTag);

        var data = await _repository.GetEntity<ShelfTag>()
            .FirstOrDefaultAsync(x => x.Id == id);

        return response;
        // });
    }
}