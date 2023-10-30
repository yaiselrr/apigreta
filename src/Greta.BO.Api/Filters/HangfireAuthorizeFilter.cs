using System.Diagnostics.CodeAnalysis;
using Hangfire.Dashboard;

namespace Greta.BO.Api.Filters
{
    [ExcludeFromCodeCoverage]
    public class HangfireAuthorizeFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([Hangfire.Annotations.NotNull] DashboardContext context)
        {
            //var httpcontext = context.GetHttpContext();
            //return httpcontext.User.Identity.IsAuthenticated;
            return true;
        }
    }
}