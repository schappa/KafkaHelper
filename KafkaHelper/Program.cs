using Confluent.Kafka;
using KafkaHelper.Extensions;
using System;


namespace Publix.Kafka
{
    internal class Program
    {
        private static IList<int> topicSequence;
        private static IList<string> messages = new List<string>() { "message1", "message2", "message3"};

        private static Random rng = new Random();
        static void Main(string[] args)
        {
            int messageIndex = 0;
            topicSequence = Enumerable.Range(1, 3).ToList();
            topicSequence.Shuffle();
            messages.Shuffle();

            IKafkaHelper kafkaProducerHelper = new KafkaHelper("appconfig.producer.json");


            ////Produce messages
            foreach (var item in topicSequence)
            {
                string key = Guid.NewGuid().ToString();
                string message = messages[messageIndex];

                string topic = $"topic_{item.ToString("00")}";
                Console.WriteLine($"Producing message {message} to topic {topic}");

                kafkaProducerHelper.ProduceKafkaMessageAsync(topic, key, message).Wait();

                ++messageIndex;
            }

            //Consume Messages
            topicSequence.Shuffle();

            IKafkaHelper kafkaConsumerHelper = new KafkaHelper("appconfig.consumer.json");

            foreach (var item in topicSequence)
            {
                string topic = $"topic_{item.ToString("00")}";
                var retVal = kafkaConsumerHelper.ConsumeKafkaMessage(topic);

                if (retVal != null)
                    Console.WriteLine($"Consuming message {retVal.Message.Value} from topic {topic}");
                else
                    Console.WriteLine($"No message(s) awaiting consumption in topic {topic}..");
            }
        }

    }
}