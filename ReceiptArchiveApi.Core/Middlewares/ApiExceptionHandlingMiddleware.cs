using System.Net;
using Confluent.Kafka;
using ReceiptArchiveApi.Contracts.Defaults;
using ReceiptArchiveApi.Contracts.Responses.Base;
using ReceiptArchiveApi.Core.ProgramExtensions.Logger;
using ReceiptArchiveApi.Utils.Extensions;

namespace ReceiptArchiveApi.Core.Middlewares;

public class ApiExceptionHandlingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<ApiExceptionHandlingMiddleware> logger;

    public ApiExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ApiExceptionHandlingMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await this.next(context);
        }
        catch (KafkaException kEx)
        {
            await this.HandleKafkaExceptionAsync(context, kEx);
        }
        catch (OperationCanceledException)
        when (context?.RequestAborted.IsCancellationRequested ?? false)
        {
            await this.HandleManuallyOperationCanceledExceptionAsync(context);
        }
        catch (OperationCanceledException opEx)
        {
            await this.HandleOperationCanceledExceptionAsync(context, opEx);
        }
        catch (Exception ex)
        {
            await this.HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleKafkaExceptionAsync(HttpContext context, KafkaException ex)
    {
        context.Response.StatusCode = (int)HttpStatusCode.FailedDependency;

        var requestBody = await context.Request.Body.GetStringFromStream();

        this.logger.LogKafkaError(
            LogAndExceptionMessagesDefaults.KafkaErrorCode,
            LogAndExceptionMessagesDefaults.KafkaError,
            ex.ToString(),
            context.Request.Method,
            context.Request.Path.Value,
            requestBody ?? "<no body>");

        await context.Response.WriteAsJsonAsync(BaseResponseDefaults.KafkaErrorResponse,
            ReceiptArchiveApiDefaults.DefaultJsonOptions);
    }

    private async Task HandleExceptionGenWithBodyAsync(HttpContext context, Exception ex,
        int statusCode, int code, string message, BaseResponse response)
    {
        context.Response.StatusCode = statusCode;

        var requestBody = await context.Request.Body.GetStringFromStream();

        this.logger.LogErrorWithInfo(ex,
            code,
            message,
            ex.AggExceptionMessages(),
            context.Request.Method,
            context.Request.Path.Value,
            requestBody ?? "<no body>");

        await context.Response.WriteAsJsonAsync(response, ReceiptArchiveApiDefaults.DefaultJsonOptions);
    }

    private async Task HandleManuallyOperationCanceledExceptionAsync(HttpContext context)
    {
        this.logger.LogDefaultMessageWarning(
            LogAndExceptionMessagesDefaults.ManuallyOperationCanceledErrorCode,
            LogAndExceptionMessagesDefaults.ManuallyOperationCanceledError,
            context.Request.Method,
            context.Request.Path.Value);

        await Task.CompletedTask;
    }

    private async Task HandleOperationCanceledExceptionAsync(HttpContext context, OperationCanceledException ex)
    {
        await this.HandleExceptionGenWithBodyAsync(context, ex, (int)HttpStatusCode.Gone,
            LogAndExceptionMessagesDefaults.OperationCanceledErrorCode, LogAndExceptionMessagesDefaults.OperationCanceledError,
            BaseResponseDefaults.OperationCancelledErrorResponse);
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        await this.HandleExceptionGenWithBodyAsync(context, ex, (int)HttpStatusCode.InternalServerError,
            LogAndExceptionMessagesDefaults.InternalErrorCode, LogAndExceptionMessagesDefaults.InternalError,
            BaseResponseDefaults.InternalErrorResponse);
    }
}
