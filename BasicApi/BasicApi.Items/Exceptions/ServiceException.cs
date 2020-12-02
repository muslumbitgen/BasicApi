using System;

namespace BasicApi.Items.Exceptions
{
    public class ServiceException : Exception
    {
        public int StatusCode { get; private set; }

        public string ErrorCode { get; private set; }

        public ServiceException(string message, int code = 400, string errorCode = "") : base(message)
        {
            StatusCode = code;
            ErrorCode = errorCode;
        }
    }
}
