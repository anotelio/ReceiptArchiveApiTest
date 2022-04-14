using System.Text.Json;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using ReceiptArchiveApi.Contracts.Defaults;
using ReceiptArchiveApi.Contracts.Options;
using static ReceiptArchiveApi.Contracts.Options.KafkaSettings;

namespace ReceiptArchiveApi.Core.Services.Kafka;

public static class KafkaAdminClient
{
    public static AdminClientConfig CreateConfig(string bootstrapServers)
    {
        return new()
        {
            BootstrapServers = bootstrapServers
        };
    }

    public static IAdminClient CreateAdminClient(AdminClientConfig adminClientConfig)
    {
        return new AdminClientBuilder(adminClientConfig).Build();
    }

    public static bool TopicExists(IAdminClient adminClient, TopicSettingsValues topicConfig)
    {
        var meta = adminClient.GetMetadata(topicConfig.TopicName, TimeSpan.FromSeconds(2))
            .Topics.Single().Error;

        return !(meta.IsError || meta.IsLocalError || meta.IsBrokerError || meta.IsFatal);
    }

    public static Task CreateTopicAsync(IAdminClient adminClient, TopicSettingsValues topicConfig)
    {
        TopicSpecification topicSpecification = new()
        {
            Name = topicConfig.TopicName,
            ReplicationFactor = topicConfig.ReplicationFactor ?? -1,
            NumPartitions = topicConfig.NumPartitions ?? -1,
            Configs = topicConfig.Configs
        };

        return adminClient.CreateTopicsAsync(new[] { topicSpecification });
    }

    public static async Task CheckTopicConfigAsync(IAdminClient adminClient, TopicSettingsValues topicConfig)
    {
        ConfigResource configResource = new()
        {
            Name = topicConfig.TopicName,
            Type = ResourceType.Topic
        };

        var descrConfigList = await adminClient.DescribeConfigsAsync(new[] { configResource });
        var descrConfig = descrConfigList.SingleOrDefault().Entries;
        var descrConfigValues = descrConfig.ToDictionary(d => d.Key, d => d.Value.Value);

        var topicConfigDiff = topicConfig.Configs
            .Except(descrConfigValues)
            .ToDictionary(entry => entry.Key, GetDictionaryElement(descrConfigValues));

        if (topicConfigDiff?.Count > 0)
        {
            var topicConfigDiffJson = JsonSerializer.Serialize(topicConfigDiff,
                options: ReceiptArchiveApiDefaults.NoPolicyUnsafeJsonOptions);

            var errorMessage = string.Format(LogAndExceptionMessagesDefaults.TopicDifferenceMessage,
                topicConfig.TopicName, topicConfigDiffJson);

            throw new ArgumentException(string.Concat(nameof(KafkaSettings.TopicSettings),
                " is not configured correctly. ", errorMessage));
        }

        static Func<KeyValuePair<string, string>, string> GetDictionaryElement(
            Dictionary<string, string> descrConfigValues)
        {
            return entry =>
            {
                if (descrConfigValues.TryGetValue(entry.Key, out string val))
                {
                    return $"{entry.Value} <=> {val}";
                }

                return "<wrong parameter name>";
            };
        }
    }
}
