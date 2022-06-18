using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace MyRabbitMqService.Models
{
   public class UserCache
    {
        public static ConcurrentBag<User> Cache { get; set; } = new ConcurrentBag<User>();
    }
}
