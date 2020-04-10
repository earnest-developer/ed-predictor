using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Predictor.Logging;
using Serilog;

namespace Predictor.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                await BuildWebHost(args).RunAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Predictor API terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IWebHost BuildWebHost(string[] args)
        {
            return new WebHostBuilder()
                .UseKestrel()
                .ConfigureKestrel(x => x.AddServerHeader = false)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true)
                        .AddJsonFile("appsettings.local.json", optional: true)
                        .AddEnvironmentVariables(prefix: "Predictor_");
                })
                .UseStartup<Startup>()
                .UseSerilog((hostContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration)
                        .EnrichWithEventType()
                        .Enrich.WithProperty("Version", ReflectionUtils.GetAssemblyVersion<Program>())
                        .Enrich.WithMachineName();
                })
                .Build();
        }
    }
}