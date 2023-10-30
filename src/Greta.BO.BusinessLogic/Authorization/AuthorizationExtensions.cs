using System.Diagnostics.CodeAnalysis;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Microsoft.Extensions.DependencyInjection;

namespace Greta.BO.BusinessLogic.Authorization
{
    [ExcludeFromCodeCoverage]
    public static class AuthorizationExtensions
    {
        public static void AddAuthorizationRequirementHandlers<IEntity>(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssemblyOf<IEntity>()
                .AddClasses(classes => classes.AssignableTo<IRequirementHandler>())
                .AsImplementedInterfaces()
                .WithTransientLifetime());
        }
    }
}