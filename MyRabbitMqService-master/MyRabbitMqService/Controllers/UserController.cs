using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyRabbitMqService.BL.Interfaces;
using MyRabbitMqService.Models;

namespace MyRabbitMqService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IRabbitMqService _rabbitMqService;
        private readonly ILogger<UserController> _logger;


        public UserController(ILogger<UserController> logger, IRabbitMqService rabbitMqService)
        {
            _logger = logger;
            _rabbitMqService = rabbitMqService;
        }

        [HttpPost]
        public async Task<IActionResult> SendUser([FromBody] User p)
        {
            await _rabbitMqService.SendUserAsync(p);

            return Ok();
        }
    }
}
