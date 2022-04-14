using ReceiptArchiveApi.Contracts.Defaults;
using ReceiptArchiveApi.Core.ProgramExtensions.Logger;
using ReceiptArchiveApi.Utils.Extensions;

namespace ReceiptArchiveApi.Core.Middlewares;

public class ApiCriticalExceptionHandlingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<ApiCriticalExceptionHandlingMiddleware> logger;

    public ApiCriticalExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ApiCriticalExceptionHandlingMiddleware> logger)
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
        catch (Exception ex)
        {
            await this.HandleCriticalExceptionAsync(context, ex);
        }
    }

    private async Task HandleCriticalExceptionAsync(HttpContext context, Exception ex)
    {
        this.logger.LogCriticalError(ex,
            LogAndExceptionMessagesDefaults.CriticalErrorCode,
            LogAndExceptionMessagesDefaults.CriticalError,
            ex.AggExceptionMessages());

        await context?.Response.WriteAsJsonAsync(BaseResponseDefaults.CriticalErrorResponse,
            ReceiptArchiveApiDefaults.DefaultJsonOptions);
    }
}
