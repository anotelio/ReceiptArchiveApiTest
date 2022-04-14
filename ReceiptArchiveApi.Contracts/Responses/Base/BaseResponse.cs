using System.Net;

namespace ReceiptArchiveApi.Contracts.Responses.Base;

public interface IBaseResponse
{
    HttpStatusCode StatusCode { get; set; }

    IEnumerable<ErrorData> ErrorData { get; set; }
}

public class BaseResponse : IBaseResponse
{
    public HttpStatusCode StatusCode { get; set; }

    public IEnumerable<ErrorData> ErrorData { get; set; }
}

public class ErrorData
{
    public int ErrorCode { get; set; }

    public string ErrorMessage { get; set; }

    public string ErrorTrace { get; set; }
}
