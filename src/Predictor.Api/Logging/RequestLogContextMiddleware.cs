using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Predictor.Http;
using Predictor.Logging;
using Serilog.Context;

namespace Predictor.Api.Logging
{
    /// <summary>
    /// Middleware for pushing additional data into the Serilog log context for the request
    /// </summary>
    public class RequestLogContextMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLogContextMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public Task Invoke(HttpContext context)
        {
            var properties = new PropertyBagEnricher()
                .Add("CorrelationId", context.GetCorrelationId());

            using (LogContext.Push(properties))
            {
                return _next.Invoke(context);
            }
        }
    }
}