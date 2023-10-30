using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;

/// <summary>
/// Service layer for scalendar entity
/// </summary>
public interface IScalendarService : IGenericBaseService<Scalendar>
{

}

/// <inheritdoc cref="Greta.BO.BusinessLogic.Service.IScalendarService" />
public class ScalendarService : BaseService<IScalendarRepository, Scalendar>, IScalendarService
{
    /// <inheritdoc />
    public ScalendarService(IScalendarRepository repository,
        ILogger<ScalendarService> logger)
        : base(repository, logger)
    {
    }


    /// <summary>
    ///     Get entity by Id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Customer</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    public override async Task<Scalendar> Get(long id)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        var entity = await _repository.GetEntity<Scalendar>()
            .Include(x => x.Breeds)
            .Where(x => x.Id == id).FirstOrDefaultAsync();
        return entity;
    }


    /// <summary>
    ///     Insert a entity
    /// </summary>
    /// <param name="entity">Entity to insert</param>
    /// <returns>Entity</returns>
    public override async Task<Scalendar> Post(Scalendar entity)
    {
        // return await _repository.TransactionAsync(async context =>
        // {
        // var elem = new List<Store>();
        if (entity.Breeds != null)
            for (var i = 0; i < entity.Breeds.Count; i++)
                _repository.GetEntity<Breed>().Attach(entity.Breeds[i]);
        return await base.Post(entity);
        // });
    }

    /// <summary>
    ///     Update entity
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="entity">Entity to update</param>
    /// <returns>Boolean success</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    public override async Task<bool> Put(long id, Scalendar entity)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        var tax = await _repository.GetEntity<Scalendar>()
            .Include(x => x.Breeds)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        var longList = entity.Breeds.Select(x => x.Id).ToList();
        var addList = new List<long>();
        foreach (var store in tax.Breeds.ToList())
            // Remove the roles which are not in the list of new roles
            if (!longList.Contains(store.Id))
            {
                tax.Breeds.Remove(store);
            }

        foreach (var newStoreId in longList)
            // Add the roles which are not in the list of user's roles
            if (!tax.Breeds.Any(r => r.Id == newStoreId))
            {
                addList.Add(newStoreId);
                var newRole = new Breed { Id = newStoreId };
                _repository.GetEntity<Breed>().Attach(newRole);
                tax.Breeds.Add(newRole);
            }

        entity.Breeds = tax.Breeds;
        entity.Id = tax.Id;
        entity.DayId = tax.DayId;
        return await base.Put(id, entity);
    }
}