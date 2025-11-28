using OrdersTask.Application.DTOs;
using OrdersTask.Domain.Exceptions;
using System.Text.Json;

namespace OrdersTask.APIs.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unhandled exception occurred: {ex.Message}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = exception switch
            {
                NotFoundException notFound => new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Response = ApiResponse<object>.FailureResponse(notFound.Message)
                },
                ValidationException validation => new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Response = ApiResponse<object>.FailureResponse(validation.Message, validation.Errors)
                },
                _ => new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Response = ApiResponse<object>.FailureResponse("An internal server error occurred")
                }
            };

            context.Response.StatusCode = response.StatusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response.Response));
        }
    }
}
