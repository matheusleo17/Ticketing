using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ticketing.Infrastructure.Messaging;
using Ticketing.Worker;
var builder = Host.CreateApplicationBuilder(args);

// força o carregamento do appsettings da raiz
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.Configure<RabbitMqOptions>(
    builder.Configuration.GetSection("RabbitMq")
);

builder.Services.AddHostedService<OrderCreatedConsumerHostedService>();

var host = builder.Build();
host.Run();

