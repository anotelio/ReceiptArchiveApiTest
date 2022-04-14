using Confluent.Kafka;

namespace ReceiptArchiveApi.Core.Services.Kafka
{
    public class KafkaDependentProducer<K, V>
    {
        private readonly IProducer<K, V> kafkaHandle;

        public KafkaDependentProducer(KafkaClientHandle handle)
        {
            kafkaHandle = new DependentProducerBuilder<K, V>(handle.Handle).Build();
        }

        public Task ProduceAsync(string topic, Message<K, V> message, CancellationToken cancellationToken = default)
            => this.kafkaHandle.ProduceAsync(topic, message, cancellationToken);

        public void Produce(string topic, Message<K, V> message, Action<DeliveryReport<K, V>> deliveryHandler = null)
            => this.kafkaHandle.Produce(topic, message, deliveryHandler);

        public void Flush(TimeSpan timeout)
            => this.kafkaHandle.Flush(timeout);
    }
}
