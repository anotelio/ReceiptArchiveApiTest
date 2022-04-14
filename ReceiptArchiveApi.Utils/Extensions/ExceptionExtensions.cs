namespace ReceiptArchiveApi.Utils.Extensions;

public static class ExceptionExtensions
{
    public static string AggExceptionMessages(this Exception ex)
    {
        if (ex.InnerException is null)
        {
            return ex.Message;
        }

        return string.Concat(ex.Message, " :: ", AggExceptionMessages(ex.InnerException));
    }
}
