using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ticketing.Infrastructure.Messaging;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile(
    "appsettings.json",
    optional: false,
    reloadOnChange: true
);

builder.Services.Configure<RabbitMqOptions>(
    builder.Configuration.GetSection("RabbitMq")
);

var host = builder.Build();
host.Run();
