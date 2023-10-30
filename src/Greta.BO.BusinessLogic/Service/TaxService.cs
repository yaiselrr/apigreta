using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.Sdk.Core.Models.Pager;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;
/// <summary>
/// Service layer for tax entity
/// </summary>
public interface ITaxService : IGenericBaseService<Tax>
{
    /// <summary>
    ///     Get Entity by StoreId
    /// </summary>
    /// <param name="storeId">Store Id</param>
    /// <returns></returns>
    Task<List<Tax>> GetTaxByStore(long storeId);

    /// <summary>
    ///     Filter and sort list of entities
    /// </summary>
    /// <param name="currentPage">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="filter">Filter params</param>
    /// <param name="searchstring">basic searc string</param>
    /// <param name="sortstring">sort string </param>
    /// <returns></returns>
    Task<Pager<Tax>> FilterTax(int currentPage, int pageSize, TaxSearchModel filter, string searchstring,
            string sortstring);
}

/// <inheritdoc cref="Greta.BO.BusinessLogic.Service.ITaxService" />
public class TaxService : BaseService<ITaxRepository, Tax>, ITaxService
{

    /// <inheritdoc />
    public TaxService(
        ITaxRepository taxRepository,
        ISynchroService synchroService,
        ILogger<TaxService> logger)
        : base(taxRepository, logger, synchroService, Converter)
    {
    }

    private static object Converter(Tax from) => (LiteTax.Convert(from));

    /// <summary>
    ///     Insert a entity
    /// </summary>
    /// <param name="entity">Entity to insert</param>
    /// <returns>Entity</returns>
    public override async Task<Tax> Post(Tax entity)
    {
        //get the attached elements
        //tax
        return await _repository.TransactionAsync(async context =>
        {
            // var elem = new List<Store>();
            if (entity.Stores != null)
                for (var i = 0; i < entity.Stores.Count; i++)
                    _repository.GetEntity<Store>().Attach(entity.Stores[i]);
            // entity.Stores = elem;
            var result = await base.Post(entity);
            var elem = result.Stores.Select(x => x.Id).ToList();
            // result.Stores.Clear();
            var syncro = new Tax
            {
                Id = result.Id,
                Name = result.Name,
                Description = result.Description,
                Type = result.Type,
                Value = result.Value,
                SpecialValue = result.SpecialValue,
                State = result.State,
                Stores = result.Stores.Select(x => new Store { Id = x.Id }).ToList()
            };
            //await synchroService.AddSynchroToStores(elem, syncro, SynchroType.CREATE);//,converter: Converter);
            await synchroService.AddSynchroToStores(elem, LiteTax.Convert(syncro), SynchroType.CREATE);
            
            return result;
        });
    }

