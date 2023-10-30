using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.BO.BusinessLogic.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;

/// <summary>
/// Service layer for device entity
/// </summary>
public interface IDeviceService : IGenericBaseService<Device>
{
    /// <summary>
    /// Signal connected in a device
    /// </summary>
    /// <param name="deviceId">device Id</param>
    /// <param name="connectionId">connection Id</param>
    /// <returns></returns>
    Task<Device> SignalRConnected(string deviceId, string connectionId);

    /// <summary>
    /// Signal disconnected in a device
    /// </summary>
    /// <param name="connectionId">connection Id</param>
    /// <returns></returns>
    Task<string> SignalRDisconnected(string connectionId);

    /// <summary>
    /// Get connection device by id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns></returns>
    Task<string> GetConnectionIdById(long id);

    /// <summary>
    /// Get connection device by device id
    /// </summary>
    /// <param name="id">device Id</param>
    /// <returns></returns>
    Task<string> GetConnectionIdByDeviceId(string id);

    /// <summary>
    /// Get connection device by guid
    /// </summary>
    /// <param name="guid">guid</param>
    /// <returns></returns>
    Task<string> GetConnectionIdByGuid(string guid);
    /// <summary>
    ///     Update entity last pong time and signalr connection id
    /// </summary>
    /// <param name="deviceId">deviceId</param>
    /// <param name="connectionId">connectionId</param>
    /// <returns></returns>
    Task<bool> UpdatePong(string deviceId, string connectionId);

    /// <summary>
    ///     Update entity only allow Name
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="entity">Entity to update</param>
    /// <returns>Boolean success</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    Task<bool> PutName(long id, Device entity);

    /// <summary>
    ///     Get all the Entities by store id
    /// </summary>
    /// <param name="storeId">Store id</param>
    /// <returns>Return list of entities</returns>
    Task<List<Device>> GetDeviceConnectedByStore(long storeId);

    /// <summary>
    ///     Get synchronization status of devices
    /// </summary>
    /// <param name="requestId">requestId Id</param>
    /// <returns></returns>
    Task<DeviceSynchronizationStatus> GetSynchronizationStatus(long requestId);

    /// <summary>
    /// Update entity only allow Configuration Fields
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="entity">Entity to update</param>
    /// <returns>Boolean success</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    Task<bool> PutConfiguration(long id, Device entity);

    /// <summary>
    ///     Update tag version device
    /// </summary>
    /// <param name="deviceId">Device Id</param>
    /// <param name="newTag">new Tag</param>
    /// <returns>Return true or false</returns>
    Task<bool> UpdateTagVersion(string deviceId, long newTag);

    /// <summary>
    ///     Get id by guid
    /// </summary>
    /// <param name="guid">guid</param>
    /// <returns>Id</returns>
    Task<long> GetIdByGuid(Guid guid);

    /// <summary>
    ///     Get device by lic
    /// </summary>
    /// <param name="lic">lic</param>
    /// <returns>device</returns>
    Task<Device> GetByDeviceLic(string lic);

    /// <summary>
    ///     Get all the connected entities by store id
    /// </summary>
    /// <param name="storeId">Store id</param>
    /// <returns>Return list of entities</returns>
    Task<List<Device>> GetAllConnectedByStore(long storeId);

    /// <summary>
    ///     Get device by signalRConnectionId
    /// </summary>
    /// <param name="signalRConnectionId">signalR connection id</param>
    /// <returns>device</returns>
    Task<Device> GetDeviceByConnectionId(string signalRConnectionId);
}

/// <inheritdoc cref="Greta.BO.BusinessLogic.Service.IDeviceService" />
public class DeviceService : BaseService<IDeviceRepository, Device>, IDeviceService
{
    /// <inheritdoc />
    public DeviceService(IDeviceRepository repository,
        ILogger<DeviceService> logger)
        : base(repository, logger)
    {
    }

