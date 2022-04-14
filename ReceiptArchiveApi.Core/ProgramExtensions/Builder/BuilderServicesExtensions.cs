using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Options;
using ReceiptArchiveApi.Contracts.Defaults;
using ReceiptArchiveApi.Contracts.Options;
using ReceiptArchiveApi.Contracts.Validators;
using ReceiptArchiveApi.Core.ProgramExtensions.Validator;
using ReceiptArchiveApi.Core.Services.Kafka;

namespace ReceiptArchiveApi.Core.ProgramExtensions.Builder;

public static class BuilderServicesExtensions
{
    public static IServiceCollection ConfigureKafka(this IServiceCollection services, IConfiguration config)
{
        services.Configure<KafkaSettings>(config.GetSection(nameof(KafkaSettings)));
        services.AddSingleton<KafkaClientHandle>();
        services.AddSingleton<KafkaDependentProducer<Null, string>>();

        return services;
    }

    public static IMvcBuilder ConfigureJsonOptions(this IMvcBuilder builder)
    {
        builder.AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.WriteIndented = false;
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        return builder;
    }

    public static IMvcBuilder ConfigureFluentValidation(this IMvcBuilder builder)
    {
        builder.AddFluentValidation(options =>
        {
            options.RegisterValidatorsFromAssemblyContaining<WrequestValidator>(lifetime: ServiceLifetime.Singleton);
            options.DisableDataAnnotationsValidation = true;
            options.ImplicitlyValidateChildProperties = true;
            ValidatorOptions.Global.DisplayNameResolver = (_, memberInfo, expression) =>
                CamelCasePropertyNameResolver.ResolvePropertyName(memberInfo, expression);
        });

        return builder;
    }

    public static IApplicationBuilder OptionsValidate(this IApplicationBuilder app)
    {
        var kafkaOptions = app.ApplicationServices.GetRequiredService<IOptions<KafkaSettings>>();
        kafkaOptions.Value.Validate();

        return app;
    }

    public static void KafkaAdminClientConfigures(this IApplicationBuilder app)
    {
        var kafkaOptions = app.ApplicationServices.GetRequiredService<IOptions<KafkaSettings>>().Value;
        var adminConfig = KafkaAdminClient.CreateConfig(kafkaOptions.ProducerSettings.BootstrapServers);
        using var adminClient = KafkaAdminClient.CreateAdminClient(adminConfig);

        bool topicExists = KafkaAdminClient.TopicExists(adminClient, kafkaOptions.TopicSettings);

        if (!topicExists)
        {
            KafkaAdminClient.CreateTopicAsync(adminClient, kafkaOptions.TopicSettings)
                .GetAwaiter().GetResult();

            return;
        }

        KafkaAdminClient.CheckTopicConfigAsync(adminClient, kafkaOptions.TopicSettings)
            .GetAwaiter().GetResult();
    }
}
