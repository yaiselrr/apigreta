using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Sqlserver;
using Greta.Sdk.FileStorage.Interfaces;
using Greta.Sdk.FileStorage.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Greta.BO.Api.Workers
{
    [ExcludeFromCodeCoverage]
    public class SynchrDeleterWorker: BaseWorker
    {
        private readonly ILogger<SynchrDeleterWorker> _logguer;
        private readonly IConfiguration configuration;
        private readonly IStorageProvider _storage;
        private readonly IOptions<StorageOption> _options;

        public SynchrDeleterWorker(ILogger<SynchrDeleterWorker> logguer, 
            IConfiguration configuration,
            IStorageProvider storage,
            IOptions<StorageOption> options
            )
        {
            this._logguer = logguer;
            this.configuration = configuration;
            _storage = storage;
            _options = options;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
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

                    using (var context = new SqlServerContext(b.Options))
                    {
                        var currentTime = DateTime.UtcNow.AddDays(-5);
                        currentTime = DateTime.SpecifyKind(currentTime, DateTimeKind.Utc);
                        var pbs = await context.Set<Synchro>()
                            .Include(x => x.SynchroDetails)
                            .Where(x => x.CreatedAt < currentTime && (x.Status == SynchroStatus.CLOSE || x.Status == SynchroStatus.COMPLETE))
                            .ToListAsync(cancellationToken: stoppingToken);

                        foreach (var pb in pbs)
                        {
                            //await _storage.DeleteFile(pb.FilePath);
                            context.Set<SynchroDetail>().RemoveRange(pb.SynchroDetails);
                            var d = await context.SaveChangesAsync(stoppingToken) > 0;
                        }
                    }
                }
                catch (Exception e)
                {
                    _logguer.LogError(e, "Problem removing old synchros");
                }
                finally
                {
                    await Task.Delay((int)TimeSpan.FromDays(1).TotalMilliseconds);
                }
            }
            await WaitForCancellationAsync(stoppingToken);

            _logguer.LogInformation("Closing SynchrDeleterWorker");

            _logguer.LogInformation("Dispose SynchrDeleterWorker");
        }
    }
}