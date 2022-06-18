using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyRabbitMqService.BL.DataFlow
{
   public interface IUserDataFlow
    {
        Task SendUser(byte[] data);
    }
}
