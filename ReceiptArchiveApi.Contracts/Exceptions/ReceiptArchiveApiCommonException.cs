using System.Runtime.Serialization;
using ReceiptArchiveApi.Contracts.Defaults;

namespace ReceiptArchiveApi.Contracts.Exceptions;

public abstract class ReceiptArchiveApiCommonException : Exception
{
    public int ErrorCode { get; }

    public string ErrorTrace { get; }

    protected ReceiptArchiveApiCommonException(int errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }

    protected ReceiptArchiveApiCommonException(int errorCode, string message, string trace) : this(errorCode, message)
    {
        ErrorCode = errorCode;
        ErrorTrace = trace;
    }

    protected ReceiptArchiveApiCommonException(int errorCode, string message, Exception innerException) : base(message, innerException)
    {
        ErrorCode = errorCode;
    }

    protected ReceiptArchiveApiCommonException(int errorCode, string message, Exception innerException, string trace) : this(errorCode, message, innerException)
    {
        ErrorTrace = trace;
    }

    protected ReceiptArchiveApiCommonException()
    {
        ErrorCode = LogAndExceptionMessagesDefaults.GeneralErrorCode;
    }

    protected ReceiptArchiveApiCommonException(string message) : base(message)
    {
        ErrorCode = LogAndExceptionMessagesDefaults.GeneralErrorCode;
    }

    protected ReceiptArchiveApiCommonException(string message, Exception innerException) : base(message, innerException)
    {
        ErrorCode = LogAndExceptionMessagesDefaults.GeneralErrorCode;
    }

    protected ReceiptArchiveApiCommonException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
