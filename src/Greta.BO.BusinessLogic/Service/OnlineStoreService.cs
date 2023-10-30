using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Events.Internal.OnlineStores;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.BO.BusinessLogic.Specifications.OnlineStoreSpec;
using Greta.BO.BusinessLogic.Specifications.OnlineStoreSpecs;
using Greta.BO.Wix.Handlers.Commands.OnlineStores;
using Greta.Sdk.EFCore.Extensions;
using MassTransit.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;

/// <summary>
/// Service layer for online store entity
/// </summary>
public interface IOnlineStoreService : IGenericBaseService<OnlineStore>
{
    /// <summary>
    /// Determine if this online store entity can be deleted
    /// </summary>
    /// <param name="id">online store Id</param>
    /// <returns></returns>
    Task<bool> CanDeleted(long id);
    
    /// <summary>
    /// Determine if this online store entity can be deleted of list
    /// </summary>
    /// <param name="ids">online store ids</param>
    /// <returns></returns>
    /// 
    Task<bool> CanDeleted(List<long> ids);

    /// <summary>
    ///     Get wix store tokens
    /// </summary>
    /// <param name="stores"></param>
    /// <param name="departmentId">department Id</param>
    /// <param name="cancellationToken">cancellation Token</param>
    /// <returns></returns>
    Task<List<OnlineStore>> GetWixStoreTokens(List<long> stores, long departmentId, CancellationToken cancellationToken = default);
    /// <summary>
    ///     Get wix store token
    /// </summary>
    /// <param name="storeId">store Id</param>
    /// <returns></returns>
    Task<string> GetWixStoreToken(long storeId);

    /// <summary>
    ///     Create category online
    /// </summary>
    /// <param name="categoryId">category Id </param>
    /// <param name="onlineStoreId">onlineStore Id </param>
    /// <param name="onlineId">online Id </param>
    /// <returns></returns>
    Task<bool> CreateCategoryOnline(long categoryId, long onlineStoreId, string onlineId);

    /// <summary>
    ///     Ge online category by id for store
    /// </summary>
    /// <param name="categoryId">category Id </param>
    /// <returns></returns>
    Task<OnlineCategory> GetOnlineCategoryByIdForStore(long categoryId);

    /// <summary>
    ///     Create product online
    /// </summary>
    /// <param name="productId">product Id </param>
    /// <param name="onlineStoreId">onlineStore Id </param>
    /// <param name="onlineId">online Id </param>
    /// <returns></returns>
    Task<bool> CreateProductOnline(long productId, long onlineStoreId, string onlineId);

    /// <summary>
    ///     Ge online category by id for store
    /// </summary>
    /// <param name="categoryId">category Id </param>
    /// <param name="onlineStoreId">online store Id </param>
    /// <returns></returns>
    Task<OnlineCategory> GetOnlineCategoryForStore(long categoryId, long onlineStoreId);

    /// <summary>
    ///     Ge online category by id for store
    /// </summary>
    /// <param name="categoryId">category Id </param>
    /// <param name="onlineStoreId">online store Id </param>
    /// <returns></returns>
    Task<string> GetOnlineCategoryIdForStore(long categoryId, long onlineStoreId);
    /// <summary>
    ///     Ge online product for store
    /// </summary>
    /// <param name="productId">product Id </param>
    /// <param name="onlineStoreId">online store Id </param>
    /// <returns></returns>
    Task<OnlineProduct> GetOnlineProductForStore(long productId, long onlineStoreId);

    /// <summary>
    ///     Ge online product for store
    /// </summary>
    /// <param name="productId">product Id </param>
    /// <param name="onlineStoreId">online store Id </param>
    /// <returns></returns>
    Task<long> GetOnlineProductIdForStore(long productId, long onlineStoreId);

    /// <summary>
    ///     Ge category by id
    /// </summary>
    /// <param name="id">product Id </param>
    /// <returns></returns>
    Task<Category> GetCategoryWithProduct(long id);

    /// <summary>
    ///     Ge category by id
    /// </summary>
    /// <param name="id">product Id </param>
    /// <param name="cancellationToken">cancellationToken </param>
    /// <returns></returns>
    Task<List<Category>> GetCategoriesWithProduct(long id, CancellationToken cancellationToken = default);
}

