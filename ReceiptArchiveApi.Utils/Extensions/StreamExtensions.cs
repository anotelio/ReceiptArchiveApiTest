namespace ReceiptArchiveApi.Utils.Extensions;

public static class StreamExtensions
{
    public static async Task<string> GetStringFromStream(this Stream stream)
    {
        if (stream?.Length == 0 || stream?.CanRead != true)
            return null;

        if (stream.CanSeek)
            stream.Position = 0;

        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}