    private readonly List<string> _blacklistForConfiguration = new List<string>() {
            nameof(Device.Id),
            nameof(Device.CreatedAt),
            nameof(Device.UserCreatorId),
            nameof(Device.DeviceId),
            nameof(Device.Name),
            nameof(Device.SynchroVersion),
            nameof(Device.SignalRConnectionId),
            nameof(Device.Printers),
            nameof(Device.GuidId),
            nameof(Device.StoreId),
            nameof(Device.WorkerAlive),
            nameof(Device.BrokerAlive),
            nameof(Device.LastPongTime),
            nameof(Device.BrokerVersion),
            nameof(Device.WorkerVersion),
            nameof(Device.RealTimeRaw)
        };

    /// <summary>
    ///     Get all entities
    /// </summary>
    /// <returns></returns>
    public override async Task<List<Device>> Get()
    {
        // i remove the state verification because the device not set this element yet
        var entities = await _repository.GetEntity<Device>()/*.Where(x => x.State)*/.ToListAsync();
        return entities;
    }

    /// <inheritdoc />
    public async Task<bool> PutConfiguration(long id, Device entity)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds.");
        }

        var entityUpdated = await _repository.GetEntity<Device>().FindAsync(id);

        if (entityUpdated != null)
        {
            var properties = typeof(Device).GetProperties().Where(x => !_blacklistForConfiguration.Contains(x.Name)).ToList();

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
    public async Task<bool> UpdateTagVersion(string deviceId, long newTag)
    {
        var entityUpdated = await _repository.GetEntity<Device>()
            .Where(x => x.DeviceId == deviceId)
            .FirstOrDefaultAsync();
        if (entityUpdated == null) return false;

        _logger.LogInformation("Device {EntityUpdatedName} updating version from {EntityUpdatedSynchroVersion} to {NewTag}", entityUpdated.Name, entityUpdated.SynchroVersion, newTag);
        entityUpdated.SynchroVersion = (int)newTag;
        var updated = await base.Put(entityUpdated.Id, entityUpdated);

        if (updated)
        {
            var store = await _repository.GetEntity<Store>()
                .Include(x => x.Synchros)
                .Include(x => x.Devices)
                .Where(x => x.Id == entityUpdated.StoreId)
                .FirstOrDefaultAsync();

            //update store if this device is the last with this tag
            if (store != null && store.SynchroVersion < newTag)
            {
                //if store is not on current tag
                //check if all device in this store are updated
                var deviceUpdated = true;
                foreach (var dev in store.Devices)
                {
                    if (dev.SynchroVersion < newTag)
                    {
                        deviceUpdated = false;
                        break;
                    }
                }

                if (deviceUpdated)
                {
                    store.SynchroVersion = (int)newTag;
                    var completeSynchro = entityUpdated.Store.Synchros
                        .FirstOrDefault(x => x.Tag == newTag && x.Status == SynchroStatus.CLOSE);
                    if (completeSynchro != null)
                    {
                        completeSynchro.Status = SynchroStatus.COMPLETE;
                    }
                    await _repository.UpdateAsync(store);
                    //if not update then update the synchro by hand
                }

            }
        }

        return updated;
    }

    /// <inheritdoc />
    public async Task<bool> PutName(long id, Device entity)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        var fromDb = await _repository.GetEntity<Device>().Where(x => x.Id == id).FirstOrDefaultAsync();
        if (fromDb == null)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        fromDb.Name = entity.Name;
        return await base.Put(id, fromDb);
    }

    /// <inheritdoc />
    public async Task<DeviceSynchronizationStatus> GetSynchronizationStatus(long requestId)
    {
        var entity = await _repository.GetEntity<Device>()
            .Include(x => x.Store)
            .Where(x => x.Id == requestId)
            .FirstOrDefaultAsync();

        //get all close synchros and see if one have more big tag
        var synchros = await _repository.GetEntity<Synchro>()
            .Where(x => x.StoreId == entity.StoreId && x.Status == SynchroStatus.CLOSE)
            .ToListAsync();

        if (synchros.Any(x => entity != null && x.Tag > entity.SynchroVersion))
            return DeviceSynchronizationStatus.Outdated;

        // var s = await _repository.GetEntity<Synchro>()
        //        .Where(x => x.StoreId == entity.StoreId && x.Status == SynchroStatus.OPEN)
        //        .FirstOrDefaultAsync() != null;
        if (synchros.Any(x => entity != null && x.Tag > entity.SynchroVersion))
            return DeviceSynchronizationStatus.UpdateForNow;


        return DeviceSynchronizationStatus.Update;
    }

    /// <inheritdoc />
    public async Task<string> GetConnectionIdById(long id)
    {
        return await _repository.GetEntity<Device>()
            .Where(x => x.Id == id)
            .Select(x => x.SignalRConnectionId)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<string> GetConnectionIdByDeviceId(string id)
    {
        return await _repository.GetEntity<Device>()
            .Where(x => x.DeviceId == id)
            .Select(x => x.SignalRConnectionId)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<string> GetConnectionIdByGuid(string guid)
    {
        return await _repository.GetEntity<Device>()
            .Where(x => x.GuidId == Guid.Parse(guid))
            .Select(x => x.SignalRConnectionId)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<Device> GetDeviceByConnectionId(string signalRConnectionId)
    {
        return await _repository.GetEntity<Device>()
            .Where(x => x.SignalRConnectionId == signalRConnectionId)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<List<Device>> GetAllConnectedByStore(long storeId)
    {
        return (await _repository.GetEntity<Device>()
            .Where(x => x.SignalRConnectionId != null && x.StoreId == storeId)
            .ToListAsync());
    }

    /// <inheritdoc />
    public async Task<long> GetIdByGuid(Guid guid)
    {
        return (await _repository.GetEntity<Device>()
            .Where(x => x.GuidId == guid)
            .Select(x => x.Id)
            .FirstOrDefaultAsync());
    }

    /// <inheritdoc />
    public async Task<Device> GetByDeviceLic(string lic)
    {
        return (await _repository.GetEntity<Device>()
            .Include(x => x.Store)
            .Where(x => x.DeviceId == lic)
            .FirstOrDefaultAsync());
    }

    /// <inheritdoc />
    public async Task<Device> SignalRConnected(string deviceId, string connectionId)
    {
        try
        {
            var device = await _repository.GetEntity<Device>()
                .Where(x => x.DeviceId == deviceId)
                .FirstOrDefaultAsync();
            if (device == null) return null;

            device.SignalRConnectionId = connectionId;

            var updated = await Put(device.Id, device);

            return updated ? device : null;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "OnSignalRConnected");
            return null;
        }
    }

    /// <inheritdoc />
    public async Task<string> SignalRDisconnected(string connectionId)
    {
        var device = await _repository.GetEntity<Device>()
            .Where(x => x.SignalRConnectionId == connectionId)
            .FirstOrDefaultAsync();
        if (device == null) return "Device not found";

        device.SignalRConnectionId = null;
        device.BrokerAlive = false;
        device.WorkerAlive = false;

        var updated = await Put(device.Id, device);

        return updated ? device.Name : null;
    }

    /// <inheritdoc />
    public async Task<bool> UpdatePong(string deviceId, string connectionId)
    {
        var device = await _repository.GetEntity<Device>()
            .Where(x => x.DeviceId == deviceId)
            .FirstOrDefaultAsync();

        if (device == null) return false;
        device.SignalRConnectionId = connectionId;
        device.LastPongTime = DateTime.UtcNow;
        return await Put(device.Id, device);
    }

    /// <inheritdoc />
    public async Task<List<Device>> GetDeviceConnectedByStore(long storeId)
    {
        var a = await _repository.GetEntity<Device>().Where(x => x.StoreId == storeId && x.SignalRConnectionId != null).ToListAsync();
        return a;
    }
}