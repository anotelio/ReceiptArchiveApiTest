namespace ReceiptArchiveApi.Core.ProgramExtensions.Logger;

public static partial class LoggerMessageDefinitions
{
    [LoggerMessage(EventId = 0,
        Message = "({code}){message}")]
    public static partial void LogDefaultMessage(this ILogger logger, LogLevel level, int code, string message);

    [LoggerMessage(EventId = 1,
        Message = "({code}){message} ::: {logMessage}")]
    public static partial void LogKafkaLocalMessage(this ILogger logger, LogLevel level, int code, string message, string logMessage);

    [LoggerMessage(EventId = 53,
        Message = "({code}){message} ::: {logMessage}",
        SkipEnabledCheck = true)]
    public static partial void LogKafkaLocalError(this ILogger logger, LogLevel level, int code, string message, string logMessage);

    [LoggerMessage(EventId = 41,
        Level = LogLevel.Error,
        Message = "({code}){message} ::: {kafkaEx}::: RequestInfo: {method} {path} ::: RequestBody: {body}",
        SkipEnabledCheck = true)]
    public static partial void LogKafkaError(this ILogger logger, int code, string message, string kafkaEx, string method, string path, string body);

    [LoggerMessage(EventId = 30,
        Level = LogLevel.Warning,
        Message = "({code}){message} ::: RequestInfo: {method} {path}")]
    public static partial void LogDefaultMessageWarning(this ILogger logger, int code, string message, string method, string path);

    [LoggerMessage(EventId = 52,
        Level = LogLevel.Error,
        Message = "({code}){message}",
        SkipEnabledCheck = true)]
    public static partial void LogDefaultMessageError(this ILogger logger, int code, string message);

    [LoggerMessage(EventId = 51,
        Level = LogLevel.Error,
        Message = "({code}){message} ::: {aggExMessages}",
        SkipEnabledCheck = true)]
    public static partial void LogExceptionMessagesError(this ILogger logger, Exception ex, int code, string message, string aggExMessages);

    [LoggerMessage(EventId = 50,
        Level = LogLevel.Error,
        Message = "({code}){message} ::: {aggExMessages} ::: RequestInfo: {method} {path} ::: RequestBody: {body}",
        SkipEnabledCheck = true)]
    public static partial void LogErrorWithInfo(this ILogger logger, Exception ex, int code, string message, string aggExMessages,
        string method, string path, string body);

    [LoggerMessage(EventId = 60,
        Level = LogLevel.Critical,
        Message = "({code}){message} ::: {aggExMessages}",
        SkipEnabledCheck = true)]
    public static partial void LogCriticalError(this ILogger logger, Exception ex, int code, string message, string aggExMessages);
}
