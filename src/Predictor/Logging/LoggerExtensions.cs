using System;
using Serilog;
using Serilog.Events;
using SerilogTimings;
using SerilogTimings.Extensions;

namespace Predictor.Logging
{
    /// <summary>
    /// Serilog Logger extensions
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Begin a new timed operation at debug level. The return value must be disposed to complete the operation.
        /// </summary>
        /// <param name="logger">The logger instance</param>
        /// <param name="messageTemplate">A log message describing the operation, in message template format.</param>
        /// <param name="args">
        /// Arguments to the log message. These will be stored and captured only when the
        /// operation completes, so do not pass arguments that are mutated during the operation.
        /// </param>
        /// <returns>A <see cref="SerilogTimings.Operation" /> object.</returns>
        public static IDisposable TimeDebug(this ILogger logger, string messageTemplate, params object[] args)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            return logger.OperationAt(LogEventLevel.Debug).Time(messageTemplate, args);
        }

        /// <summary>
        /// Begin a new timed operation. The return value must be completed using SerilogTimings.Operation.Complete,
        /// or disposed to record abandonment.
        /// </summary>
        /// <param name="logger">The logger instance</param>
        /// <param name="messageTemplate">A log message describing the operation, in message template format.</param>
        /// <param name="args">
        /// Arguments to the log message. These will be stored and captured only when the
        /// operation completes, so do not pass arguments that are mutated during the operation.
        /// </param>
        /// <returns>A <see cref="SerilogTimings.Operation" /> object.</returns>
        public static Operation BeginDebug(this ILogger logger, string messageTemplate, params object[] args)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            return logger.OperationAt(LogEventLevel.Debug).Begin(messageTemplate, args);
        }
    }
}