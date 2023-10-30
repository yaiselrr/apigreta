using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Sqlserver;
using Greta.Sdk.EFCore.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Greta.BO.Api.HealthChecks;

[ExcludeFromCodeCoverage]
public class DatabaseHealthCheck : IHealthCheck
{
    private readonly ILogger<DatabaseHealthCheck> _logger;
    private readonly IConfiguration _configuration;
    private readonly IAuthenticateUser<string> _authenticatetUser;

    public DatabaseHealthCheck(
        ILogger<DatabaseHealthCheck> logger,
        IConfiguration configuration,
        IAuthenticateUser<string> authenticatetUser)
    {
        _logger = logger;
        _configuration = configuration;
        _authenticatetUser = authenticatetUser;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
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

            await using var ctxDb = new SqlServerContext(b.Options, _authenticatetUser);
            var cmd = ctxDb.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "SELECT 1";
            if (cmd.Connection != null)
            {
                cmd.Connection.Open();
                await cmd.ExecuteScalarAsync(cancellationToken);
                return HealthCheckResult.Healthy("Database connection healthy");
            }
            else
            {
                _logger.LogError("Database connection not found");
                return HealthCheckResult.Unhealthy("Database connection not found");
            }

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Database connection fail");
            return HealthCheckResult.Unhealthy("Database connection fail", exception: e);
        }
    }
}