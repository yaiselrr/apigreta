using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Greta.BO.BusinessLogic.Extensions
{
    public static class Dependencies
    {
        public static IServiceCollection ConfigureMediatR<TDependency>(
            this IServiceCollection services)
        {
            //AppDomain.CurrentDomain.GetAssemblies(
            // return services
            //     .AddMediatR(typeof(TDependency).Assembly);
            
            // services.AddMediatR(typeof(Startup));
            return services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            // services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblyContaining(typeof(Startup)));
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek = DayOfWeek.Sunday)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}