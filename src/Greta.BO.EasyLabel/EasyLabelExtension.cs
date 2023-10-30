using Microsoft.Extensions.DependencyInjection;

namespace Greta.BO.EasyLabel;

public static class EasyLabelExtension
{
    public static async Task<IServiceCollection> AddEasyLabelSupport(this IServiceCollection services)
    {
        return services;
    }
}