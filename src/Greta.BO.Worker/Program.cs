using System.Reflection;
using DotNetEnv;
using Elastic.Apm.SerilogEnricher;
using Greta.BO.Api.Sqlserver;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Handlers.Command.Synchro;
using Greta.BO.BusinessLogic.Hubs;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.Worker;
using Greta.BO.Worker.Extensions;
using Greta.Sdk.EFCore.Extensions;
using Greta.Sdk.FileStorage.Extensions;
using Greta.Sdk.Hangfire.MediatR;
using Hangfire;
using Hangfire.PostgreSql;
using MediatR;
using MediatR.Registration;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
builder.Host.ConfigureAppSettings();
// remove default logging providers
builder.Logging.ClearProviders();

var agent = builder.Configuration["Company:CompanyCode"] ?? string.Empty;
var loggerConfig = new LoggerConfiguration()
    //.ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName()
    .Enrich.WithMachineName()
    .Enrich.WithElasticApmCorrelationInfo()
    .MinimumLevel.Information()
    .Enrich.WithProperty("bo-client", agent)
    .Enrich.WithProperty("Application", $"worker-{agent}");

// if (builder.Environment.IsProduction())
//      loggerConfig.MinimumLevel.Information();
// else
//     loggerConfig.MinimumLevel.Debug();
loggerConfig.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
loggerConfig.MinimumLevel.Override("Quartz", LogEventLevel.Information);

loggerConfig.WriteTo.Console();

// var elasticUri = builder.Configuration["Serilog:Elasticsearch:nodeUris"];
// if (elasticUri != null)
// {
//     loggerConfig.WriteTo.Elasticsearch(
//         new ElasticsearchSinkOptions(
//             new Uri(elasticUri))
//         {
//             LevelSwitch = new LoggingLevelSwitch(LogEventLevel.Verbose),
//             CustomFormatter = new EcsTextFormatter(),
//             TemplateName = "Greta-Worker",
//             AutoRegisterTemplate = true,
//             AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
//             IndexFormat =
//                 $"{Assembly.GetExecutingAssembly().GetName().Name!.ToLower().Replace(".", "-")}-{builder.Configuration["Company:CompanyCode"]}-{builder.Environment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
//             ModifyConnectionSettings = x =>
//                 x.BasicAuthentication(builder.Configuration["Serilog:Elasticsearch:username"]!,
//                     builder.Configuration["Serilog:Elasticsearch:password"]!),
//         });
// }


var logger = loggerConfig.CreateLogger();

// Register Serilog
builder.Logging.AddSerilog(logger);

//register

builder.Services.AddSingleton<IBoHubClient>(opt =>
{
    var host = builder.Configuration["Company:BackEndUrl"];
    var hClient = new BoHubClient($"https://{host}/fronthub", opt.GetRequiredService<ILogger<BoHubClient>>());
    if (host != "undefined") hClient.Init();
    return hClient;
});


//Adding support for sqlserver
builder.Services.AddDbContext<SqlServerContext>(opt =>
{
    if (!opt.IsConfigured)
        opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), sqlopt =>
            // opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), sqlopt =>
        {
            sqlopt.MigrationsAssembly(typeof(PingPongWorker).GetTypeInfo().Assembly.GetName().Name);
            sqlopt.EnableRetryOnFailure(10, TimeSpan.FromSeconds(10),
                null);
        });
}, ServiceLifetime.Transient);

builder.Services.AddAutoMapper(typeof(AutoMapping).GetTypeInfo().Assembly);

builder.Services.AddHostedService<PingPongWorker>();

builder.Services.AddEFCore(builder.Configuration)
    .AddIdentityService<string>()
    .AddStorageManager(builder.Configuration)
    .AddRepositories<long, string, SqlServerContext>();
// builder.Services.AddBLServices<IBaseService>();

// load only the necessary services
builder.Services.AddTransient<ISynchroService, SynchroService>();
builder.Services.AddTransient<IDeviceService, DeviceService>();
builder.Services.AddTransient<IStoreService, StoreService>();

builder.Services.AddTransient<IShelfTagService, ShelfTagService>();
builder.Services.AddTransient<IRoundingTableService, RoundingTableService>();
builder.Services.AddTransient<IStoreProductService, StoreProductService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();

builder.Services.AddTransient<IManualConsoleService, ManualConsoleService>();

// builder.Services.ConfigureMediatR<IBaseService>();

var serviceConfig = new MediatRServiceConfiguration();
ServiceRegistrar.AddRequiredServices(builder.Services, serviceConfig);


builder.Services.AddScoped<IRequestHandler<SynchroCloseCommand>, SynchroCloseHandler>();
builder.Services.AddScoped<IRequestHandler<SynchroFullBackupCommand>, SynchroFullBackupHandler>();

// builder.Services.AddHangfire(conf =>
// {
//     conf.UsePostgreSqlStorage(
//         builder.Configuration["ConnectionStrings:DefaultConnection"]
//     );
//     conf.UseMediatR();
// });

// GlobalConfiguration.Configuration
//     .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
//     .UseColouredConsoleLogProvider()
//     .UseSimpleAssemblyNameTypeSerializer()
//     .UseRecommendedSerializerSettings()
//     .UsePostgreSqlStorage(builder.Configuration["ConnectionStrings:DefaultConnection"])
//     .UseMediatR();

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(
        builder.Configuration["ConnectionStrings:DefaultConnection"]
    )
    .UseMediatR());

builder.Services.AddHangfireServer();

var app = builder.Build();

// var service = app.Services.GetRequiredService<IManualConsoleService>();
// service.setupVendorONMorelos("/Users/chenry/Downloads/Query1.txt");

// service.ImportStandardProducts("/Users/chenry/Downloads/GROCERS.csv");
// service.OutsideGiftCardImport("/Users/chenry/Downloads/prescottgiftcarg.csv");
//service.LoadQtyProducts("/Users/chenry/Downloads/LiquorImport.csv").GetAwaiter().GetResult();
//service.ResyncScaleProduct();
// service.ResampleProductsFromOneStoreToOther(3, 4);
//service.OutsideCustomerImport("/Users/chenry/Proyectos/GRETA/Clients/zbar/CustomersZBAR.csv.txt");
//service.OutsideGiftCardImport("/Users/chenry/Downloads/oldbutchernewRegisters.csv");
//service.OutsideVendorProductsImport("C:\\Project\\docs\\vendororder\\ProductVendor.csv");
//service.Send(new SynchroClose.Command(1)).GetAwaiter().GetResult();
//service.UpdateSalesProductYavapee().GetAwaiter().GetResult();
//service.ImportSProducts(@"C:\Users\admin\Downloads\Telegram Desktop\Deli Scale.csv").GetAwaiter().GetResult();
//service.ImportWProducts(@"C:\Users\admin\Documents\Produce (2).csv").GetAwaiter().GetResult();
//service.ResyncScaleCategory().GetAwaiter().GetResult();


app.Logger.LogInformation("Initialization success");
app.MapGet("/", () => "Hello World!");

app.Run();