﻿using System.Diagnostics.CodeAnalysis;
using DotNetEnv;
using Greta.BO.Api.Sqlserver;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Greta.BO.Api.Core.Startup.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            using var appContext = scope.ServiceProvider.GetRequiredService<SqlServerContext>();
            Env.Load();
            appContext.Database.Migrate();

            return host;
        }
    }
}