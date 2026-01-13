using CleanArchitectureRealEstate.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace CleanArchitectureRealEstate.WebAPI.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                await WriteResponse(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (ValidationDomainException ex)
            {
                await WriteResponse(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (UnauthorizedDomainException ex)
            {
                await WriteResponse(context, HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (Exception ex)
            {
                await WriteResponse(context, HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private static async Task WriteResponse(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                error = message,
                statusCode = (int)statusCode
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
