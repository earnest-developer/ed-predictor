using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection
{
    /// <summary>
    /// Defines an interface classes containing service registrations
    /// </summary>
    public interface IServiceRegistry
    {
        /// <summary>
        /// Configure the provided service collection
        /// </summary>
        /// <param name="services">The service collection to configure</param>
        void ConfigureServices(IServiceCollection services);
    }
}