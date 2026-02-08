using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Ticketing.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Interfaces;
using Ticketing.Infrastructure.Time;
using Ticketing.Infrastructure.Messaging;
using Ticketing.Application.UseCases;



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
            services.Configure<RabbitMqOptions>(options =>
            {
                configuration.GetSection("RabbitMq").Bind(options);
            });


            services.AddScoped<ITicketRepository, EFTicketRepository>();
            services.AddScoped<IOrderRepository, EFOrderRepository>();
            services.AddScoped<IClock, SystemClock>();
            services.AddScoped<IEventBus, RabbitMqEventBus>();
            services.AddScoped<CreateOrderUseCase>();


            return services;
        }

    }
}
