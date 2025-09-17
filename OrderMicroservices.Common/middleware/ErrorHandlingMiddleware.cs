using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace OrderMicroservices.Common.Middleware
{
    public class ErrorHandlingMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Executa o próximo middleware/handler
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                Message = "Ocorreu um erro inesperado.",
                Detail = exception.Message, 
            };

            var payload = JsonSerializer.Serialize(response);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(payload);
        }
    }
}
