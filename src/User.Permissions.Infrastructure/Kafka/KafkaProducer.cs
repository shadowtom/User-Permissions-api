using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using User.Permissions.Domain.Entities;
using User.Permissions.Domain.Interfaces.Kafka;
using User.Permissions.Infrastructure.Kafka.Settings;

namespace User.Permissions.Infrastructure.Kafka
{
    public class KafkaProducer : IKafkaProducer
    {

        readonly IProducer<string, string> producer;
        private readonly KafkaProducerSettings settings;
        public KafkaProducer(IOptions<KafkaProducerSettings> settings)
        {
            this.settings = settings.Value;

            var config = new ProducerConfig
            {
                BootstrapServers = settings.Value.BrokersList,
                ClientId = settings.Value.Clientid,
                Partitioner = Partitioner.Murmur2
            };

            producer = new ProducerBuilder<string, string>(config).Build();
        }
        public async Task ProduceAsync(kafkaLog kafkaMessage)
        {
            try
            {
                await producer.ProduceAsync(settings.Topic, new Message<string, string>
                {
                    Key = kafkaMessage.id.ToString(),
                    Value = JsonSerializer.Serialize(kafkaMessage)
                });
            }
            catch (Exception ex)
            {
                Log.Error("{UtcNow} - Error producing message to Kafka - {Message}", DateTime.UtcNow,ex.Message);
            }
        }
    }
}
