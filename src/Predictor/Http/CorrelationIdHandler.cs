using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Predictor.Http
{
    /// <summary>
    /// A handler that appends a Correlation-Id to all outgoing HTTP requests
    /// </summary>
    public class CorrelationIdHandler : DelegatingHandler
    {
        private const string CorrelationIdHeaderName = "Correlation-Id";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CorrelationIdHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            if (_httpContextAccessor.HttpContext != null)
            {
                string correlationId = _httpContextAccessor.HttpContext.GetCorrelationId();
                if (!string.IsNullOrEmpty(correlationId))
                    request.Headers.Add(CorrelationIdHeaderName, correlationId);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}