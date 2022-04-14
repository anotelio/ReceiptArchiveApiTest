using System.Runtime.Serialization;

namespace ReceiptArchiveApi.Contracts.Exceptions
{
    [Serializable]
    public sealed class ReceiptArchiveApiException : ReceiptArchiveApiCommonException
    {
        public ReceiptArchiveApiException(int errorCode, string message)
            : base(errorCode, message)
        {
        }

        public ReceiptArchiveApiException(int errorCode, string message, string trace)
            : base(errorCode, message, trace)
        {
        }

        public ReceiptArchiveApiException(int errorCode, string message, Exception innerException)
            : base(errorCode, message, innerException)
        {
        }

        public ReceiptArchiveApiException(int errorCode, string message, Exception innerException, string trace)
            : base(errorCode, message, innerException, trace)
        {
        }

        public ReceiptArchiveApiException()
        {
        }

        public ReceiptArchiveApiException(string message): base(message)
        {
        }

        public ReceiptArchiveApiException(string message, Exception innerException) : base(message, innerException)
        {
        }

        private ReceiptArchiveApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
