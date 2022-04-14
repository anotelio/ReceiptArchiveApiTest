namespace ReceiptArchiveApi.Contracts.Defaults;

public static class LogAndExceptionMessagesDefaults
{
    public const string TopicDifferenceMessage = "Topic {0} has different configuration values. Values must be equivalent in order to the consistency of the application running. Values to change: {1}.";

    public const int KafkaLocalMessageCode = 1050;

    public const string KafkaLocalMessage = "The local kafka message has happened.";

    public const int GeneralErrorCode = 4000;

    public const string GeneralError = "A general handled error has occured.";

    public const int ValidationErrorCode = 4010;

    public const string ValidationError = "The validation error has occured.";

    public const int OperationCanceledErrorCode = 4100;

    public const string OperationCanceledError = "The operation was cancelled.";

    public const int ManuallyOperationCanceledErrorCode = 4101;

    public const string ManuallyOperationCanceledError = "The operation's cancellation was requested.";

    public const int KafkaErrorCode = 4200;

    public const string KafkaError = "Kafka error has occured.";

    public const int InternalErrorCode = 5000;

    public const string InternalError = "Internal error has occurred on the server.";

    public const int KafkaLocalErrorCode = 5050;

    public const string KafkaLocalError = "The local kafka error has occured.";

    public const int CriticalErrorCode = 6000;

    public const string CriticalError = "Critical internal error has occurred on the server.";
}
