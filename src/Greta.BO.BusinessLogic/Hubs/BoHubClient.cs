using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Models.Hubs;
using Greta.Sdk.HubClient;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Hubs
{
    public interface IBoHubClient : IHubClient
    {
        Task<bool> PingPong();
        Task<bool> OnNotifyWorkerStatus(int status, string message, object data);

        Task<bool> OnNotifyWorkerFullBackupStatus(int status, string connectionId, string message, object data);
    }

    public class BoHubClient : HubClient, IBoHubClient
    {
        public BoHubClient(string host, ILogger<BoHubClient> logger) : base(host, logger)
        {
        }

        public async Task<bool> OnNotifyWorkerStatus(int status, string message, object data)
        {
            var tosend = new NotifyWorkerStatus
            {
                StatusCode = status,
                Errors = new List<string> {message},
                Data = data
            };
            if (connection == null || connection.State != HubConnectionState.Connected)
            {
                _logger.LogError("Bo Hub not connected");
                throw new Exception("Bo Hub not connected");
            }

            return await connection.InvokeAsync<bool>(
                "OnNotifyWorkerStatus",
                tosend
            );
        }

        public async Task<bool> OnNotifyWorkerFullBackupStatus(int status, string connectionId, string message,
            object data)
        {
            var tosend = new NotifyWorkerFullBackupStatus
            {
                StatusCode = status,
                Errors = new List<string> {message},
                Data = data,
                ConnectionId = connectionId
            };
            if (connection == null || connection.State != HubConnectionState.Connected)
            {
                _logger.LogError("Bo Hub not connected");
                throw new Exception("Bo Hub not connected");
            }

            return await connection.InvokeAsync<bool>(
                "OnNotifyWorkerFullBackupStatus",
                tosend
            );
        }

        public async Task<bool> PingPong()
        {
            return await connection.InvokeAsync<bool>(
                "PingPong"
            );
        }

        protected new async Task SetupConnection()
        {
            await base.SetupConnection();
        }
    }
}