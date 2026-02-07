using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using Ticketing.Application.Events;

var factory = new ConnectionFactory
{
    HostName = "localhost",
    UserName = "guest",
    Password = "guest"
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

var exchange = "ticketing.events";

// garante que a exchange existe
channel.ExchangeDeclare(
    exchange: exchange,
    type: ExchangeType.Topic,
    durable: true,
    autoDelete: false
);

// cria um evento fake só pra teste
var @event = new OrderCreated(
    Guid.NewGuid(),
    Guid.NewGuid()
);

// serializa
var json = JsonSerializer.Serialize(@event);
var body = Encoding.UTF8.GetBytes(json);

// publica
channel.BasicPublish(
    exchange: exchange,
    routingKey: "order.created",
    basicProperties: null,
    body: body
);

Console.WriteLine("OrderCreated published!");
