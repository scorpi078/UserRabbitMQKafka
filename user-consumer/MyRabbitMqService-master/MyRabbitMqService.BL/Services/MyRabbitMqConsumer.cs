using MessagePack;
using Microsoft.Extensions.Hosting;
using MyRabbitMqService.BL.DataFlow;
using MyRabbitMqService.BL.Interfaces;
using MyRabbitMqService.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyRabbitMqService.BL.Services
{
    public class MyRabbitMqConsumer : IHostedService, IDisposable
    {
        private readonly IModel _channel;
        private readonly IConnection _connection;
        private readonly IRabbitMqService _rabbitMqService;
        private readonly IUserDataFlow _userDataFlow;

        public MyRabbitMqConsumer(IUserDataFlow userDataFlow)
        {
    
            _userDataFlow = userDataFlow;

            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("user", ExchangeType.Fanout, durable: false);

            _channel.QueueDeclare("user", true, false, autoDelete: false);

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (sender, ea) =>
            {
                User user = MessagePackSerializer.Deserialize<User>(ea.Body);

                Console.WriteLine($"id {user.Id} - Name {user.Username} - Last Seen Online {user.LastSeenOnline}");
                _userDataFlow.SendUser(ea.Body.ToArray());
                UserCache.Cache.Add(user);
            };


            _channel.BasicConsume("user", true, consumer);

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }

        public async Task GetUserAsync(User u)
        {
           await Task.Factory.StartNew(() =>
           {
               var serialize = JsonConvert.SerializeObject(u);
               var body = Encoding.UTF8.GetBytes(serialize);

                return _rabbitMqService.GetUserAsync(u);
              
           });
        }
    }
}
