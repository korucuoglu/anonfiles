using FileUpload.Shared.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FileUpload.Shared.Middlewares
{
    public class RequestLogger
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        public RequestLogger(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            var watch = Stopwatch.StartNew();

            string message = $"[Request] HTTP {context.Request.Method} - {context.Request.Path}";

            await _next.Invoke(context);
            _logger.Write(message);
            watch.Stop();

            message = $"[Response] HTTP {context.Request.Method} - {context.Request.Path} responded {context.Response.StatusCode} in {watch.Elapsed.TotalMilliseconds} ms";
            _logger.Write(message);
        }

    }

    public static class CustomExceptionMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomRequestLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLogger>();
        }
    }
}
