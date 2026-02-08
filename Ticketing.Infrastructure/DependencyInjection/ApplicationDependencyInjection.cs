using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ticketing.Application.UseCases;

namespace Ticketing.Infrastructure.DependencyInjection
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services) 
        {

            services.AddScoped<CreateOrderUseCase>();
            return services;
        }
    }
}
