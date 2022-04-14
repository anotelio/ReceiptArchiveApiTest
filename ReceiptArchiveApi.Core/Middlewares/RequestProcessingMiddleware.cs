namespace ReceiptArchiveApi.Core.Middlewares;

public class RequestProcessingMiddleware
{
    private readonly RequestDelegate next;

    public RequestProcessingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.EnableBuffering();
        await this.next(context);
    }
}
