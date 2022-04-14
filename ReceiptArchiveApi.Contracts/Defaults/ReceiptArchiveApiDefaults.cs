using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReceiptArchiveApi.Contracts.Defaults;

public static class ReceiptArchiveApiDefaults
{
    public readonly static JsonSerializerOptions NumberAsStringJsonOptions = new()
    {
        WriteIndented = false,
        NumberHandling = JsonNumberHandling.WriteAsString,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public readonly static JsonSerializerOptions DefaultJsonOptions = new()
    {
        WriteIndented = false,
        PropertyNameCaseInsensitive = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public readonly static JsonSerializerOptions NoPolicyUnsafeJsonOptions = new()
    {
        WriteIndented = false,
        PropertyNameCaseInsensitive = false,
        PropertyNamingPolicy = null,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
}
