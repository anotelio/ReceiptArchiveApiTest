using System.Text.Json;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ReceiptArchiveApi.Contracts.Options;
using ReceiptArchiveApi.Contracts.Requests;
using ReceiptArchiveApi.Core.ProgramExtensions;
using ReceiptArchiveApi.Core.Services.Kafka;

namespace ReceiptArchiveApi.Core.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ApiController : ControllerBase
{
    private readonly string topic;
    private readonly KafkaDependentProducer<Null, string> producer;

    public ApiController(IOptions<KafkaSettings> kafkaConfig, KafkaDependentProducer<Null, string> producer)
    {
        this.topic = kafkaConfig.Value.TopicSettings.TopicName;
        this.producer = producer;
    }

    [HttpPost]
    public async Task<IActionResult> Get(Wrequest w)
    {
        Message<Null, string> message = new()
        {
            Value = JsonSerializer.Serialize(w)
        };

        await this.producer.ProduceAsync(topic, message, this.GetCancellationToken());

        return Ok();
    }
}
