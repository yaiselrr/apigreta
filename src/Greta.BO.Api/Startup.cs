using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Greta.BO.Api.Core;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Filters;
using Greta.BO.Api.HealthChecks;
using Greta.BO.Api.MassTransit;
using Greta.BO.Api.PipelineBehavior;
using Greta.BO.Api.Sqlserver;
using Greta.BO.Api.Workers;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Extensions;
using Greta.BO.BusinessLogic.Hubs;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Options;
using Greta.BO.Wix.Extentions;
using Greta.Sdk.EFCore.Extensions;
using Greta.Sdk.FileStorage.Extensions;
using Greta.Sdk.Hangfire.Elastic.PipelineBehavior;
using Greta.Sdk.Hangfire.MediatR;
using Greta.Sdk.MassTransit.Extensions;
using Greta.Sdk.MassTransit.Interfaces;
using Greta.Sender.Masstransit.Helper.Extensions;
using Hangfire;
using Hangfire.PostgreSql;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Prometheus;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using Yarp.ReverseProxy.Configuration;

// using Asp.Versioning;
// using Microsoft.Extensions.Options;
// using Swashbuckle.AspNetCore.SwaggerGen;


namespace Greta.BO.Api
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var loggerFactory = new LoggerFactory();
            var hClient = new BoHubClient(null, loggerFactory.CreateLogger<BoHubClient>());
            services.AddSingleton<IBoHubClient>(hClient);

            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

            services.AddCors();

            services.AddControllers(opt =>
            {
                opt.Filters.Add<ResponseMappingFilter>();
                opt.Filters.Add<GlobalExceptionFilter>();
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            }).AddNewtonsoftJson(opt =>
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.Configure<HostOptions>(hostOptions =>
            {
                hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
            });

            services.Configure<MainOption>(Configuration.GetSection("Company"));

            //Adding Authentication
            AddAuthentication(services);

            services.AddAutoMapper(typeof(StoreProduct).GetTypeInfo().Assembly,
                typeof(AutoMapping).GetTypeInfo().Assembly);

            AddDBContext(services);

            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(
                    Configuration["ConnectionStrings:DefaultConnection"]
                )
                .UseMediatR());

            services.AddHealthChecks()
                // .AddCheck<DatabaseHealthCheck>("Database")
                // .AddDbContextCheck<SqlServerContext>()
                .AddUrlGroup(new Uri($"{Configuration.GetValue<string>("Enterprise:CorporateApi")}/health/ready"),
                    name: "CorporateApi")
                .AddUrlGroup(new Uri($"{Configuration.GetValue<string>("Enterprise:IdentityUrl")}/health/ready"),
                    name: "IdentityApi");

            AddSwagger(services);

            services.AddEFCore(Configuration)
                .AddIdentityService<string>()
                .AddStorageManager(Configuration)
                .AddRepositories<long, string, SqlServerContext>();
            services.AddBLServices<IBaseService>();


            // Register the MediatR request handlers
            services.ConfigureMediatR<IBaseService>();

            services.AddSingleton<MetricReporter>();

            services.AddMemoryCache(opt => { });

            // Add Authorization Handlers
            services.AddAuthorizationRequirementHandlers<IBaseService>();

            // Add Validation Handlers
            services.AddValidatorsFromAssembly(typeof(IBaseService).Assembly);

            // Order is important
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
            // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehaviour<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(DiagnosticPipeline<,>));

            services.AddSignalR(opt =>
            {
                opt.ClientTimeoutInterval = TimeSpan.FromMinutes(5);
                opt.HandshakeTimeout = TimeSpan.FromSeconds(30);
                //opt.MaximumReceiveMessageSize = 90000000;
                opt.MaximumReceiveMessageSize = null;
                opt.StreamBufferCapacity = 50;
            });


            services.AddHostedService<PriceBatchAdjustmentWorker>(); //-----
            services.AddHostedService<SynchrDeleterWorker>(); //-----
            services.AddHostedService<ExternalJobWorker>(); //------


            services.AddExportImportSupport();

            // services.AddMassTransitServices<Startup, MarkAssemble>(this.Configuration);
            //services.AddSingleton<IEmailHelper, EmailHelper>();
            services.AddMassTransitServices<BOApi_ConsumerMarkAssemble>(Configuration,
                prefix: $"BO-{Configuration["Company:CompanyCode"]}",
                endpoints: (s) =>
                {
                    List<Type> types = ((IEnumerable<Assembly>)AppDomain.CurrentDomain.GetAssemblies())
                        .Where<Assembly>((Func<Assembly, bool>)(x =>
                            x.FullName.Contains("Greta")))
                        .SelectMany<Assembly, Type>((Func<Assembly, IEnumerable<Type>>)(x =>
                            ((IEnumerable<Type>)x.GetTypes()).Where<Type>((Func<Type, bool>)(x => x.IsInterface &&
                                typeof(IRegisteredEventContract).IsAssignableFrom(x))))).ToList<Type>();

                    foreach (Type requestType in types)
                        s.AddRequestClient(requestType);

                    s.AddSenderRequestClients(services, options =>
                    {
                        options.LoadAdministration = true;
                        options.LoadEmail = true;
                        options.LoadNotifications = true;
                    });
                });
            SetupReverseProxy(services);

            services.AddWixSupport(opt =>
            {
                var url = Configuration["Wix:Url"];
                if(!string.IsNullOrWhiteSpace(url))
                opt.Url = url;
                opt.ClientId = Configuration["Wix:ClientId"];
                opt.ClientSecret = Configuration["Wix:ClientSecret"];
                opt.WebHookApiPublicKey = Configuration["Wix:WebhookKey"];
                opt.UrlShared = Configuration["Wix:UrlShared"];
                opt.RedirectUrl = Configuration["Wix:RedirectUrl"];
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IBackgroundJobClient backgroundJobs, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHangfireDashboard();
            }

            backgroundJobs.Enqueue(() => Console.WriteLine("Init BO Api Server!"));

            var rootPath = Configuration["Files:Root"];
            var content = "";
            if (!string.IsNullOrEmpty(rootPath))
                content = Path.Combine(rootPath, "Content");
            else
                content = Path.Combine(AppContext.BaseDirectory, "Content");

            if (!Directory.Exists(content)) Directory.CreateDirectory(content);

            env.WebRootFileProvider = new PhysicalFileProvider(content);
            env.WebRootPath = content;

            // app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(content),
                RequestPath = "/Content"
            });

            // global cors policy
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()
                .WithExposedHeaders("Token-Expired")
            ); // allow credentials

            app.UseSerilogRequestLogging();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseIdentityService<string>();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(x =>
                {
                    // var descriptions = app.DescribeApiVersiAsp.Versioning.Mvc.ApiExplorerons();
                    //
                    // // build a swagger endpoint for each discovered API version
                    // foreach (var description in descriptions)
                    // {
                    //     var url = $"/swagger/{description.GroupName}/swagger.json";
                    //     var name = description.GroupName.ToUpperInvariant();
                    //     x.SwaggerEndpoint(url, name);
                    // }
                    // x.SwaggerEndpoint("/swagger/v1/swagger.json", "Greta.BO.Api");
                });
            }

            app.UseMetricServer();
            app.UseHttpMetrics();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapReverseProxy().AllowAnonymous();
                AddHealthEndpoints(endpoints);

                if (env.IsDevelopment())
                {
                    endpoints.MapHangfireDashboard("/hangfire");
                }

                endpoints.MapHub<FrontHub>("/fronthub");

                endpoints.MapHub<CloudHub>("/cloudhub");


                endpoints.MapWixEndpoints("/wix");
            });
        }

        private static void AddHealthEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
            {
                Predicate = (check) => check.Tags.Contains("ready"),
            });
            ;
            // if we need used this only from one host
            // .RequireHost($"*:{Configuration["ManagementPort"]}");

            endpoints.MapHealthChecks("/health/live", new HealthCheckOptions()
            {
                Predicate = (_) => false
            });
            endpoints.MapHealthChecks("/health/details",
                new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                }
            );
        }

        private void SetupReverseProxy(IServiceCollection services)
        {
            var clusterId = "IdentityCluster";
            ClusterConfig identityCluster = new ClusterConfig()
            {
                ClusterId = clusterId,
                // LoadBalancingPolicy = ""
                Destinations = new Dictionary<string, DestinationConfig>()
                {
                    {
                        "identity", new DestinationConfig()
                        {
                            Address = $"{Configuration["Enterprise:IdentityUrl"]!}",
                            Health = $"{Configuration["Enterprise:IdentityUrl"]!}/health/ready"
                        }
                    }
                }
            };
            RouteConfig identityRoute = new RouteConfig()
            {
                RouteId = "identityRoute",
                ClusterId = clusterId,
                Match = new RouteMatch()
                {
                    Path = "/api/identity/{**rest}"
                },
                Transforms = new[]
                {
                    new Dictionary<string, string>()
                    {
                        { "PathPattern", "/{**rest}" }
                    }
                }
            };


            services.AddReverseProxy()
                .LoadFromMemory(new[] { identityRoute }, new[] { identityCluster });
            // .LoadFromConfig(Configuration.GetSection("ReverseProxy"));
        }

        private void AddDBContext(IServiceCollection services)
        {
            services.AddDbContext<SqlServerContext>(opt =>
            {
                if (!opt.IsConfigured)
                    opt.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), sqlopt =>
                        //opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), sqlopt =>
                    {
                        sqlopt.UseAdminDatabase("defaultdb");
                        sqlopt.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                        sqlopt.EnableRetryOnFailure(10, TimeSpan.FromSeconds(10),
                            null);
                    });
            }, ServiceLifetime.Transient);
        }

        private void AddAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var authenticationConfiguration = new AuthenticationConfiguration();
                    Configuration.Bind("Authentication", authenticationConfiguration);

                    // options.Authority = "";
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidAudience = authenticationConfiguration.Audience,
                        ValidIssuer = authenticationConfiguration.Issuer,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(authenticationConfiguration.AccessTokenSecret)),
                        ClockSkew = TimeSpan.Zero
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = (context) =>
                        {
                            StringValues values;

                            if (!context.Request.Query.TryGetValue("token", out values))
                            {
                                return Task.CompletedTask;
                            }

                            if (values.Count > 1)
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                context.Fail(
                                    "Only one 'token' query string parameter can be defined. " +
                                    $"However, {values.Count:N0} were included in the request."
                                );

                                return Task.CompletedTask;
                            }

                            var token = values.Single();

                            if (String.IsNullOrWhiteSpace(token))
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                context.Fail(
                                    "The 'token' query string parameter was defined, " +
                                    "but a value to represent the token was not included."
                                );

                                return Task.CompletedTask;
                            }

                            context.Token = token;

                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            // context.Response.StatusCode = 401;
                            // context.Response.ContentType = "application/json";
                            // var response = new CQRSResponse()
                            // {
                            //     Errors = new[] { "You are not authorized." },
                            //     StatusCode = HttpStatusCode.Unauthorized
                            // };
                            // var result = System.Text.Json.JsonSerializer.Serialize(response);
                            // context.Response.WriteAsync(result);
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            context.NoResult();
                            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                            // context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";

                            var msg = "The access token provided is not valid. " +
                                      context.Exception.GetBaseException().Message;
                            var stack = context.Exception.StackTrace;
                            //#endif

                            // var json = new ApiErrorResponse(msg)
                            // {
                            //     Detail = stack
                            // };

                            var json = new CQRSResponse<string>
                            {
                                StatusCode = HttpStatusCode.BadRequest,
                                Errors = new List<String>()
                                {
                                    msg, stack
                                }
                            };

                            //context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                            //context.Result = new JsonResult(json);


                            var response =
                                JsonConvert.SerializeObject(json);
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                                msg = "The access token provided has expired. " +
                                      context.Exception.GetBaseException().Message;
                                // json = new ApiErrorResponse(msg)
                                // {
                                //     Detail = stack
                                // };
                                var json1 = new CQRSResponse<string>
                                {
                                    StatusCode = HttpStatusCode.Forbidden,
                                    Errors = new List<String>()
                                    {
                                        msg, stack
                                    }
                                };
                                response =
                                    JsonConvert.SerializeObject(json1);
                            }

                            context.Response.WriteAsync(response);
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        private void AddSwagger(IServiceCollection services)
        {
            // services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            // services.AddSwaggerGen( options => options.OperationFilter<SwaggerDefaultValues>() );
            services.AddSwaggerGen(options =>
            {
                // options.OperationFilter<SwaggerDefaultValues>();
                var version = "v1";
                options.SwaggerDoc(version, new OpenApiInfo
                {
                    Title = $"Greta.BO.Api {version}",
                    Description = "Greta BO Api",
                    Version = version,
                    Contact = new OpenApiContact
                    {
                        Name = "Greta.BO",
                        Email = "chenry@journeybizsolutions.com"
                    }
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer <Token>'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                
                options.OperationFilter<AuthResponseOperationFilter>();

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
                options.EnableAnnotations();

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }

        private class AuthenticationConfiguration
        {
            public string Issuer { get; set; }
            public string Audience { get; set; }
            public string AccessTokenSecret { get; set; }
        }
        
        public class AuthResponseOperationFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                if (!operation.Responses.ContainsKey("401"))
                {
                    operation.Responses.Add("401", new OpenApiResponse { Description = "You are not authorized" });
                }
                if (!operation.Responses.ContainsKey("403"))
                {
                    operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });
                }
            }
        }
    }
}