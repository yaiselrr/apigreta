using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using DotNetEnv;
using Elastic.CommonSchema.Serilog;
using Greta.BO.Api.Core.Startup.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Serilog.Sinks.Elasticsearch;

namespace Greta.BO.Api
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        protected Program()
        {
        }

        public static void Main(string[] args)
        {
            Env.Load();
            CreateHostBuilder(args).Build().MigrateDatabase().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args) 
                .UseSerilog((host, log) =>
                {
                    log//.ReadFrom.Configuration(host.Configuration)
                        .Enrich.FromLogContext()
                        .Enrich.WithEnvironmentName()
                        .Enrich.WithMachineName()
                        .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
                            .WithDefaultDestructurers()
                            .WithDestructurers(new[] { new DbUpdateExceptionDestructurer() })
                        )
                        //.Enrich.WithElasticApmCorrelationInfo()
                        .Enrich.WithProperty("bo-client", host.Configuration["Company:CompanyCode"] ?? string.Empty)
                        .Enrich.WithProperty("Application", $"bo-{host.Configuration["Company:CompanyCode"] ?? string.Empty}");
                    

                        log.Filter.ByExcluding("RequestPath like '/health%'");
                        log.Filter.ByExcluding("RequestPath like '/cloudhub%'");
                        log.Filter.ByExcluding("RequestPath like '/fronthub%'");
                            
                     if (host.HostingEnvironment.IsProduction())
                         log.MinimumLevel.Information();
                     else
                         log.MinimumLevel.Debug();
                     
                    log.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
                    log.MinimumLevel.Override("Quartz", LogEventLevel.Information);
                    log.MinimumLevel.Override("Hangfire", LogEventLevel.Information);
                    
                    // log.WriteTo.Console(new JsonFormatter(renderMessage: true));
                    log.WriteTo.Console();

                    var elasticUri = host.Configuration["Serilog:Elasticsearch:nodeUris"];
                    if (elasticUri != null)
                    {
                        log.WriteTo.Elasticsearch(
                                new ElasticsearchSinkOptions(
                                    new Uri(elasticUri))
                                {
                                    LevelSwitch = new LoggingLevelSwitch(LogEventLevel.Information),
                                    CustomFormatter = new EcsTextFormatter(),
                                    TemplateName = "Greta-BO",
                                    AutoRegisterTemplate = true,
                                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                                    IndexFormat =
                                        $"{Assembly.GetExecutingAssembly().GetName().Name!.ToLower().Replace(".", "-")}-{host.Configuration["Company:CompanyCode"]}-{host.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                                    ModifyConnectionSettings = x =>
                                        x.BasicAuthentication(host.Configuration["Serilog:Elasticsearch:username"]!,
                                            host.Configuration["Serilog:Elasticsearch:password"]!),
                                });
                    }
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}