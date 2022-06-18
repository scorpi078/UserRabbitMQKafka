using MessagePack;
using MyRabbitMqService.BL.Common;
using MyRabbitMqService.BL.Interfaces;
using MyRabbitMqService.BL.Services;
using MyRabbitMqService.Models;
using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace MyRabbitMqService.BL.DataFlow
{
    public class UserDataFlow : IUserDataFlow
    {
        private readonly TransformBlock<byte[], User> entryBlock;

        private readonly IKafkaProducer _kafkaProducer;

        public UserDataFlow(IKafkaProducer kafkaProducer)
        {
            _kafkaProducer = kafkaProducer;


            entryBlock = new TransformBlock<byte[], User>(data =>
              MessagePackSerializer.Deserialize<User>(data)
             );

            var enrichBlock = new TransformBlock<User, User>(u =>
            {
                u.Username = "edited";
                u.LastSeenOnline = DateTime.Now;
                return u;
            });

            var publishBlock = new ActionBlock<User>(u =>
            {
                Console.WriteLine($"Username {u.Username} , Updated date {u.LastSeenOnline}");
                
                _kafkaProducer.SendUserKafka(u);                
            });

            var linkOptions = new DataflowLinkOptions()
            {
                PropagateCompletion = true
            };

            entryBlock.LinkTo(enrichBlock, linkOptions);
            enrichBlock.LinkTo(publishBlock, linkOptions);
        }

        public async Task SendUser(byte[] data)
        {
            await entryBlock.SendAsync(data);
        }
    }
}
