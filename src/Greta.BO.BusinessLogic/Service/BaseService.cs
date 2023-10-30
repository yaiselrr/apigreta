using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Attributes;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.Sdk.Core.Abstractions;
using Greta.Sdk.Core.Models.Pager;
using Greta.Sdk.EFCore.Extensions;
using Greta.Sdk.EFCore.Interfaces;
using Greta.Sdk.EFCore.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;
/// <summary>
/// Base implementation for <exception cref="IGenericBaseService{T}"></exception>
/// </summary>
/// <typeparam name="TRepository"></typeparam>
/// <typeparam name="TEntity"></typeparam>
public class BaseService<TRepository, TEntity>
    where TEntity : class, IEntityBase<long, string>
    where TRepository : IOperationBase<long, string, TEntity>
{
    protected readonly ILogger _logger;
    protected readonly TRepository _repository;
    protected readonly ISynchroService synchroService;
    protected Func<TEntity, object> _converter = null;

    protected BaseService(TRepository repository, ILogger logger, Func<TEntity, object> converter = null)
    {
        _repository = repository;
        _logger = logger;
        _converter = converter;
    }

    protected BaseService(TRepository repository, ILogger logger, ISynchroService synchroService,
        Func<TEntity, object> converter = null)
        : this(repository, logger)
    {
        this.synchroService = synchroService;
        _converter = converter;
    }

    /// <summary>
    /// Generate a csv string format for a list of entities
    /// </summary>
    /// <param name="rows"></param>
    /// <typeparam name="TEntityCsv"></typeparam>
    /// <returns></returns>
    protected string GenerateCsv<TEntityCsv>(IEnumerable<TEntityCsv> rows) where TEntityCsv : BaseEntityLong
    {
        var fieldInfo = BaseEntityLong.GetFieldInto<TEntityCsv>().ToArray();
        var stringBuilder = new StringBuilder();

        //Generate header
        stringBuilder.AppendLine(string.Join(",", fieldInfo.Select(info => info.field.Header)));

        // generate rows
        PropertyInfo lastPropertyInfo = fieldInfo.Last().property;
        foreach (TEntityCsv row in rows)
        {
            foreach ((PropertyInfo currentProperty, FieldInfoAttribute field) in fieldInfo)
            {
                stringBuilder.Append(field.Format(currentProperty.GetValue(row)));
                if (lastPropertyInfo != currentProperty) stringBuilder.Append(',');
            }

            stringBuilder.Append(Environment.NewLine);
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// Filter a list of entities
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IReadOnlyList<TEntity>> Get(Specification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(specification, cancellationToken);
    }

    /// <summary>
    /// Filter a list of entities and return a result model
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IReadOnlyList<TResult>> Get<TResult>(Specification<TEntity, TResult> specification,
        CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(specification, cancellationToken);
    }

    /// <summary>
    /// Get a single item or default
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TEntity> Get(SingleResultSpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        return await _repository.SingleOrDefaultAsync(specification, cancellationToken);
    }

    /// <summary>
    /// Get a single item or default
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResult> Get<TResult>(SingleResultSpecification<TEntity, TResult> specification,
        CancellationToken cancellationToken = default)
    {
        return await _repository.SingleOrDefaultAsync(specification, cancellationToken);
    }

    /// <summary>
    ///     Filter and sort list of entities
    /// </summary>
    /// <param name="currentPage">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="filter">Filter params</param>
    /// <param name="searchstring">basic searc string</param>
    /// <param name="sortstring">sort string </param>
    /// <returns></returns>
    [Obsolete]
    public async Task<Pager<TEntity>> Filter(int currentPage, int pageSize, TEntity filter, string searchstring,
        string sortstring)
    {
        if (currentPage < 1 || pageSize < 1)
        {
            _logger.LogError("Page parameter (currentPage or pageSize) out of bounds");
            throw new BusinessLogicException("Page parameter out of bounds");
        }

        var splited = string.IsNullOrEmpty(sortstring) ? new[] { "", "" } : sortstring.Split("_");

        var query = _repository.GetEntity<TEntity>();
        var query1 = FilterqueryBuilder(filter, searchstring, splited, query);

        var entities = await query1.ToPageAsync(currentPage, pageSize);
        return entities;
    }

    /// <summary>
    ///     Function to return filter
    /// </summary>
    /// <param name="filter">Entity to filter</param>
    /// <param name="searchstring">String to bassic filter</param>
    /// <param name="sortSplited">sort data</param>
    /// <param name="query">Dataset base to return</param>
    /// <returns></returns>
    [Obsolete]
    protected virtual IQueryable<TEntity> FilterqueryBuilder(TEntity filter, string searchstring,
        string[] sortSplited, DbSet<TEntity> query)
    {
        return query;
    }

    /// <summary>
    ///     Function to return filter
    /// </summary>
    /// <param name="pageSize">Data amount</param>
    /// <param name="currentPage">Current page for skip data</param>
    /// <param name="spec">Specification</param>
    /// <param name="cancellationToken">Cancellation Token by default [default]</param>
    /// <returns></returns>
    public virtual Task<Pager<TEntity>> FilterSpec(
        int currentPage,
        int pageSize,
        ISpecification<TEntity> spec,
        CancellationToken cancellationToken = default)
    {
        if (currentPage >= 1 && pageSize >= 1)
            return _repository.PagesAsync(spec, currentPage, pageSize, cancellationToken);
        _logger.LogError("Page parameter (currentPage or pageSize) out of bounds");
        throw new BusinessLogicException("Page parameter out of bounds");
    }
    
    /// <summary>
    ///     Function to return filter
    /// </summary>
    /// <param name="pageSize">Data amount</param>
    /// <param name="currentPage">Current page for skip data</param>
    /// <param name="spec">Specification</param>
    /// <param name="cancellationToken">Cancellation Token by default [default]</param>
    /// <returns></returns>
    public virtual Task<Pager<TResult>> FilterSpec<TResult>(
        int currentPage,
        int pageSize,
        ISpecification<TEntity, TResult> spec,
        CancellationToken cancellationToken = default) where TResult: class, IDtoLong<string>
    {
        if (currentPage >= 1 && pageSize >= 1)  
            return _repository.PagesAsync(spec, currentPage, pageSize, cancellationToken);
        _logger.LogError("Page parameter (currentPage or pageSize) out of bounds");
        throw new BusinessLogicException("Page parameter out of bounds");
    }

    /// <summary>
    ///     Get all entities
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<TEntity>> Get()
    {
        if (typeof(TEntity).IsAssignableTo(typeof(ISoftDelete)))
        {
            return await _repository.GetEntity<TEntity>().Where(x => x.State && !((ISoftDelete)x).IsDeleted).ToListAsync();
        }
        else
        {
            return await _repository.GetEntity<TEntity>().Where(x => x.State).ToListAsync();
        }
    }

    /// <summary>
    ///     Get entity by Id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Customer</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    public virtual async Task<TEntity> Get(long id)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        var entity = await _repository.GetEntity<TEntity>().FindAsync(id);
        return entity;
    }

    /// <summary>
    ///     Insert a entity
    /// </summary>
    /// <param name="entity">Entity to insert</param>
    /// <returns>Entity</returns>
    public virtual async Task<TEntity> Post(TEntity entity)
    {
        if (entity is BaseLocationEntityLong elem)
        {
            elem.CountryName ??= await _repository.GetEntity<Country>().Where(x => x.Id == elem.CountryId)
                .Select(x => x.Name).FirstOrDefaultAsync();

            elem.ProvinceName ??= await _repository.GetEntity<Province>().Where(x => x.Id == elem.ProvinceId)
                .Select(x => x.Name).FirstOrDefaultAsync();
        }

        var ignoreSync = false;

        var hasExpire = entity.GetType().GetProperty("Expire");
        if (hasExpire != null)
        {
            var v = hasExpire.GetValue(entity);
            ignoreSync = (v != null && v is DateTime?);
        }

        if (!ignoreSync && synchroService != null && entity is IFullSyncronizable sync)
        {
            var data = await _repository.TransactionAsync(async context =>
            {
                var id = await _repository.CreateAsync(entity);
                entity.Id = id;

                var hasMultiplesStores = entity.GetType().GetProperty("Stores");
                if (hasMultiplesStores != null)
                {
                    //if have multiples stores only storage on this stores
                    var stores = (List<Store>)hasMultiplesStores.GetValue(entity);
                    if (stores != null) //&& stores.Count > 0)
                    {
                        await synchroService.AddSynchroToStores(stores.Select(x => x.Id).ToList(), entity,
                            SynchroType.CREATE, _converter);
                    }
                    // else
                    // {
                    //     await synchroService.AddSynchroToAllStores(entity, SynchroType.CREATE, _converter);
                    // }
                }
                else
                {
                    var hasOnlyOneStore = entity.GetType().GetProperty("StoreId");
                    if (hasOnlyOneStore != null)
                    {
                        var storeid = (long)hasOnlyOneStore.GetValue(entity);
                        if (storeid > 0)
                        {
                            await synchroService.AddSynchroToStore(storeid, entity, SynchroType.CREATE, _converter);
                        }
                        else
                        {
                            await synchroService.AddSynchroToAllStores(entity, SynchroType.CREATE, _converter);
                        }
                    }
                    else
                    {
                        await synchroService.AddSynchroToAllStores(entity, SynchroType.CREATE, _converter);
                    }
                }

                return entity;
            }, IsolationLevel.ReadCommitted);
            return data;
        }

        {
            var id = await _repository.CreateAsync(entity);
            entity.Id = id;
            return entity;
        }
    }

    public virtual async Task<bool> Put(long id, TEntity entity)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        if (entity is BaseLocationEntityLong elem)
        {
            elem.CountryName = await _repository.GetEntity<Country>().Where(x => x.Id == elem.CountryId)
                .Select(x => x.Name).FirstOrDefaultAsync();

            elem.ProvinceName = await _repository.GetEntity<Province>().Where(x => x.Id == elem.ProvinceId)
                .Select(x => x.Name).FirstOrDefaultAsync();
        }

        if (synchroService != null && entity is IFullSyncronizable sync)
        {
            var data = await _repository.TransactionAsync(async context =>
            {
                entity.Id = id;
                //await synchroService.AddSynchroToAllStores(entity, SynchroType.UPDATE, _converter);

                var hasMultiplesStores = entity.GetType().GetProperty("Stores");
                if (hasMultiplesStores != null)
                {
                    //if have multiples stores only storage on this stores
                    var stores = (List<Store>)hasMultiplesStores.GetValue(entity);
                    if (stores != null && stores.Count > 0)
                    {
                        await synchroService.AddSynchroToStores(stores.Select(x => x.Id).ToList(), entity,
                            SynchroType.UPDATE, _converter);
                    }
                    else
                    {
                        await synchroService.AddSynchroToAllStores(entity, SynchroType.UPDATE, _converter);
                    }
                }
                else
                {
                    var hasOnlyOneStore = entity.GetType().GetProperty("StoreId");
                    if (hasOnlyOneStore != null)
                    {
                        var storeid = (long)hasOnlyOneStore.GetValue(entity);
                        if (storeid > 0)
                        {
                            await synchroService.AddSynchroToStore(storeid, entity, SynchroType.UPDATE, _converter);
                        }
                        else
                        {
                            await synchroService.AddSynchroToAllStores(entity, SynchroType.UPDATE, _converter);
                        }
                    }
                    else
                    {
                        await synchroService.AddSynchroToAllStores(entity, SynchroType.UPDATE, _converter);
                    }
                }

                var success = await _repository.UpdateAsync(id, entity);

                return success;
            });
            return data;
        }
        else
        {
            var success = await _repository.UpdateAsync(id, entity);
            return success;
        }
    }

    public virtual async Task<bool> ChangeState(long id, bool state)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        var entity = await _repository.GetEntity<TEntity>()
            .Where(x => x.Id == id).FirstOrDefaultAsync();
        //if (synchroService != null && typeof(TEntity).IsAssignableTo(typeof(IFullSyncronizable))) 
        if (synchroService != null && entity is IFullSyncronizable sync)
        {
            var data = await _repository.TransactionAsync(async context =>
            {
                var success = await _repository.ChangeStateAsync<TEntity>(id, state);

                //await synchroService.AddSynchroToAllStores(entityUpdate, SynchroType.UPDATE, _converter);

                var hasMultiplesStores = entity.GetType().GetProperty("Stores");
                if (hasMultiplesStores != null)
                {
                    //if have multiples stores only storage on this stores
                    var stores = (List<Store>)hasMultiplesStores.GetValue(entity);
                    await synchroService.AddSynchroToStores(stores.Select(x => x.Id).ToList(), entity,
                        SynchroType.UPDATE, _converter);
                }
                else
                {
                    var hasOnlyOneStore = entity.GetType().GetProperty("StoreId");
                    if (hasOnlyOneStore != null)
                    {
                        var storeid = (long)hasOnlyOneStore.GetValue(entity);
                        await synchroService.AddSynchroToStore(storeid, entity, SynchroType.UPDATE, _converter);
                    }
                    else
                    {
                        await synchroService.AddSynchroToAllStores(entity, SynchroType.UPDATE, _converter);
                    }
                }

                return success;
            });
            return data;
        }
        else
        {
            var success = await _repository.ChangeStateAsync<TEntity>(id, state);
            return success;
        }
    }

    /// <summary>
    ///     Delete a entity
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Boolean success</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    public virtual async Task<bool> Delete(long id)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        if (synchroService != null && typeof(TEntity).IsAssignableTo(typeof(IFullSyncronizable)))
        {
            var data = await _repository.TransactionAsync(async context =>
            {
                var entitieDelete = await _repository.GetEntity<TEntity>()
                    .Where(x => x.Id == id).FirstOrDefaultAsync();

                if (entitieDelete == null) return false;
                var hasMultiplesStores = entitieDelete.GetType().GetProperty("Stores");
                if (hasMultiplesStores != null)
                {
                    var nE = await _repository.GetEntity<TEntity>()
                        .Where(x => x.Id == id).Include("Stores").FirstOrDefaultAsync();
                    //if have multiples stores only storage on this stores
                    var stores = (List<Store>)nE.GetType().GetProperty("Stores").GetValue(nE);
                    await synchroService.AddSynchroToStores(stores.Select(x => x.Id).ToList(), entitieDelete,
                        SynchroType.DELETE, _converter);
                }
                else
                {
                    var hasOnlyOneStore = entitieDelete.GetType().GetProperty("StoreId");
                    if (hasOnlyOneStore != null)
                    {
                        var storeid = (long)hasOnlyOneStore.GetValue(entitieDelete);
                        await synchroService.AddSynchroToStore(storeid, entitieDelete, SynchroType.DELETE,
                            _converter);
                    }
                    else
                    {
                        await synchroService.AddSynchroToAllStores(entitieDelete, SynchroType.DELETE, _converter);
                    }
                }
                var success = await _repository.DeleteAsync(id);
                return success;
            });
            return data;
        }

        {
            var success = await _repository.DeleteAsync(id);
            return success;
        }
    }

    /// <summary>
    ///     Delete a list
    /// </summary>
    /// <param name="ids">List of ids</param>
    /// <returns>Boolean success</returns>
    /// <exception cref="BusinessLogicException">If list is null or empty</exception>
    public virtual async Task<bool> DeleteRange(List<long> ids)
    {
        if (ids == null || ids.Count == 0)
        {
            _logger.LogError("List of ids is null or empty");
            throw new BusinessLogicException("List of ids is null or empty");
        }

        if (synchroService != null && typeof(TEntity).IsAssignableTo(typeof(IFullSyncronizable)))
        {
            var data = await _repository.TransactionAsync(async context =>
            {
                var entitiesDelete = await _repository.GetEntity<TEntity>()
                    .Where(x => ids.Contains(x.Id))
                    .ToListAsync();

                foreach (var t in entitiesDelete)
                    await synchroService
                        .AddSynchroToAllStores(
                            t, SynchroType.DELETE, _converter);

                var success = await _repository.DeleteRangeAsync(entitiesDelete);
                return success;
            });
            return data;
        }

        {
            var entitiesDelete = await _repository.GetEntity<TEntity>()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
            var success = await _repository.DeleteRangeAsync(entitiesDelete);
            return success;
        }
    }

    protected List<TEntityProcess> ProcessMany2ManyUpdate<TEntityProcess>(List<TEntityProcess> dbList,
        List<TEntityProcess> newElems, bool syncro = false)
        where TEntityProcess : BaseEntityLong
    {
        var newElemsLong = newElems.Select(x => x.Id).ToList();

        var addList = new List<long>();
        var removeList = new List<long>();
        var updateList = new List<long>();

        foreach (var ent in dbList.ToList())
            if (!newElemsLong.Contains(ent.Id))
            {
                dbList.Remove(ent);
                removeList.Add(ent.Id);
            }

        foreach (var ent in newElems)
            if (!dbList.Any(r => r.Id == ent.Id))
            {
                addList.Add(ent.Id);
                _repository.GetEntity<TEntityProcess>().Attach(ent);
                dbList.Add(ent);
            }
            else
            {
                updateList.Add(ent.Id);
            }

        return dbList;
    }
}