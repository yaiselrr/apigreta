using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Handlers.Command.Device;
using Greta.BO.BusinessLogic.Handlers.Queries.Device;
using Greta.BO.BusinessLogic.Handlers.Queries.Synchro;
using Greta.BO.BusinessLogic.Models.Hubs;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRSwaggerGen.Attributes;
using SignalRSwaggerGen.Enums;

namespace Greta.BO.BusinessLogic.Hubs
{
    [SignalRHub(autoDiscover: AutoDiscover.MethodsAndParams, path:"/fronthub")]
    public interface IFrontHub
    {
        /// <summary>
        /// NOtify the result data from device
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [SignalRMethod(autoDiscover: AutoDiscover.Params, description:"NOtify the result data from device")] 
        Task OnGetPrinter(object data);//long deviceid, string data);
        /// <summary>
        /// Notify one synchro are closed
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [SignalRMethod(autoDiscover: AutoDiscover.Params, description:"Notify one synchro are closed")] 
        Task OnCloseSynchronization(NotifyWorkerStatus data);
        /// <summary>
        /// Update import status
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [SignalRMethod(autoDiscover: AutoDiscover.Params, description:"Update import status")] 
        Task OnUpdateStatus(object data);

        Task OnUpdateScaleStatus(long job, int status, string message);
    }

    public class FrontHub : Hub<IFrontHub>
    {
        private readonly ILogger<FrontHub> _logger;
        private readonly IHubContext<CloudHub, ICloudHub> _cloudHubContext;
        private readonly IDeviceService _deviceService;
        private readonly IMediator _mediator;

        public FrontHub(
            ILogger<FrontHub> logger,
            IHubContext<CloudHub, ICloudHub> cloudHubContext,
            IDeviceService deviceService,
            IMediator mediator
        )
        {
            _logger = logger;
            _cloudHubContext = cloudHubContext;
            _deviceService = deviceService;
            _mediator = mediator;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            //adding the is to storage
            // var userId = Context.ConnectionId;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
            //remove the is to storage
            // var userId = Context.ConnectionId;
        }

        public async Task<bool> OnNotifyWorkerStatus(NotifyWorkerStatus data)
        {
            await Clients.All.OnCloseSynchronization(data);

            //System.Text.Json.JsonElement
            var synchro = JsonSerializer.Deserialize<Api.Entities.Synchro>(((System.Text.Json.JsonElement)data.Data).ToString());
            //var synchro = (Api.Entities.Synchro)data.Data;

            var paths = await _mediator.Send(new GetPathsLeftForStore.Query(synchro.StoreId));

            var connectedDevices = await _mediator.Send(new GetDevicesConnectedByStore.DeviceGetDevicesConnectedByStoreQuery(synchro.StoreId));

            await _cloudHubContext.Clients.Clients(connectedDevices.Select(x => x.SignalRConnectionId).ToList()).OnNeedUpdate(
                Context.ConnectionId, 
                paths.Select(x => new SynchroPathData() { Path = x.FilePath, Tag = x.Tag }).ToList());
            return true;
        }

        public async Task<bool> OnNotifyWorkerFullBackupStatus(NotifyWorkerFullBackupStatus data)
        {
            if (data.ConnectionId == null)
                await _cloudHubContext.Clients.All.OnCompleteFullBackup(data);
            else
                await _cloudHubContext.Clients.Client(data.ConnectionId).OnCompleteFullBackup(data);
            return true;
        }

        /// <summary>
        ///     Reuqest all the printers from one device
        /// </summary>
        /// <param name="type">romote, local, all</param>
        /// <param name="deviceid">device id</param>
        /// <returns></returns>
        public async Task<bool> GetPrinters(string type, long deviceid)
        {
            var connectionId = await _deviceService.GetConnectionIdById(deviceid);
            if (connectionId == null)
                return false;
            
            _logger.LogInformation($"Requested printers from Front client for device {deviceid}");

            await _cloudHubContext.Clients.Client(connectionId).GetPrinters(type, deviceid, Context.ConnectionId);

            return true;
        }
        
        /// <summary>
        ///     Reuqest to one device tahat need syncronization
        /// </summary>
        /// <param name="type">romote, local, all</param>
        /// <param name="deviceid">device id</param>
        /// <returns></returns>
        public async Task<bool> NeedSynchro(string deviceid)
        {
            try
            {
                if (deviceid == null) return false;
                var connectionId = await _deviceService.GetConnectionIdByDeviceId(deviceid);
                if (connectionId == null)
                    return false;
                var device = await _deviceService.GetByDeviceLic(deviceid);
                if (device == null)return false;
                    
                _logger.LogInformation("Requested synchro from device {DeviceId}", deviceid);

                //find all path needed and send to client

                var paths = await _mediator.Send(new GetPathsLeftForDeviceQuery(device.DeviceId, device.SynchroVersion, device.StoreId));

                await _cloudHubContext.Clients.Client(connectionId).OnNeedUpdate(Context.ConnectionId, paths.Select(x => new SynchroPathData() { Path = x.FilePath, Tag = x.Tag }).ToList());

                return true;
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Requested synchro fail from device {DeviceId}", deviceid);
                return false;
            }
           
        }
        
        
        /// <summary>
        /// Ping pong function for frontend
        /// </summary>
        /// <returns></returns>
        public Task<bool> PingPong()
        {
            return Task.FromResult(true);
        }
    }
}