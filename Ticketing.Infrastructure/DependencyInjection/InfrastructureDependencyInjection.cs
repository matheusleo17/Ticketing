using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;



namespace Ticketing.Infrastructure.DependencyInjection
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }

    }
}
