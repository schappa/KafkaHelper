using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publix.Kafka
{
    public class KafkaHelper : IKafkaHelper
    {
        private readonly IConfiguration _config;

        public int ConsumerTimeout { get; set; } = 10000;

        public KafkaHelper(string appConfig)
        {
            _config = CreateKafkaConfig(appConfig);
        }

        public async Task<DeliveryResult<string, string>> ProduceKafkaMessageAsync(string topic, string key, string value)
        {
            DeliveryResult<string, string> deliveryResult;

            using (var producer = new ProducerBuilder<string, string>(
                _config.AsEnumerable()).Build())
            {
                deliveryResult = await producer.ProduceAsync(
                    topic,
                    new Message<string, string>
                    { Key = key, Value = value });

                producer.Flush(TimeSpan.FromSeconds(10));
            }

            return deliveryResult;
        }

        public ConsumeResult<string, string> ConsumeKafkaMessage(string topic)
        {
            ConsumeResult<string, string> consumeResult;

            using (var consumer = new ConsumerBuilder<string, string>(
                _config.AsEnumerable()).Build())
            {
                consumer.Subscribe(topic);

                consumeResult =  consumer.Consume(ConsumerTimeout);
                consumer.Close();
            }

            return consumeResult;
        }

        private IConfiguration CreateKafkaConfig(string appConfig)
        {
            JObject file = JObject.Parse(File.ReadAllText(appConfig));
            IDictionary<string, JToken> config = (JObject)file["KafkaConfig"];

            return new ConfigurationBuilder()
                .AddJsonStream(new MemoryStream(Encoding.ASCII.GetBytes(config.ToString())))
                .Build();
        }

    }
}
