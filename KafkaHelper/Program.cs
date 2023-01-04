namespace Publix.Kafka
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IKafkaHelper kafkaHelper = new KafkaHelper("appconfig.json");
        }
    }
}