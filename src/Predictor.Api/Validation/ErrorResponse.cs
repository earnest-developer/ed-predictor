using System;

namespace Predictor.Api.Validation
{
    public class ErrorResponse
    {
        public ErrorResponse(string requestId, string errorType, string errorCode)
        {
            if (string.IsNullOrEmpty(requestId)) throw new ArgumentNullException(nameof(requestId));
            if (string.IsNullOrEmpty(errorType)) throw new ArgumentNullException(nameof(errorType));

            RequestId = requestId;
            ErrorType = errorType;
            ErrorCode = errorCode;
        }

        public string RequestId { get; }
        public string ErrorType { get; }
        public string ErrorCode { get; }
    }
}