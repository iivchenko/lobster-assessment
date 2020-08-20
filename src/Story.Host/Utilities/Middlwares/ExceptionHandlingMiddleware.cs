using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Story.Application.Queries;
using System.Threading.Tasks;

namespace Story.Host.Utilities.Middlewares
{
    public sealed class ExceptionHandlingMiddleware
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
            catch (EntityNotFoundException e)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = "application/json";
                var error = new
                {
                    ErrorCode = StatusCodes.Status404NotFound,
                    Message = e.Message
                };

                await context.Response.WriteAsync(JsonConvert.SerializeObject(error));
            }
            catch
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }
}
