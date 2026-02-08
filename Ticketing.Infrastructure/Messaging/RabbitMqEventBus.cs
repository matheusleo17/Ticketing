using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Ticketing.Application.Interfaces;

namespace Ticketing.Infrastructure.Messaging
{
    public sealed class RabbitMqEventBus : IEventBus, IDisposable
    {
        private readonly RabbitMqOptions _options;
        private readonly ConnectionFactory _factory;

        private IConnection? _connection;
        private IModel? _channel;

        public RabbitMqEventBus(IOptions<RabbitMqOptions> options)
        {
            _options = options.Value;

            _factory = new ConnectionFactory
            {
                HostName = _options.Host,
                Port = _options.Port,
                UserName = _options.Username,
                Password = _options.Password,
                DispatchConsumersAsync = true
            };
        }

        public Task Publish<T>(T @event)
        {
            EnsureConnection();

            var eventName = typeof(T).Name;
            var routingKey = ToRoutingKey(eventName);

            var body = Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(@event)
            );

            var properties = _channel!.CreateBasicProperties();
            properties.ContentType = "application/json";
            properties.DeliveryMode = 2;

            _channel.BasicPublish(
                exchange: _options.Exchange,
                routingKey: routingKey,
                basicProperties: properties,
                body: body
            );

            return Task.CompletedTask;
        }

        private void EnsureConnection()
        {
            if (_connection is { IsOpen: true } && _channel is { IsOpen: true })
                return;

            _connection?.Dispose();
            _channel?.Dispose();

            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(
                exchange: _options.Exchange,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false
            );
        }

        private static string ToRoutingKey(string name)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < name.Length; i++)
            {
                if (char.IsUpper(name[i]) && i > 0)
                    sb.Append('.');
                sb.Append(char.ToLowerInvariant(name[i]));
            }
            return sb.ToString();
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
