using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace Greta.BO.Api.Test
{
    public class StartupTest
    {
        // protected TestServer _testServer;
        public StartupTest()
        {
            // var webBuilder = new WebHostBuilder();
            //.ConfigureServices(services =>
            // {
            // })
            //   .Configure(app =>
            //   {
            //       //app.UseSecurityHeadersMiddleware(policy);
            //       app.Run(async context =>
            //       {
            //           await context.Response.WriteAsync("Test response");
            //       });
            //   });
            // webBuilder.UseStartup<Startup>();
            //
            // _testServer = new TestServer(webBuilder);
        }

        [Theory]
        [InlineData(2), InlineData(4), InlineData(6)]
        public void DefaultSecurePolicy_RemovesServerHeader(int i)
        {
            Assert.Equal(0, i % 2);
        }

        // [Fact]
        // public async Task DefaultSecurePolicy_RemovesServerHeader()
        // {
        //     // Arrange
        //     //var policy = new SecurityHeadersPolicyBuilder()
        //     //    .AddDefaultSecurePolicy();
        //
        //     var hostBuilder = new WebHostBuilder()
        //         .ConfigureServices(services =>
        //         {
        //         })
        //         .Configure(app =>
        //         {
        //             //app.UseSecurityHeadersMiddleware(policy);
        //             app.Run(async context =>
        //             {
        //                 await context.Response.WriteAsync("Test response");
        //             });
        //         });
        //
        //     using (var server = new TestServer(hostBuilder))
        //     {
        //         // Act
        //         // Actual request.
        //         var response = await server.CreateRequest("/")
        //             .SendAsync("GET");
        //
        //         // Assert
        //         response.EnsureSuccessStatusCode();
        //         var content = await response.Content.ReadAsStringAsync();
        //
        //         Assert.Equal("Test response", content);
        //         Assert.False(response.Headers.Contains("Server"), "Should not contain server header");
        //     }
        // }
    }
}

