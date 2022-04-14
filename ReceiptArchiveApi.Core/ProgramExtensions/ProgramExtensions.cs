using Microsoft.AspNetCore.Mvc;

namespace ReceiptArchiveApi.Core.ProgramExtensions;

public static class ProgramExtensions
{
    public static bool IsLocal(this IHostEnvironment env)
    {
        return env.IsEnvironment("Local");
    }

    public static CancellationToken GetCancellationToken(this ControllerBase controller)
    {
        return controller?.HttpContext?.RequestAborted ?? default;
    }
}
