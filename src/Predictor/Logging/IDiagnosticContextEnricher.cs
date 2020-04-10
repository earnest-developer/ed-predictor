namespace Predictor.Logging
{
    /// <summary>
    /// Defines an interface for types that can enrich the serilog diagnostic context (completion event)
    /// </summary>
    public interface IDiagnosticContextEnricher
    {
        /// <summary>
        /// Enriches the provides diagnostic context
        /// </summary>
        /// <param name="diagnosticContext">The diagnostic context to enrich</param>
        void Enrich(IDiagnosticContextAdaptor diagnosticContext);
    }
}