namespace User.Permissions.Infrastructure.Kafka.Settings
{
    public class KafkaProducerSettings
    {
        public string BrokersList { get; set; }
        public string Topic { get; set; }
        public string Clientid { get; set; }
    }
}