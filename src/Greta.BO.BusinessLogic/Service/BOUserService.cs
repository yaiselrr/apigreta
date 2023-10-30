using System;
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
using Greta.Identity.Api.EventContracts.BO.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;
/// <summary>
/// Service layer for BOUser entity
/// </summary>
public interface IBOUserService : IGenericBaseService<BOUser>
{
    Task<FilterUserResponseContract> FilterUserAsync(string userId, string application, BOUser filter,
        string searchstring, string sortstring, int CurrentPage, int PageSize);

    Task<BOUser> GetByUserId(string id);
    Task<BOUser> GetById(long id);
    Task<List<BOUser>> GetUsersBoByFilterAsync(List<string> longIds);
    Task<BOUser> GetDetachedById(long id);
    Task<bool> PutBase(long id, BOUser entity);
    Task<BOUser> GetByPin(string pin, long id = -1);
}

public class BOUserService : BaseService<IBOUserRepository, BOUser>, IBOUserService
{
    private readonly IRequestClient<FilterUserRequestContract> _client;

    public BOUserService(IBOUserRepository repository, ILogger<BOUserService> logger,
        IRequestClient<FilterUserRequestContract> client)
        : base(repository, logger)
    {
        _client = client;
    }

    public BOUserService(IBOUserRepository repository, ILogger<BOUserService> logger,
        ISynchroService synchroService,
        IRequestClient<FilterUserRequestContract> client)
        : base(repository, logger, synchroService, Converter)
    {
        _client = client;
    }
    private static object Converter(BOUser from) => LiteEmployee.Convert(from);


    public async Task<FilterUserResponseContract> FilterUserAsync(
        string userId,
        string application,
        BOUser filter,
        string searchstring,
        string sortstring,
        int CurrentPage,
        int PageSize)
    {
        //get filtered users list from identity
        var usersIdentity =
            (await _client.GetResponse<FilterUserResponseContract>(
                new
                {
                    __Header_user = userId,
                    __Header_application = application,
                    SearchString = searchstring,
                    SortString = sortstring,
                    filter.UserName,
                    filter.Email,
                    Application = application,
                    filter.PhoneNumber,
                    CurrentPage,
                    PageSize
                }));
        return usersIdentity?.Message;
    }

    public async Task<List<BOUser>> GetUsersBoByFilterAsync(
        List<string> longIds)
    {
        try
        {
            var usersBo =
                await _repository.GetEntity<BOUser>()
                    .Include(x => x.POSProfile)
                    .Include(x => x.BOProfile)
                    .Where(e => longIds.Contains(e.UserId))
                    .ToListAsync();
            return usersBo;
        }
        catch (Exception)
        {
            return null;
        }
    }


    /// <summary>
    ///     Get a BOUser by Global user id
    /// </summary>
    /// <param name="id">GLobal user id</param>
    /// <returns></returns>
    public async Task<BOUser> GetByUserId(string id)
    {
        return await _repository.GetEntity<BOUser>()
            .Include(x => x.Role)
            // .Include(x => x.POSProfile)
            .Include(x => x.BOProfile)
            .Include(x => x.BOProfile.Permissions)
            .Where(x => x.UserId == id)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    ///     Get a BOUser by Global user id
    /// </summary>
    /// <param name="id">GLobal user id</param>
    /// <returns></returns>
    public async Task<BOUser> GetById(long id)
    {
        return await _repository.GetEntity<BOUser>()
            .Include(x => x.Role)
            .Include(x => x.POSProfile)
            .Include(x => x.BOProfile)
            .Include(x => x.Stores)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }
    
    /// <summary>
    /// Chack if exist user with this pin
    /// </summary>
    /// <param name="pin"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<BOUser> GetByPin(string pin, long id = -1)
    {
        if (id == -1)
        {
            return await _repository.GetEntity<BOUser>()
                .Where(x => x.Pin == pin)
                .FirstOrDefaultAsync();
        }
        else
        {
            return await _repository.GetEntity<BOUser>()
                .Where(x => x.Pin == pin && x.Id != id)
                .FirstOrDefaultAsync();
        }
    }

    /// <summary>
    ///     Get a BOUser by Global user id
    /// </summary>
    /// <param name="id">GLobal user id</param>
    /// <returns></returns>
    public async Task<BOUser> GetDetachedById(long id)
    {
        return await _repository.GetEntity<BOUser>()
            .Include(x => x.Role)
            .Include(x => x.POSProfile)
            .Include(x => x.BOProfile)
            .Include(x => x.Stores)
            .Where(x => x.Id == id)
            .AsNoTrackingWithIdentityResolution()
            .FirstOrDefaultAsync();
    }

    /// <summary>
    ///     Get entity by Id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>BoUser</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    public override async Task<BOUser> Get(long id)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds.");
        }

