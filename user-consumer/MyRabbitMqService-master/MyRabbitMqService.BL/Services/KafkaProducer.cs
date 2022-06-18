using Confluent.Kafka;
using MyRabbitMqService.BL.Common;
using MyRabbitMqService.BL.Interfaces;
using MyRabbitMqService.Models;
using System.Threading.Tasks;

namespace MyRabbitMqService.BL.Services
{
    public class KafkaProducer : IKafkaProducer
    {
        private static IProducer<int, User> _producer;

        public KafkaProducer()
        {
            ProducerConfig config = new ProducerConfig()
            {
                BootstrapServers = "localhost:9092"
            };

            _producer = new ProducerBuilder<int, User>(config)
                .SetValueSerializer(new MsgPackSerializer<User>())
                .Build();
        }

        public async Task SendUserKafka(User u)
        {
            var message = new Message<int, User>()
            {
                Key = u.Id,
                Value = u
            };

            var result = await _producer.ProduceAsync("test", message);
        }
    }
}
