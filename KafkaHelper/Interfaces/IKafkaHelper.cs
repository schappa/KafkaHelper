using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaHelper.Interfaces
{
    public interface IKafkaHelper
    {
        Task<DeliveryResult<string, string>> ProduceKafkaMessageAsync(string topic, string key, string value);

        ConsumeResult<string, string> ConsumeKafkaMessage(string topic);

        Task<DeliveryResult<string, string>> ProduceKafkaMessageWithSchemaRegistryAsync(string topic, string key, string value);

        ConsumeResult<string, string> ConsumeKafkaMessageWithSchemaRegistry(string topic);
    }
}
