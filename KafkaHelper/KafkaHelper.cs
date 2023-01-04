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

        public string TopicName { get; set; }
        public int ConsumerTimeout { get; set; } = 1000;

        public KafkaHelper(string appConfig, string topicName) 
        {
            _config = CreateKafkaConfig(appConfig);
            this.TopicName = topicName;
        }

        public KafkaHelper(string appConfig)
        {
            _config = CreateKafkaConfig(appConfig);
        }

        public async Task<DeliveryResult<string, string>> ProduceKafkaMessageAsync(string key, string value)
        {
            using (var producer = new ProducerBuilder<string, string>(
                _config.AsEnumerable()).Build())
            {
                return await producer.ProduceAsync(
                    TopicName,
                    new Message<string, string>
                    { Key = key.ToString(), Value = value });
            }
        }

        public ConsumeResult<string, string> ConsumeKafkaMessage()
        {
            using (var consumer = new ConsumerBuilder<string, string>(
                _config.AsEnumerable()).Build())
            {
                consumer.Subscribe(TopicName);
                return consumer.Consume(ConsumerTimeout);
            }
        }

        private IConfiguration CreateKafkaConfig(string appConfig)
        {
            JObject file = JObject.Parse(File.ReadAllText("appconfig.json"));
            IDictionary<string, JToken> config = (JObject)file["KafkaConfig"];

            return new ConfigurationBuilder()
                .AddJsonStream(new MemoryStream(Encoding.ASCII.GetBytes(config.ToString())))
                .Build();
        }

    }
}
