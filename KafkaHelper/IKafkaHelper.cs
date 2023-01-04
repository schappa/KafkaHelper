using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publix.Kafka
{
    public interface IKafkaHelper
    {
        Task<DeliveryResult<string, string>> ProduceKafkaMessageAsync(string key, string value);

        ConsumeResult<string, string> ConsumeKafkaMessage();
    }
}
