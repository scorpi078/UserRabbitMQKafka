using MessagePack;
using System;

namespace MyRabbitMqService.Models
{
    [MessagePackObject]
    public class User
    {
        [Key(0)]
        public int Id { get; set; }

        [Key(1)]
        public string Username { get; set; }

        [Key(2)]
        public DateTime LastSeenOnline { get; set; }
    }
}
