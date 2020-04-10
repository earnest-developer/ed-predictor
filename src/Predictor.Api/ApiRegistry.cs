using DependencyInjection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Predictor.Api.Logging;
using Predictor.Logging;

namespace Predictor.Api
{
    public class ApiRegistry : IServiceRegistry
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingMediatorBehavior<,>));
            services.AddScoped<IDiagnosticContextAdaptor, SerilogDiagnosticContextAdaptor>();
        }
    }
}