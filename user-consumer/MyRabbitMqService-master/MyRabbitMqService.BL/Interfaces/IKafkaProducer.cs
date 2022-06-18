using MyRabbitMqService.Models;
using System.Threading.Tasks;

namespace MyRabbitMqService.BL.Interfaces
{

    public interface IKafkaProducer
    {
        Task SendUserKafka(User u);
    }
}
