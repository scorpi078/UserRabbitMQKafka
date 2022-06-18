using MyRabbitMqService.BL.Interfaces;
using MyRabbitMqService.Models;
using RabbitMQ.Client;
using System;
using System.Threading.Tasks;
using MessagePack;

namespace MyRabbitMqService.BL.Services
{
    public class RabbitMqService : IRabbitMqService, IDisposable
    {
        private readonly IModel _channel;
        private readonly IConnection _connection;

        public RabbitMqService()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("user", ExchangeType.Fanout);

            _channel.QueueDeclare("user", true, false, autoDelete: false);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }

        public async Task SendUserAsync(User u)
        {
            await Task.Factory.StartNew(() =>
            {
                var body = MessagePackSerializer.Serialize(u);

                _channel.BasicPublish("", "user", body: body);
            });
        }
    }
}
