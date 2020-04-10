using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Predictor.Http
{
    /// <summary>
    /// Correlation Extensions for HttpContext
    /// </summary>
    public static class TracingHttpContextExtensions
    {
        /// <summary>
        /// Gets the correlation identifier for the request
        /// </summary>
        /// <param name="httpContext">The HTTP Context</param>
        /// <returns>The Correlation-Id HTTP request header if present, otherwise <see cref="HttpContext.TraceIdentifier"/></returns>
        public static string GetCorrelationId(this HttpContext httpContext)
        {
            httpContext.Request.Headers.TryGetValue("Correlation-Id", out StringValues correlationId);
            return correlationId.FirstOrDefault() ?? httpContext.TraceIdentifier;
        }
    }
}