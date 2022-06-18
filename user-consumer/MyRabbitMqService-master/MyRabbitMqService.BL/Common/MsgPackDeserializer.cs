using Confluent.Kafka;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyRabbitMqService.BL.Common
{
    public class MsgPackDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return MessagePackSerializer.Deserialize<T>(data.ToArray());
        }       
    }
}