        return await _repository.GetEntity<BOUser>()
            .Include(x => x.Role)
            .Include(x => x.POSProfile)
            .Include(x => x.BOProfile)
            .Include(x => x.Stores)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public override async Task<BOUser> Post(BOUser entity)
    {
        if (entity.POSProfileId == -1) entity.POSProfileId = null;
        if (entity.BOProfileId == -1) entity.BOProfileId = null;
        if (entity.RoleId == -1) entity.RoleId = null;

        return await _repository.TransactionAsync(async context =>
        {
            // var elem = new List<Store>();
            if (entity.Stores != null)
                for (var i = 0; i < entity.Stores.Count; i++)
                    _repository.GetEntity<Store>().Attach(entity.Stores[i]);
            // entity.Stores = elem;
            var temp = await base.Post(entity);
            var result = await _repository.GetEntity<BOUser>()
                .Include(x => x.Stores)
                .FirstOrDefaultAsync(x => x.Id == temp.Id);
            if (result.Stores is { Count: > 0 })
            {
                var elem = result.Stores.Select(x => x.Id).ToList();
                await synchroService.AddSynchroToStores(elem, LiteEmployee.Convert(result),
                    SynchroType.CREATE); //,converter: Converter);
            }

            return result;
        });
    }

    public async Task<bool> PutBase(long id, BOUser entity)
    {
        var storesId = entity.Stores.Select(x => x.Id).ToList();
        await synchroService.AddSynchroToStores(storesId, LiteEmployee.Convert(entity), SynchroType.UPDATE);
        return await base.Put(id, entity);
    }


    public override async Task<bool> Put(long id, BOUser entity)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds.");
            throw new BusinessLogicException("Id parameter out of bounds.");
        }

        if (entity.POSProfileId == -1) entity.POSProfileId = null;
        if (entity.BOProfileId == -1) entity.BOProfileId = null;
        if (entity.RoleId == -1) entity.RoleId = null;

        var data = await _repository.TransactionAsync(async context =>
        {
            // remove all stores first
            var user = await _repository.GetEntity<BOUser>()
                .Include(x => x.Stores)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            //entity.Stores = ProcessMany2ManyUpdate<Store>(tax.Stores, entity.Stores);

            //TODO
            //TODO: Remove this code after the first syncro
            //TODO
            if (user.Stores == null || user.Stores.Count == 0)
            {
                await synchroService.AddSynchroToAllStores(user, SynchroType.DELETE); //, _converter);
            }
            //TODO: End Remove this code after the first syncro

            var longList = entity.Stores.Select(x => x.Id).ToList();
            var removeList = new List<long>();
            var addList = new List<long>();
            var updateList = new List<long>();
            foreach (var store in user.Stores.ToList())
                // Remove the roles which are not in the list of new roles
                if (!longList.Contains(store.Id))
                {
                    user.Stores.Remove(store);
                    removeList.Add(store.Id);
                }

            foreach (var newStoreId in longList)
                // Add the roles which are not in the list of user's roles
                if (!user.Stores.Any(r => r.Id == newStoreId))
                {
                    addList.Add(newStoreId);
                    var newStore = new Store { Id = newStoreId };
                    _repository.GetEntity<Store>().Attach(newStore);
                    user.Stores.Add(newStore);
                }
                else
                {
                    updateList.Add(newStoreId);
                }

            entity.Stores.Clear();
            entity.Id = id;

            await synchroService.AddSynchroToStores(addList, LiteEmployee.Convert(entity), SynchroType.CREATE);//,converter: Converter);
            await synchroService.AddSynchroToStores(updateList, LiteEmployee.Convert(entity), SynchroType.UPDATE);//,converter: Converter);
            await synchroService.AddSynchroToStores(removeList, LiteEmployee.Convert(entity), SynchroType.DELETE);//,converter: Converter);

            entity.Stores = user.Stores;

            return await base.Put(id, entity);
        });
        return data;

    }
    
    public virtual async Task<bool> ChangeState(long id, bool state)
        {
            if (id < 1)
            {
                _logger.LogError("Id parameter out of bounds");
                throw new BusinessLogicException("Id parameter out of bounds");
            }

            var entity = await _repository.GetEntity<BOUser>()
                .Include(x => x.Stores)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
            
                var data = await _repository.TransactionAsync(async context =>
                {
                    var success = await _repository.ChangeStateAsync<BOUser>(id, state);

                    await synchroService.AddSynchroToStores(entity.Stores.Select(
                        x => x.Id).ToList(), 
                        LiteEmployee.Convert(entity), 
                        SynchroType.UPDATE
                        );
                    return success;
                });
                return data;
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
        var data = await _repository.TransactionAsync(async context =>
        {
            var entitieDelete = await _repository.GetEntity<BOUser>()
                .Include(x => x.Stores)
                .Where(x => x.Id == id).FirstOrDefaultAsync();

            if (entitieDelete == null) return false;
           
            await synchroService.AddSynchroToStores(entitieDelete.Stores.Select(x => x.Id).ToList(), entitieDelete,
                SynchroType.DELETE, _converter);
 
           
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
            throw new BusinessLogicException("List of ids is null or empty");
        }
        var data = await _repository.TransactionAsync(async context =>
        {
            var entitiesDelete = await _repository.GetEntity<BOUser>()
                .Include(x => x.Stores)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();

            foreach (var t in entitiesDelete)
                await synchroService
                    .AddSynchroToAllStores(
                        LiteEmployee.Convert(t), SynchroType.DELETE);

            var success = await _repository.DeleteRangeAsync(entitiesDelete);
            return success;
        });
        return data;
        
    }
}