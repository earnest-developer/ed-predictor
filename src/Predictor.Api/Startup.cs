using System;
using System.Text.Json;
using DependencyInjection;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Predictor.Api.Http;
using Predictor.Api.Logging;
using Predictor.Api.Validation;
using Serilog;

namespace Predictor.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            if (env == null) throw new ArgumentNullException(nameof(env));

            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            Log.Logger.Information("Starting Predictor API {Version}",
                ReflectionUtils.GetAssemblyVersion<Startup>());

            if (env.IsDevelopment())
                Log.Logger.Debug(Configuration.Dump());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRegistry(new ApiRegistry());

            services.AddMediatR(typeof(Startup));

            services
                .AddAuthorization()
                .AddMvcCore(options =>
                {
                    options.Filters.Add<ValidateRequestFilter>();
                    options.Filters.Add<ValidateModelStateFilter>();
                    options.AllowEmptyInputInBodyModelBinding = true;
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            var pathBase = Configuration.GetValue<string>("PathBase");
            if (!string.IsNullOrEmpty(pathBase)) app.UsePathBase(pathBase);
            
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseMiddleware<RequestLogContextMiddleware>();
            app.UseMiddleware<ResponseHeadersMiddleware>();
            app.UseSerilogRequestLogging(opts
                => opts.EnrichDiagnosticContext = LogEnricher.EnrichFromRequest);

            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}