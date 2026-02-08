using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Ticketing.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Ticketing.Infrastructure.Persistence.Repositories;
using Ticketing.Application.Interfaces;



namespace Ticketing.Infrastructure.DependencyInjection
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var usePost = services.AddDbContext<TicketingDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

            });


            services.AddScoped<ITicketRepository, EfTicketRepository>();
            services.AddScoped<IOrderRepository, EfOrderRepository>();

            return services;
        }

    }
}
