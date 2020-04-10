using System;
using Serilog;

namespace Predictor.Logging
{
    public static class LoggerConfigurationExtensions
    {
        /// <summary>
        /// Enrich each log with an EventType
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration</param>
        /// <returns>The enriched logger configuration</returns>
        public static LoggerConfiguration EnrichWithEventType(this LoggerConfiguration loggerConfiguration)
        {
            if (loggerConfiguration == null)
                throw new ArgumentNullException(nameof(loggerConfiguration));

            return loggerConfiguration.Enrich.With(new EventTypeEnricher());
        }
    }
}