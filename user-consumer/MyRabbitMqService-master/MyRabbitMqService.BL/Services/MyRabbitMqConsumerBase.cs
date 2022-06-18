using System.Threading;
using System.Threading.Tasks;

namespace MyRabbitMqService.BL.Services
{
    internal class MyRabbitMqConsumerBase
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}