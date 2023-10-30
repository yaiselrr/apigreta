using System.Diagnostics.CodeAnalysis;

namespace Greta.BO.Worker.Extensions;

[ExcludeFromCodeCoverage]
public static class WebApplicationBuilder
{
    public static IHostBuilder ConfigureAppSettings(this IHostBuilder host)
    {
        var enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        host.ConfigureAppConfiguration((ctx, builder) =>
        {
            builder.AddJsonFile("appsettings.json", false, false);
            builder.AddJsonFile($"appsettings.{enviroment}.json", true, true);
            builder.AddEnvironmentVariables();
        });

        return host;
    }
}