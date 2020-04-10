using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OneOf;
using Serilog;

namespace Predictor.Logging
{
    /// <summary>
    /// Mediator behavior that logs the message type and time to be handled and 
    /// pushes contextual import data contained in mediator request and responses into the Serilog Diagnostics context
    /// </summary>
    /// <typeparam name="TRequest">The pipeline request type</typeparam>
    /// <typeparam name="TResponse">The pipeline response type</typeparam>
    public class LoggingMediatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger;
        private readonly IDiagnosticContextAdaptor _diagnosticContext;

        public LoggingMediatorBehavior(ILogger logger, IDiagnosticContextAdaptor diagnosticContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _diagnosticContext = diagnosticContext ?? throw new ArgumentNullException(nameof(diagnosticContext));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            using (_logger.TimeDebug("Handling {MessageType} with Mediator", typeof(TRequest).GetDisplayName()))
            {
                if (request is IDiagnosticContextEnricher requestEnricher)
                {
                    requestEnricher.Enrich(_diagnosticContext);
                }

                TResponse response = await next();

                if (response is IDiagnosticContextEnricher responseEnricher)
                {
                    responseEnricher.Enrich(_diagnosticContext);
                }
                else if (response is IOneOf oneOfResult && oneOfResult.Value is IDiagnosticContextEnricher oneOfEnricher)
                {
                    oneOfEnricher.Enrich(_diagnosticContext);
                }

                return response;
            }
        }
    }
}