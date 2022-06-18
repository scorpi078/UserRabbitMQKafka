using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using MyRabbitMqService.BL.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyRabbitMqService.BL.Services
{
    public class KafkaConsumer<TKey, TValue> : IHostedService
    {
        private readonly IConsumer<TKey, TValue> consumer;

        public KafkaConsumer()
        {
            var config = new ConsumerConfig()
            {
                BootstrapServers = "localhost:9092",
                GroupId = Guid.NewGuid().ToString(),
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true,
                AutoCommitIntervalMs = 3000,
                FetchWaitMaxMs = 30,
            };

            consumer = new ConsumerBuilder<TKey, TValue>(config)
              .SetValueDeserializer(new MsgPackDeserializer<TValue>())
              .Build();

            consumer.Subscribe("test");
        }

        CancellationTokenSource cts = new CancellationTokenSource();

        public Task StartAsync(CancellationToken cancellationToken)
        {

            try
            {
                Task.Factory.StartNew(() =>
                {
                    while (!cts.IsCancellationRequested)
                    {
                        try
                        {
                            var cr = consumer.Consume(cts.Token);
                            UserCacheDictionary.Users.Add(cr.Message.Key, cr.Message.Value);
                            Console.WriteLine($"Id: {cr.Message.Key}, Username: {cr.Message.Value}");
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error {e.Error.Reason}");
                        }
                    }
                }, cts.Token);
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
