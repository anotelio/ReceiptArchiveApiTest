using System.Net;
using ReceiptArchiveApi.Contracts.Responses.Base;

namespace ReceiptArchiveApi.Contracts.Defaults;

public static class BaseResponseDefaults
{
    public static readonly BaseResponse KafkaErrorResponse = new()
    {
        StatusCode = HttpStatusCode.FailedDependency,
        ErrorData = new List<ErrorData>()
        {
            new ErrorData()
            {
                ErrorCode = LogAndExceptionMessagesDefaults.KafkaErrorCode,
                ErrorMessage = LogAndExceptionMessagesDefaults.KafkaError
            }
        }
    };

    public static readonly BaseResponse OperationCancelledErrorResponse = new()
    {
        StatusCode = HttpStatusCode.Gone,
        ErrorData = new List<ErrorData>()
        {
            new ErrorData()
            {
                ErrorCode = LogAndExceptionMessagesDefaults.OperationCanceledErrorCode,
                ErrorMessage = LogAndExceptionMessagesDefaults.OperationCanceledError
            }
        }
    };

    public static readonly BaseResponse InternalErrorResponse = new()
    {
        StatusCode = HttpStatusCode.InternalServerError,
        ErrorData = new List<ErrorData>()
        {
            new ErrorData()
            {
                ErrorCode = LogAndExceptionMessagesDefaults.InternalErrorCode,
                ErrorMessage = LogAndExceptionMessagesDefaults.InternalError
            }
        }
    };

    public static readonly BaseResponse CriticalErrorResponse = new()
    {
        StatusCode = HttpStatusCode.InternalServerError,
        ErrorData = new List<ErrorData>()
        {
            new ErrorData()
            {
                ErrorCode = LogAndExceptionMessagesDefaults.CriticalErrorCode,
                ErrorMessage = LogAndExceptionMessagesDefaults.CriticalError
            }
        }
    };
}
