using System;
using Predictor.Logging;
using Serilog;

namespace Predictor.Api.Logging
{
    /// <summary>
    /// Adaptor that uses the Serilog diagnostic context in the ASP.NET Core package
    /// </summary>
    public class SerilogDiagnosticContextAdaptor : IDiagnosticContextAdaptor
    {
        private readonly IDiagnosticContext _diagnosticContext;

        public SerilogDiagnosticContextAdaptor(IDiagnosticContext diagnosticContext)
        {
            _diagnosticContext = diagnosticContext ?? throw new ArgumentNullException(nameof(diagnosticContext));
        }
        
        public void Set(string propertyName, object value, bool destructureObjects = false)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException("Property name is required", nameof(propertyName));

            if (value is null)
                return; // Little value in having nulls in the log

            _diagnosticContext.Set(propertyName, value, destructureObjects);
        }
    }
}