using Confluent.Kafka;
using Microsoft.Extensions.Options;
using ReceiptArchiveApi.Contracts.Defaults;
using ReceiptArchiveApi.Contracts.Options;
using ReceiptArchiveApi.Core.ProgramExtensions.Logger;

namespace ReceiptArchiveApi.Core.Services.Kafka;

public sealed class KafkaClientHandle : IDisposable
{
    private readonly IProducer<byte[], byte[]> kafkaProducer;
    private readonly ILogger<KafkaClientHandle> logger;

    public KafkaClientHandle(IOptions<KafkaSettings> kafkaConfig, ILogger<KafkaClientHandle> logger)
    {
        this.logger = logger;
        var producerSettings = kafkaConfig.Value.ProducerSettings;
        ProducerConfig producerConfig = new()
        {
            BootstrapServers = producerSettings.BootstrapServers,
            MessageTimeoutMs = producerSettings.MessageTimeoutMs
        };

        this.kafkaProducer = new ProducerBuilder<byte[], byte[]>(producerConfig)
            .SetLogHandler((_, logMessage) => LogCallBack(logMessage))
            .Build();
    }

    public Handle Handle => this.kafkaProducer.Handle;

    public void Dispose()
    {
        kafkaProducer.Flush();
        kafkaProducer.Dispose();
    }

    private void LogCallBack(LogMessage logMessage)
    {
        var logLevel = (LogLevel)logMessage.LevelAs(LogLevelType.MicrosoftExtensionsLogging);

        switch (logLevel)
        {
            case >= LogLevel.Error:
                LogKafkaLocalError();
                break;
            default:
                LogKafkaLocalMessage();
                break;
        }

        void LogKafkaLocalError()
        {
            this.logger.LogKafkaLocalError(
                logLevel,
                LogAndExceptionMessagesDefaults.KafkaLocalErrorCode,
                LogAndExceptionMessagesDefaults.KafkaLocalError,
                logMessage.Message);
        }

        void LogKafkaLocalMessage()
        {
            this.logger.LogKafkaLocalMessage(
                logLevel,
                LogAndExceptionMessagesDefaults.KafkaLocalMessageCode,
                LogAndExceptionMessagesDefaults.KafkaLocalMessage,
                logMessage.Message);
        }
    }
}
