using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Sqlserver;
using Greta.BO.BusinessLogic.Handlers.Command.Synchro;
using Greta.BO.BusinessLogic.Handlers.Queries.Synchro;
using Greta.BO.BusinessLogic.Hubs;
using Greta.Sdk.Hangfire.MediatR;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Greta.BO.Api.Workers
{
    [ExcludeFromCodeCoverage]
    public class SyncMonitorWorker : BaseWorker
    {
        private const int Timeoutmin = 10;
        private readonly ILogger<SyncMonitorWorker> _logguer;
        private readonly IHubContext<CloudHub, ICloudHub> _cloudHubContext;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public SyncMonitorWorker(ILogger<SyncMonitorWorker> logguer,
            IHubContext<CloudHub, ICloudHub> cloudHubContext,
            IConfiguration configuration,
            IMediator mediator
        )
        {
            this._logguer = logguer;
            _cloudHubContext = cloudHubContext;
            this._configuration = configuration;
            _mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var b = new DbContextOptionsBuilder()
                        .UseNpgsql(connectionString, sqlopt =>
                        {
                            sqlopt.UseAdminDatabase("defaultdb");
                            sqlopt.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                            sqlopt.EnableRetryOnFailure(10, TimeSpan.FromSeconds(10),
                                null);
                        });
                    await using (var context = new SqlServerContext(b.Options))
                    {
                        var stores = await context.Set<Store>()
                            .Include(x => x.Devices)
                            .ToListAsync(cancellationToken: stoppingToken);
                        foreach (var store in stores)
                        {
                            var synchros = await  context.Set<Synchro>()
                                .Where(x => x.StoreId == store.Id && x.Status == SynchroStatus.OPEN)
                                .ToListAsync(cancellationToken: stoppingToken);

                            foreach (var synchro in synchros)
                            {
                                _logguer.LogInformation("Closing synchro for store {StoreName}", store.Name);
                                _mediator.EnqueueNew("CloseSynchroFromWorker", new SynchroCloseCommand(synchro.Id));
                                // Send notification to users with this requirements
                                // determine how we process the notification process.
                            }

                            foreach (var device in store.Devices)
                            {
                                if (device.SignalRConnectionId != null)
                                {
                                    var paths = await _mediator.Send(
                                        new GetPathsLeftForDeviceQuery(device.DeviceId, device.SynchroVersion,
                                            device.StoreId), stoppingToken);
                                    _logguer.LogInformation(
                                        "Found {Count} paths for device {DeviceName}. And sending to the device",
                                        paths.Count, device.Name);
                                    if (paths.Count > 0)
                                    {
                                        await _cloudHubContext.Clients.Client(device.SignalRConnectionId)
                                            .OnNeedUpdate(null,
                                                paths.Select(x => new SynchroPathData()
                                                    { Path = x.FilePath, Tag = x.Tag }).ToList());
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    _logguer.LogError(e, "Problem monitoring synchros");
                }
                finally
                {
                    await Task.Delay((int)TimeSpan.FromMinutes(Timeoutmin).TotalMilliseconds, stoppingToken);
                }
            }

            await WaitForCancellationAsync(stoppingToken);

            _logguer.LogInformation("Closing SyncMonitorWorker");

            _logguer.LogInformation("Dispose SyncMonitorWorker");
        }
    }
}