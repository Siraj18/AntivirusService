using System;
using System.Threading.Tasks;
using AntivirusService.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ScanUtils.Exceptions;

namespace AntivirusService.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, ILogger<ExceptionMiddleware> logger)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ValidationException exception)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new { Message = exception.Message });
            }
            catch (ObjectNotFoundException exception)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                logger.LogError("Object Not Found exception, Message={message}", exception.Message);
                await httpContext.Response.WriteAsJsonAsync(new { Message = "Object Not Found" });
            }
            catch (ScanErrorException exception)
            {
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                logger.LogError("ScanErrorException, Message={message}", exception.Message);
                await httpContext.Response.WriteAsJsonAsync(new { Message = exception.Message });
            }
            catch (Exception exception)
            {
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                logger.LogError("Unhandled exception, Message={message}", exception.Message);
                await httpContext.Response.WriteAsJsonAsync(new { Error = "Внутренняя ошибка сервера" });
            }
        }
    }
}