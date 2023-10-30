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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;

/// <summary>
/// Service layer for external scale entity
/// </summary>
public interface IExternalScaleService : IGenericBaseService<ExternalScale>
{
    /// <summary>
    ///     Get Entity by id
    /// </summary>
    /// <param name="storeId">Store Id</param>
    /// <returns></returns>
    Task<List<ExternalScale>> GetExternalScaleByStore(long storeId);

    /// <summary>
    ///     Get Entity list by storeId and departmentId
    /// </summary>
    /// <param name="storeId">Store Id</param>
    /// <param name="departmentId">Department Id</param>
    /// <returns></returns>
    Task<List<ExternalScale>> GetExternalScaleByStoreAndDepartment(long storeId, long departmentId);
}

/// <inheritdoc cref="Greta.BO.BusinessLogic.Service.IFamilyService" />
public class ExternalScaleService : BaseService<IExternalScaleRepository, ExternalScale>, IExternalScaleService
{
    /// <inheritdoc />
    public ExternalScaleService(IExternalScaleRepository externalScaleRepository,
        ILogger<ExternalScaleService> logger, ISynchroService synchroService)
        : base(externalScaleRepository, logger, synchroService, Converter)
    {
    }

    private static object Converter(ExternalScale from) => (LiteExternalScale.Convert(from));

    /// <summary>
    ///     Get entity by Id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>ExternalScale</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    public override async Task<ExternalScale> Get(long id)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        var entity = await _repository.GetEntity<ExternalScale>()
            .Include(x => x.Departments)
            .Include(x => x.SyncDevice)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        return entity;
    }

    /// <inheritdoc />
    public virtual async Task<List<ExternalScale>> GetExternalScaleByStore(long storeId)
    {
        var entities = await _repository.GetEntity<ExternalScale>()
            .Where(x => x.Store.Id == storeId && x.ExternalScaleType != BoExternalScaleType.GretaLabel).ToListAsync();
        return entities;
    }

    /// <inheritdoc />
    public virtual async Task<List<ExternalScale>> GetExternalScaleByStoreAndDepartment(long storeId, long departmentId)
    {
        var entities = await _repository.GetEntity<ExternalScale>()
            .Where(x =>
                x.State &&
                x.StoreId == storeId &&
                x.ExternalScaleType != BoExternalScaleType.GretaLabel &&
                x.Departments.Any(d => d.Id == departmentId)
                ).ToListAsync();
        return entities;
    }

    /// <summary>
    ///     Insert a entity
    /// </summary>
    /// <param name="entity">Entity to insert</param>
    /// <returns>Entity</returns>
    public override async Task<ExternalScale> Post(ExternalScale entity)
    {
        if (entity.Departments != null)
            foreach (var t in entity.Departments)
                _repository.GetEntity<Department>().Attach(t);
        if (entity.SyncDeviceId == -1)
        {
            entity.SyncDeviceId = null;
        }

        return await _repository.TransactionAsync(async context =>
        {
            var id = await _repository.CreateAsync(entity);
            entity.Id = id;

            var data = await _repository.GetEntity<ExternalScale>()
                //.Include(x => x.Store)
                .Include(x => x.Departments)
                .Include(x => x.SyncDevice)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (data != null)
            {
                await synchroService.AddSynchroToStore(data.StoreId, data, SynchroType.CREATE, _converter);
            }

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
    public override async Task<bool> Put(long id, ExternalScale entity)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds.");
        }

        return await _repository.TransactionAsync(async context =>
        {
            // remove all stores first
            var extScale = await _repository.GetEntity<ExternalScale>()
                .Include(x => x.Departments)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            extScale.SyncDeviceId = (entity.SyncDeviceId == -1) ? null : entity.SyncDeviceId;
            entity.SyncDeviceId = (entity.SyncDeviceId == -1) ? null : entity.SyncDeviceId;
            await _repository.UpdateAsync(id, extScale);
            entity.Departments = ProcessMany2ManyUpdate(extScale.Departments, entity.Departments);
            var response = await _repository.UpdateAsync(id, entity);
            var data = await _repository.GetEntity<ExternalScale>()
                .Include(x => x.Departments)
                .Include(x => x.SyncDevice)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (data != null)
            {
                await synchroService.AddSynchroToStore(data.StoreId, data, SynchroType.UPDATE, _converter);
            }

            return response;
        });
    }
}