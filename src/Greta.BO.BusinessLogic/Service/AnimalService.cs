using System;
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
/// Service layer for animal entity
/// </summary>
public interface IAnimalService : IGenericBaseService<Animal>
{
    /// <summary>
    ///     get a list of animals by rancher id
    /// </summary>
    /// <param name="rancherId">Rancher Id</param>
    /// <returns></returns>
    Task<List<Animal>> GetAnimalByRancher(long rancherId);

    /// <summary>
    ///     get a list of animals by breed id
    /// </summary>
    /// <param name="breedId">Breed Id</param>
    /// <returns></returns>
    Task<List<Animal>> GetAnimalByBreed(long breedId);

    /// <summary>
    ///    Get Breeds allowed
    /// </summary>
    /// <param name="date">Date</param>
    /// <returns></returns>
    Task<List<Breed>> GetSelectScheduleForDay(DateTime date);

    /// <summary>
    ///     Validate if this day can be added more data
    /// </summary>
    /// <param name="date">Date</param>
    /// <returns></returns>
    Task<bool> ValidateForDay(DateTime date);
}

/// <inheritdoc cref="Greta.BO.BusinessLogic.Service.IAnimalService" />
public class AnimalService : BaseService<IAnimalRepository, Animal>, IAnimalService
{
    /// <inheritdoc />
    public AnimalService(IAnimalRepository AnimalRepository,
        ILogger<AnimalService> logger)
        : base(AnimalRepository, logger)
    {
    }

    /// <summary>
    ///     Get entity by Id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Animal</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    public override async Task<Animal> Get(long id)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        var entity = await _repository.GetEntity<Animal>()
            .Include(x => x.Customers)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        return entity;
    }

    /// <inheritdoc />
    public virtual async Task<List<Animal>> GetAnimalByRancher(long rancherId)
    {
        var entities = await _repository.GetEntity<Animal>()
            .Where(x => x.Rancher.Id == rancherId).ToListAsync();
        return entities;
    }

    /// <inheritdoc />
    public virtual async Task<List<Animal>> GetAnimalByBreed(long breedId)
    {
        var entities = await _repository.GetEntity<Animal>()
            .Where(x => x.Breed.Id == breedId).ToListAsync();
        return entities;
    }

    /// <summary>
    ///     Insert a entity
    /// </summary>
    /// <param name="entity">Entity to insert</param>
    /// <returns>Entity</returns>
    public override async Task<Animal> Post(Animal entity)
    {
        if (!await ValidateForDay(entity.DateReceived.Value))
        {
            _logger.LogError("Error reached the maximum per day");
            throw new BusinessLogicException("Error reached the maximum per day.");
        }

        if (entity.RancherId == -1)
        {
            entity.RancherId = null;
        }

        if (entity.BreedId == -1)
        {
            entity.BreedId = null;
        }

        entity.DateReceived ??= DateTime.UtcNow;

        entity.DateSlaughtered ??= (DateTime?)null;

        if (entity.Customers != null)
            foreach (var t in entity.Customers)
                _repository.GetEntity<Customer>().Attach(t);

        var id = await _repository.CreateAsync(entity);
        entity.Id = id;

        var data = await _repository.GetEntity<Animal>()
            .Include(x => x.Customers)
            .FirstOrDefaultAsync(x => x.Id == id);

        return data;
    }

    /// <summary>
    ///     Update entity
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="entity">Entity to update</param>
    /// <returns>Boolean success</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    public override async Task<bool> Put(long id, Animal entity)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        // remove all stores first
        var animal = await _repository.GetEntity<Animal>()
            .Include(x => x.Customers)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        animal.RancherId = (entity.RancherId == -1) ? null : entity.RancherId;
        entity.RancherId = (entity.RancherId == -1) ? null : entity.RancherId;

        animal.BreedId = (entity.BreedId == -1) ? null : entity.BreedId;
        entity.BreedId = (entity.BreedId == -1) ? null : entity.BreedId;

        animal.DateReceived ??= DateTime.UtcNow;

        animal.DateSlaughtered ??= (DateTime?)null;

        await _repository.UpdateAsync(id, animal);

        var longList = entity.Customers.Select(x => x.Id).ToList();
        var removeList = new List<long>();
        var addList = new List<long>();
        var updateList = new List<long>();
        foreach (var item in animal.Customers.ToList())
            // Remove the roles which are not in the list of new roles
            if (!longList.Contains(item.Id))
            {
                animal.Customers.Remove(item);
                removeList.Add(item.Id);
            }

        foreach (var newItemId in longList)
            // Add the roles which are not in the list of user's roles
            if (!animal.Customers.Any(r => r.Id == newItemId))
            {
                addList.Add(newItemId);
                var newItem = new Customer { Id = newItemId };
                _repository.GetEntity<Customer>().Attach(newItem);
                animal.Customers.Add(newItem);
            }
            else
            {
                updateList.Add(newItemId);
            }

        entity.Customers = animal.Customers;

        return await base.Put(id, entity);
    }

    /// <inheritdoc />
    public async Task<List<Breed>> GetSelectScheduleForDay(DateTime date)
    {
        int day = (int)date.DayOfWeek;
        var resultBreed = new List<Breed>();
        var scalendars = await _repository.GetEntity<Scalendar>()
            .Include(x => x.Breeds)
            .Where(x => x.DayId == day)
            .FirstOrDefaultAsync();

        var schedules = await _repository.GetEntity<Animal>()
            .Where(x => x.DateReceived.HasValue && x.DateReceived.Value.Date == date.ToUniversalTime().Date)
            .ToListAsync();
        int count = 0;
        foreach (var item in scalendars.Breeds)
        {
            foreach (var ent in schedules)
            {
                if (ent.BreedId == item.Id)
                {
                    count++;
                }
            }

            if (count < item.Maxx)
            {
                resultBreed.Add(item);
            }

            count = 0;
        }

        return resultBreed;
    }

    /// <inheritdoc />
    public async Task<bool> ValidateForDay(DateTime date)
    {
        int day = (int)date.DayOfWeek;
        var entitiesTotal = await _repository.GetEntity<Animal>()
            .Where(x => x.DateReceived.HasValue && x.DateReceived.Value.Date == date.ToUniversalTime().Date)
            .CountAsync();

        var maxPosible = await _repository.GetEntity<Scalendar>()
            .Where(x => x.DayId == day)
            .Select(x => x.Breeds.Sum(b => b.Maxx))
            .FirstOrDefaultAsync();
        return entitiesTotal < maxPosible;
    }
}