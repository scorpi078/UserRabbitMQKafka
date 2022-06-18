using System.Threading.Tasks;
using MyRabbitMqService.Models;

namespace MyRabbitMqService.BL.Interfaces
{
    public interface IRabbitMqService
    {
        Task GetUserAsync(User u);
    }
}
