namespace ReceiptArchiveApi.Contracts.Options
{
    public class KafkaSettings
    {
        public TopicSettingsValues TopicSettings { get; set; }

        public ProducerSettingsValues ProducerSettings { get; set; }

        public class TopicSettingsValues
        {
            public string TopicName { get; set; }

            public int? NumPartitions { get; set; }

            public short? ReplicationFactor { get; set; }

            public Dictionary<string, string> Configs { get; set; }
        }

        public class ProducerSettingsValues
        {
            public string BootstrapServers { get; set; }

            public int MessageTimeoutMs { get; set; }
        }

        public void Validate()
        {
            ArgumentNullException.ThrowIfNull(TopicSettings, $"{nameof(TopicSettings)} is not configured.");
            ArgumentNullException.ThrowIfNull(ProducerSettings, $"{nameof(ProducerSettings)} is not configured.");

            if (string.IsNullOrEmpty(TopicSettings.TopicName) || TopicSettings.TopicName.Length < 4)
                throw new ArgumentException($"{nameof(TopicSettings.TopicName)} is not configured correctly.");

            foreach (var propertyInfo in ProducerSettings.GetType().GetProperties())
            {
                var value = propertyInfo.GetValue(obj: ProducerSettings, null);
                if (value is null || (value is string && string.IsNullOrEmpty(value.ToString())))
                {
                    throw new ArgumentException($"{propertyInfo.Name} is not configured.");
                }
            }
        }
    }
}
