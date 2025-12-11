using System;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Universal.UsersService.Api.API.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
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
            catch (ValidationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/problem+json";
                var problemDetails = new Microsoft.AspNetCore.Mvc.ValidationProblemDetails
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = "One or more validation errors occurred.",
                    Detail = ex.Message,
                    Instance = context.Request.Path
                };
                // Opcional: agregar los errores individuales si están disponibles
                foreach (var error in ex.Errors)
                {
                    if (!problemDetails.Errors.ContainsKey(error.PropertyName))
                        problemDetails.Errors.Add(error.PropertyName, new[] { error.ErrorMessage });
                }
                await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var response = new { error = "Ocurrió un error inesperado. Intente más tarde." };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
