using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Greta.BO.Wix.Apis;
using Greta.BO.Wix.Handlers.Commands.ProcessNewOnlineStore;
using Greta.BO.Wix.Handlers.Commands.Webhook;
using Greta.BO.Wix.Models;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;

namespace Greta.BO.Wix.Extentions;

public static class IntegrationExtensions
{

    public static IServiceCollection AddWixSupport(this
            IServiceCollection services,
            Action<WixOptions> buildOption
        )
    {
        var options = new WixOptions()
        {
            Url = "https://www.wixapis.com",
            ClientId = null,
            ClientSecret = null
        };
        buildOption?.Invoke(options);


        services.AddSingleton(options);

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        services.AddSingleton(new WixApiClient(options));

        return services;
    }

    public static IEndpointRouteBuilder MapWixEndpoints(this
            IEndpointRouteBuilder endpoints,
        string pattern)
    {

        var mediator = endpoints.ServiceProvider.GetRequiredService<IMediator>();

        endpoints.MapPost($"{pattern}/webhook", async (context) =>
        {
            try
            {
                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8);
                var plainText = await reader.ReadToEndAsync();
                await mediator.Publish(new ProcessWebhook(plainText));
                context.Response.StatusCode = 200;
            }
            catch (Exception _)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }).AllowAnonymous();

        endpoints.MapPost($"{pattern}/install", async (context) =>
             {
                 //  var httpClient = new HttpClient();

                 try
                 {
                     using var reader = new StreamReader(context.Request.Body, Encoding.UTF8);
                     var requestBody = await reader.ReadToEndAsync();
                     await mediator.Publish(new ProcessNewOnlineStore(requestBody));
                     context.Response.StatusCode = 200;
                 }
                 catch (Exception _)
                 {
                     context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                 }
             }).AllowAnonymous();

        return endpoints;
    }
}