/// <inheritdoc cref="Greta.BO.BusinessLogic.Service.IOnlineStoreService" />
public class OnlineStoreService : BaseService<IOnlineStoreRepository, OnlineStore>, IOnlineStoreService
{
    private readonly IMediator _mediator;
    private readonly IProductRepository _productRepository;

    /// <inheritdoc />
    public OnlineStoreService(IOnlineStoreRepository onlineStoreRepository, ILogger<OnlineStoreService> logger)
        : base(onlineStoreRepository, logger)
    {
    }
    /// <inheritdoc />
    public OnlineStoreService(IOnlineStoreRepository onlineStoreRepository, ILogger<OnlineStoreService> logger, IMediator mediator, IProductRepository productRepository)
        : base(onlineStoreRepository, logger)
    {
        _mediator = mediator;
        _productRepository = productRepository;
    }

    private static string Converter(OnlineStore from) =>
        SynchroDetailRepository.DefaultConverter(LiteOnlineStore.Convert(from));

    /// <inheritdoc />
    public async Task<bool> CanDeleted(long id)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds.");
            throw new BusinessLogicException("Id parameter out of bounds.");
        }

        var entity = await _repository.GetEntity<OnlineStore>()
                                      .WithSpecification(new OnlineStoreGetByIdSpec(id))
                                      .FirstOrDefaultAsync();

        var success = await _repository.DeleteAsync(id);

        return success;
    }

    /// <inheritdoc />
    public async Task<bool> CanDeleted(List<long> ids)
    {
        if (ids == null || ids.Count == 0)
        {
            _logger.LogError("List of ids is null or empty.");
            throw new BusinessLogicException("List of ids is null or empty.");
        }

        foreach (var id in ids)
        {
            var entity = await _repository.GetEntity<OnlineStore>()
                                      .WithSpecification(new OnlineStoreGetByIdSpec(id))
                                      .FirstOrDefaultAsync();

            await Delete(id);
        }

        return true;
    }

    /// <summary>
    ///     Get entity by Id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>ExternalScale</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    public override async Task<OnlineStore> Get(long id)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds.");
            throw new BusinessLogicException("Id parameter out of bounds.");
        }
        var entity = await _repository.GetEntity<OnlineStore>()
                                      .WithSpecification(new OnlineStoreGetByIdSpec(id))
                                      .FirstOrDefaultAsync();
        return entity;
    }

    /// <summary>
    ///     Insert a entity
    /// </summary>
    /// <param name="entity">Entity to insert</param>
    /// <returns>Entity</returns>
    public override async Task<OnlineStore> Post(OnlineStore entity)
    {
        if (entity.NameWebsite == null || entity.NameWebsite == "" || entity.NameWebsite == "string")
            entity.NameWebsite = entity.Name;

        if (entity.Departments != null)
            foreach (var t in entity.Departments)
                _repository.GetEntity<Department>().Attach(t);

        entity.IsAssociated = false;
        entity.IsActiveWebSite = false;
        entity.Isdeleted = false;
        entity.IsAllowStorePickup = false;
        entity.IsStockUpdated = false;

        return await _repository.TransactionAsync(async context =>
        {
            var id = await _repository.CreateAsync(entity);

            entity.Id = id;

            var data = await _repository.GetEntity<OnlineStore>()
                                        .WithSpecification(new OnlineStoreGetByIdSpec(id))
                                        .FirstOrDefaultAsync();

            return data;
        });
    }

    /// <summary>
    ///     Update entity
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="entity">Entity to update</param>
    /// <returns>Boolean success</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    public override async Task<bool> Put(long id, OnlineStore entity)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        return await _repository.TransactionAsync(async context =>
        {
            var onlineStore = await _repository.GetEntity<OnlineStore>()
                                               .WithSpecification(new OnlineStoreGetByIdSpec(id))
                                               .FirstOrDefaultAsync();

            await _repository.UpdateAsync(id, onlineStore);

            entity.Departments = ProcessMany2ManyUpdate(onlineStore.Departments, entity.Departments);

            var response = await _repository.UpdateAsync(id, entity);
            var data = await _repository.GetEntity<OnlineStore>()
                                        .WithSpecification(new OnlineStoreGetByIdSpec(id))
                                        .FirstOrDefaultAsync();

            return response;
        });
    }

    
    /// <inheritdoc />
    public async Task<List<OnlineStore>> GetWixStoreTokens(
        List<long> stores,
        long departmentId,
        CancellationToken cancellationToken = default)
    {
        return await _repository.GetEntity<OnlineStore>()
            .WithSpecification(new OnlineStoreGetStoreTokensSpec(stores, departmentId))
            .Select(x => new OnlineStore()
            {
                Id = x.Id,
                RefreshToken = x.RefreshToken,
                StoreId = x.StoreId
            }).ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<string> GetWixStoreToken(long storeId)
    {
        return await _repository.GetEntity<OnlineStore>()
            .WithSpecification(new OnlineStoreGetStoreTokensIdSpec(storeId))
            .Select(x => x.RefreshToken).FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<bool> CreateCategoryOnline(long categoryId, long onlineStoreId, string onlineId)
    {
        var onlineCategory = new OnlineCategory
        {
            OnlineStoreId = onlineStoreId,
            CategoryId = categoryId,
            OnlineCategoryId = onlineId,
            State = true
        };

        var result = await _repository.GetEntity<OnlineCategory>().AddAsync(onlineCategory);

        await _repository.GetContext<SqlServerContext>().SaveChangesAsync();

        return result.State == EntityState.Added;
    }

    /// <inheritdoc />
    public async Task<bool> CreateProductOnline(long productId, long onlineStoreId, string onlineId)
    {
        var onlineProduct = new OnlineProduct()
        {
            OnlineStoreId = onlineStoreId,
            ProductId = productId,
            OnlineProductId = onlineId,
            State = true
        };

        var result = await _repository.GetEntity<OnlineProduct>().AddAsync(onlineProduct);

        await _repository.GetContext<SqlServerContext>().SaveChangesAsync();

        return result.State == EntityState.Added;
    }

    /// <inheritdoc />
    public async Task<OnlineCategory> GetOnlineCategoryForStore(long categoryId, long onlineStoreId)
    {
        return await _repository.GetEntity<OnlineCategory>()
            .WithSpecification(new OnlineStoreGetOnlineCategoryForStoreSpec(categoryId, onlineStoreId))
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<string> GetOnlineCategoryIdForStore(long categoryId, long onlineStoreId)
    {
        return await _repository.GetEntity<OnlineCategory>()
            .WithSpecification(new OnlineStoreGetOnlineCategoryForStoreSpec(categoryId, onlineStoreId))
            .Select(x => x.OnlineCategoryId)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<OnlineProduct> GetOnlineProductForStore(long productId, long onlineStoreId)
    {
        return await _repository.GetEntity<OnlineProduct>()
            .WithSpecification(new OnlineStoreGetOnlineProductForStoreSpec(productId, onlineStoreId))
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<long> GetOnlineProductIdForStore(long productId, long onlineStoreId)
    {
        return await _repository.GetEntity<OnlineProduct>()
            .WithSpecification(new OnlineStoreGetOnlineProductForStoreSpec(productId, onlineStoreId))
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<OnlineCategory> GetOnlineCategoryByIdForStore(long categoryId)
    {
        return await _repository.GetEntity<OnlineCategory>()
            .WithSpecification(new OnlineStoreGetOnlineCategorySpec(categoryId))
            .FirstOrDefaultAsync();
    }
    
    /// <inheritdoc />
    public async Task<Category> GetCategoryWithProduct(long id)
    {
        return await _repository.GetEntity<Category>()
            .Include(e => e.Products).ThenInclude(x => x.StoreProducts)
            .AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
    }

    /// <inheritdoc />
    public async Task<List<Category>> GetCategoriesWithProduct(long departmentId, CancellationToken cancellationToken = default)
    {
        return await _repository.GetEntity<Category>()
            .Include(e => e.Products).ThenInclude(x => x.StoreProducts)
            .AsNoTracking().Where(x => x.DepartmentId == departmentId).ToListAsync(cancellationToken);
    }
}