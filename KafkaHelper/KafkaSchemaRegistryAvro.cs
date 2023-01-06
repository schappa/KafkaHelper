using KafkaHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaHelper
{
    public class KafkaSchemaRegistryAvro : IKafkaSchemaRegistry
    {
        public string BootstrapServers { get; }
        public string SchemaRegistryUrl { get; }

        public KafkaSchemaRegistryAvro(string bootstrapServers, string schemaRegistryUrl)
        {
            BootstrapServers= bootstrapServers;
            SchemaRegistryUrl= schemaRegistryUrl;
        }
    }
}
