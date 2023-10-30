using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Dto;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Sqlserver;
using Greta.BO.BusinessLogic.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Greta.BO.Api.Workers
{
    [ExcludeFromCodeCoverage]
    public class ExternalJobWorker : BaseWorker
    {
        private const int MAXFAILRETRY = 5;
        private readonly ILogger<ExternalJobWorker> _logguer;
        private readonly IConfiguration _configuration;
        private readonly IHubContext<CloudHub, ICloudHub> _hub;
        private readonly IHostApplicationLifetime _applicationLifetime;

        public ExternalJobWorker(ILogger<ExternalJobWorker> logguer,
            IConfiguration configuration,
            IHubContext<CloudHub, ICloudHub> hub, IHostApplicationLifetime applicationLifetime)
        {
            this._logguer = logguer;
            this._configuration = configuration;
            _hub = hub;
            _applicationLifetime = applicationLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                while (!stoppingToken.IsCancellationRequested)
                {
                    var b = new DbContextOptionsBuilder()
                        .UseNpgsql(connectionString, sqlopt =>
                        {
                            sqlopt.UseAdminDatabase("defaultdb");
                            sqlopt.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                            sqlopt.EnableRetryOnFailure(10, TimeSpan.FromSeconds(10),
                                null);
                        });

                    using (var context = new SqlServerContext(b.Options))
                    {
                        //process all init or failed jobs

                        var jobs = await context.Set<ExternalJob>()
                            .Where(x => (x.Status == ExternalJobStatus.Init ||
                                         (x.Status == ExternalJobStatus.Fail && x.FailRetry < MAXFAILRETRY)))
                            .ToListAsync(cancellationToken: stoppingToken);

                        foreach (var job in jobs)
                        {
                            switch (job.Type)
                            {
                                case ExternalJobType.ScaleUpdate:
                                    _logguer.LogInformation("Begin send scale sync job");
                                    await ProcessScaleJobs(context, job);
                                    _logguer.LogInformation("End send scale sync job");
                                    break;
                            }
                        }

                        await context.SaveChangesAsync(stoppingToken);
                    }

                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }

                await WaitForCancellationAsync(stoppingToken);
            }
            catch (Exception)
            {
                _applicationLifetime.StopApplication();
            }
            finally
            {
                if (!stoppingToken.IsCancellationRequested)
                    _logguer.LogError("Worker stopped unexpectedly");
            }

            _logguer.LogInformation("Closing ExternalJobWorker");

            _logguer.LogInformation("Dispose ExternalJobWorker");
        }

        private async Task ProcessScaleJobs(SqlServerContext context, ExternalJob job)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = false
            };
            var data = JsonSerializer.Deserialize<SendScaleDataDto>(job.Data, options);

            if (data != null)
            {
                data.TaskJob = job.Id;
            }

            //determine one device connected
            //send data to that device, track the device number and change the job status to send
            var devices = await context.Set<Device>().Where(x => /*x.State && */x.SignalRConnectionId != null)
                .ToListAsync();

            if (devices.Count == 0)
            {
                _logguer.LogError($"Not found connected devices.");
                return;
            }

            bool send = false;
            foreach (var device in devices)
            {
                try
                {
                    await _hub.Clients.Client(device.SignalRConnectionId).SendExternalScaleExternalJobToDevice(data);
                    _logguer.LogInformation("Job sended to device {Name}", device.Name);
                    send = true;
                    //break;
                }
                catch (Exception e)
                {
                    _logguer.LogError(e, "Error sending information to device {Name} with id {Id}", device.Name, device.Id);
                }
            }

            if (send)
            {
                job.Status = ExternalJobStatus.Sending;
                context.Set<ExternalJob>().Update(job);
            }
        }
    }
}