    /// <summary>
    ///     Update entity
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="entity">Entity to update</param>
    /// <returns>Boolean success</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    public override async Task<bool> Put(long id, Tax entity)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        var data = await _repository.TransactionAsync(async context =>
        {
            // remove all stores first
            var tax = await _repository.GetEntity<Tax>()
                .Include(x => x.Stores)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            //entity.Stores = ProcessMany2ManyUpdate<Store>(tax.Stores, entity.Stores);

            var longList = entity.Stores.Select(x => x.Id).ToList();
            var removeList = new List<long>();
            var addList = new List<long>();
            var updateList = new List<long>();
            if (tax != null)
            {
                foreach (var store in tax.Stores.ToList())
                    // Remove the roles which are not in the list of new roles
                    if (!longList.Contains(store.Id))
                    {
                        tax.Stores.Remove(store);
                        removeList.Add(store.Id);
                    }

                foreach (var newStoreId in longList)
                    // Add the roles which are not in the list of user's roles
                    if (!tax.Stores.Any(r => r.Id == newStoreId))
                    {
                        addList.Add(newStoreId);
                        var newRole = new Store { Id = newStoreId };
                        _repository.GetEntity<Store>().Attach(newRole);
                        tax.Stores.Add(newRole);
                    }
                    else
                    {
                        updateList.Add(newStoreId);
                    }

                entity.Stores.Clear();
                entity.Id = id;

                /*
                await synchroService.AddSynchroToStores(addList, entity, SynchroType.CREATE); //,converter: Converter);
                await synchroService.AddSynchroToStores(updateList, entity, SynchroType.UPDATE); //,converter: Converter);
                await synchroService.AddSynchroToStores(removeList, entity, SynchroType.DELETE); //,converter: Converter);
                */
                
                await synchroService.AddSynchroToStores(addList, LiteTax.Convert(entity), SynchroType.CREATE); //,converter: Converter);
                await synchroService.AddSynchroToStores(updateList, LiteTax.Convert(entity), SynchroType.UPDATE); //,converter: Converter);
                await synchroService.AddSynchroToStores(removeList, LiteTax.Convert(entity), SynchroType.DELETE); //,converter: Converter);
               
                
                entity.Stores = tax.Stores;
            }

            return await base.Put(id, entity);
        });
        return data;
    }

    /// <summary>
    ///     Get entity by Id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Customer</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    public override async Task<Tax> Get(long id)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        var entity = await _repository.GetEntity<Tax>().Include(x => x.Stores).Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        return entity;
    }

    /// <inheritdoc />
    public virtual async Task<List<Tax>> GetTaxByStore(long store)
    {
        var entities = await _repository.GetEntity<Tax>()
            .Where(x => x.Stores.Any(s => s.Id == store)).ToListAsync();
        return entities;
    }

    /// <summary>
    ///     Delete a entity
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Boolean success</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    public override async Task<bool> Delete(long id)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        var data = await _repository.TransactionAsync(async context =>
        {
            var entitieDelete = await _repository.GetEntity<Tax>()
                .Include(x => x.Stores)
                .Select(p => new Tax { Id = p.Id, Stores = p.Stores })
                .Where(x => x.Id == id).FirstOrDefaultAsync();

            if (entitieDelete != null)
                await synchroService
                    .AddSynchroToStores(
                        entitieDelete.Stores.Select(p => p.Id).ToList(),
                        LiteTax.Convert(entitieDelete),
                        SynchroType.DELETE);

            var success = await _repository.DeleteAsync(id);
            return success;
        });
        return data;
    }

    /// <summary>
    ///     Delete a list
    /// </summary>
    /// <param name="ids">List of ids</param>
    /// <returns>Boolean success</returns>
    /// <exception cref="BusinessLogicException">If list is null or empty</exception>
    public override async Task<bool> DeleteRange(List<long> ids)
    {
        if (ids == null || ids.Count == 0)
        {
            _logger.LogError("List of ids is null or empty");
            throw new BusinessLogicException("List of ids is null or empty.");
        }

        var data = await _repository.TransactionAsync(async context =>
        {
            var entitiesDelete = await _repository.GetEntity<Tax>()
                .Include(x => x.Stores)
                .Select(p => new Tax { Id = p.Id, Stores = p.Stores })
                .Where(x => ids.Contains(x.Id)).ToListAsync();
            foreach (var t in entitiesDelete)
                await synchroService.AddSynchroToStores(t.Stores.Select(p => p.Id).ToList(), LiteTax.Convert(t), SynchroType.DELETE);

            var success = await _repository.DeleteRangeAsync(entitiesDelete);
            return success;
        });
        return data;
    }

    /// <inheritdoc />
    public async Task<Pager<Tax>> FilterTax(int currentPage, int pageSize, TaxSearchModel filter, string searchstring, string sortstring)
    {
        if (currentPage < 1 || pageSize < 1)
        {
            _logger.LogError("Page parameter (currentPage or pageSize) out of bounds");
            throw new BusinessLogicException("Page parameter out of bounds.");
        }

        var splited = string.IsNullOrEmpty(sortstring) ? new[] { "", "" } : sortstring.Split("_");

        var query = _repository.GetEntity<Tax>().IgnoreAutoIncludes();

        if (!string.IsNullOrEmpty(searchstring))
            query = query.Where(c => c.Name.Contains(searchstring) || c.Description.Contains(searchstring));
        else
            query = query.WhereIf(!string.IsNullOrEmpty(filter.Name), c => c.Name.Contains(filter.Name))
                    .WhereIf(!string.IsNullOrEmpty(filter.Description),
                        c => c.Description.Contains(filter.Description));

        query = query
            .Switch(splited)
            .OrderByCase(e => e[0] == "name" && e[1] == "asc", e => e.Name)
            .OrderByDescendingCase(e => e[0] == "name" && e[1] == "desc", e => e.Name)
            .OrderByCase(e => e[0] == "description" && e[1] == "asc", e => e.Description)
            .OrderByDescendingCase(e => e[0] == "description" && e[1] == "desc", e => e.Description)
            .OrderByDefault(e => e.Name);

        var entities = await query.ToPageAsync(currentPage, pageSize);
        return entities;
    }
}