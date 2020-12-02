using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using BasicApi.Items.Exceptions;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace BasicApi.Common.Mvc
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                await HandleErrorAsync(context, exception);
            }
        }

        private static Task HandleErrorAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            ErrorResponse response;

            if (exception is ServiceException applicationException)
            {
                context.Response.StatusCode = applicationException.StatusCode;

                response = new ErrorResponse
                {
                    ErrorCode = applicationException.ErrorCode,
                    ErrorDescription = exception.Message
                };
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                response = new ErrorResponse
                {
                    ErrorCode = ExceptionCodes.UnhandledError,
                    ErrorDescription = exception.Message,
                    Trace = exception.StackTrace,
                    Inner = exception.InnerException?.Message
                };
            }

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
