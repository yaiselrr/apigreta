using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.Sdk.Core.Models.Pager;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;

/// <summary>
/// Service layer for store entity
/// </summary>
public interface IStoreService : IGenericBaseService<Store>
{
    /// <summary>
    /// Get Entity by id
    /// </summary>
    /// <param name="Id">Entity Id</param>
    /// <returns></returns>
    Task<Store> GetById(long Id);

    /// <summary>
    /// Get Entity by guid
    /// </summary>
    /// <param name="guid">Guid</param>
    /// <returns></returns>
    Task<Store> GetByGuid(Guid guid);

    /// <summary>
    ///     get a list of stores by region id
    /// </summary>
    /// <param name="RegionId">RegionId</param>
    /// <returns>List Stores</returns>
    Task<List<long>> GetByRegion(long RegionId);

    /// <summary>
    ///     get a list of stores id
    /// </summary>
    /// <returns>List Stores Id</returns>
    Task<List<long>> GetAllIds();

    /// <summary>
    ///     Filter and sort list of entities
    /// </summary>
    /// <param name="currentPage">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="search">basic searc string</param>
    /// <param name="regionId">sort string </param>
    /// <param name="status">sort string </param>
    /// <returns></returns>
    Task<Pager<Store>> GetWithStores(int currentPage, int pageSize, string search, long regionId,
        SynchroStatus status);

    /// <summary>
    ///     Update configuration entity
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="entity">Entity to update</param>
    /// <returns>Boolean success</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    Task<bool> PutConfiguration(long id, Store entity);

    /// <summary>
    /// Get Entity store by id
    /// </summary>
    /// <param name="storeId">Entity Id</param>
    /// <returns>Store</returns>
    Task<Store> GetWithStores(long storeId);

    /// <summary>
    ///     Update configuration entity
    /// </summary>
    /// <param name="store">Id</param>
    /// <param name="tag">Entity to update</param>
    /// <returns>Boolean success</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    Task UpdateLastCloseSynchro(long store, long tag);

    /// <summary>
    /// Get all Stores with External Scales
    /// </summary>
    /// <returns>Return list of entities</returns>
    Task<List<Store>> GetStoresWithExternalScales();

    /// <summary>
    /// Get all the Entities
    /// </summary>
    /// <returns>Return list of entities</returns>
    Task<List<StoreDashBoardDto>> GetForDashboard();

    /// <summary>
    /// Get Entity by id with taxes
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Store> GetWithTaxesById(long id);
}

/// <inheritdoc cref="Greta.BO.BusinessLogic.Service.IStoreService" />
public class StoreService : BaseService<IStoreRepository, Store>, IStoreService
{
    /// <inheritdoc />
    public StoreService(IStoreRepository storeRepository, ILogger<StoreService> logger)
        : base(storeRepository, logger)
    {
    }

    private readonly List<string> blacklistForConfiguration = new List<string>()
    {
        nameof(Store.Id),
        nameof(Store.CreatedAt),
        nameof(Store.UserCreatorId),
        nameof(Store.State),
        nameof(Store.SynchroVersion),
        nameof(Store.RegionId),
        nameof(Store.Region),
        nameof(Store.LastBackupTime),
        nameof(Store.Updated),
        nameof(Store.LastBackupPath),
        nameof(Store.LastBackupVersion),
        nameof(Store.SynchroVersion),
        nameof(Store.Devices),
        nameof(Store.Synchros),
        nameof(Store.Taxs),
        nameof(Store.GuidId)
    };

    /// <inheritdoc />
    public async Task<List<long>> GetAllIds()
    {
        return await _repository.GetEntity<Store>()
            .Where(x => x.State && !x.IsDeleted)
            .Select(x => x.Id).ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Pager<Store>> GetWithStores(
        int currentPage,
        int pageSize,
        string search,
        long regionId,
        SynchroStatus status)
    {
        return await _repository.GetEntity<Store>()
            .Include(x => x.Devices)
            .Include(x => x.Synchros)
            .Include(x => x.Region)
            .ToPageAsync(currentPage, pageSize);
    }

    /// <inheritdoc />
    public async Task<Store> GetWithStores(long storeId)
    {
        return await _repository.GetEntity<Store>()
            .Include(x => x.Devices)
            .Include(x => x.Synchros)
            .Include(x => x.Region)
            .Where(x => x.Id == storeId)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<Store> GetById(long Id)
    {
        return await _repository.GetEntity<Store>()
            .IgnoreQueryFilters()
            .Include(e => e.Region)
            .FirstOrDefaultAsync(x => x.Id == Id);
    }
    
    /// <inheritdoc />
    public async Task<Store> GetWithTaxesById(long id)
    {
        return await _repository.GetEntity<Store>()
            .Include(x => x.Taxs)
            .Include(e => e.Region)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <inheritdoc />
    public async Task<List<long>> GetByRegion(long RegionId)
    {
        return await _repository.GetEntity<Store>()
            .Where(x => x.RegionId == RegionId).Select(x => x.Id).ToListAsync();
    }

    /// <inheritdoc />
    public async Task<bool> PutConfiguration(long id, Store entity)
    {
        if (id < 1)
        {
            this._logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        var entityUpdated = await _repository.GetEntity<Store>().FindAsync(id);

        if (entityUpdated != null)
        {
            var properties = typeof(Store).GetProperties().Where(x => !this.blacklistForConfiguration.Contains(x.Name))
                .ToList();

            foreach (var property in properties)
            {
                var value = entity.GetType().GetProperty(property.Name)?.GetValue(entity);

                if (value != null)
                    entityUpdated.GetType().GetProperty(property.Name)?.SetValue(entityUpdated, value, null);
            }

            return await base.Put(id, entityUpdated);
        }

        return false;
    }

    /// <inheritdoc />
    public async Task<Store> GetByGuid(Guid guid)
    {
        return await _repository.GetEntity<Store>()
            .Where(x => x.GuidId == guid).FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task UpdateLastCloseSynchro(long store, long tag)
    {
        var storeObj = await _repository.GetEntity<Store>()
            .Where(x => x.Id == store).FirstOrDefaultAsync();

        storeObj.LastBackupVersion = (int)tag;
        storeObj.LastBackupTime = DateTime.UtcNow;
        await base.Put(store, storeObj);
    }

    /// <inheritdoc />
    public async Task<List<Store>> GetStoresWithExternalScales()
    {
        return await _repository.GetEntity<Store>()
            .Where(x => x.ExternalScales.Count > 0).ToListAsync();
    }

    /// <inheritdoc />
    public async Task<List<StoreDashBoardDto>> GetForDashboard()
    {
        return await _repository.GetEntity<Store>()
            .Include(x => x.Devices)
            .Select(x => new StoreDashBoardDto()
            {
                Id = x.Id,
                Name = x.Name,
                Devices = x.Devices.Select(d => new DeviceDashBoardDto()
                {
                    Id = d.Id,
                    Name = d.Name,
                    Connected = d.SignalRConnectionId != null
                }).ToList()
            }).ToListAsync();
    }
}