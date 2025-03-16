using EcommerceBackend.Application.Common;
using EcommerceBackend.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace EcommerceBackend.Presentation.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            if (exception is TokenException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                var response = ApiResponse<object>.Failure(
                    message: "Token is invalid or expired.",
                    messageCode: "TOKEN_INVALID_OR_EXPIRED",
                    errors: new List<string> { exception.Message }
                );
                return context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var genericResponse = ApiResponse<object>.Failure(
                message: "An error occurred while processing your request.",
                messageCode: "INTERNAL_SERVER_ERROR",
                errors: new List<string> { exception.Message }
            );
            return context.Response.WriteAsync(JsonSerializer.Serialize(genericResponse));
        }
    }
}
