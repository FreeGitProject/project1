using EcommerceBackend.Application.Common;
using EcommerceBackend.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

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

            // Handle specific exceptions
            switch (exception)
            {
                case TokenException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return context.Response.WriteAsync(JsonSerializer.Serialize(
                        ApiResponse<object>.Failure(
                            message: "Token is invalid or expired.",
                            messageCode: "TOKEN_INVALID_OR_EXPIRED",
                            errors: new List<string> { exception.Message }
                        )
                    ));

                case ValidationException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return context.Response.WriteAsync(JsonSerializer.Serialize(
                        ApiResponse<object>.Failure(
                            message: "One or more validation errors occurred.",
                            messageCode: "VALIDATION_ERROR",
                            errors: new List<string> { exception.Message }
                        )
                    )); 
                case UnauthorizedAccessException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return context.Response.WriteAsync(JsonSerializer.Serialize(
                        ApiResponse<object>.Failure(
                            message: "Invalid email or password.",
                            messageCode: "INVALID_CREDENTIALS",
                            errors: new List<string> { exception.Message }
                        )
                    ));

                case KeyNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return context.Response.WriteAsync(JsonSerializer.Serialize(
                        ApiResponse<object>.Failure(
                            message: "Resource not found.",
                            messageCode: "RESOURCE_NOT_FOUND",
                            errors: new List<string> { exception.Message }
                        )
                    ));

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return context.Response.WriteAsync(JsonSerializer.Serialize(
                        ApiResponse<object>.Failure(
                            message: "An error occurred while processing your request.",
                            messageCode: "INTERNAL_SERVER_ERROR",
                            errors: new List<string> { exception.Message }
                        )
                    ));
            }
        }
    }
}