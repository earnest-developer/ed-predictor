using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Predictor.Api
{
    /// <summary>
    /// Catch all exception handler that returns HTTP status code 500 in response to any exception
    /// </summary>
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception)
            {
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }
}