namespace ReceiptArchiveApi.Core.Middlewares;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseApiCriticalExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ApiCriticalExceptionHandlingMiddleware>();
    }

    public static IApplicationBuilder UseApiExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ApiExceptionHandlingMiddleware>();
    }

    public static IApplicationBuilder UseRequestProcessing(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestProcessingMiddleware>();
    }
}
