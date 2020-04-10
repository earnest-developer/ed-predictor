using System;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection
{
    /// <summary>
    /// Registry extensions for <see cref="IServiceCollection"/>
    /// </summary>
    public static class RegistryServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a registry to the provided service collection
        /// </summary>
        /// <param name="services">The services to configure</param>
        /// <typeparam name="TRegistry">The type of registry to add</typeparam>
        /// <returns>The configured service collection</returns>
        public static IServiceCollection AddRegistry<TRegistry>(this IServiceCollection services) where TRegistry : IServiceRegistry, new()
        {
            return services.AddRegistry(new TRegistry());
        }

        /// <summary>
        /// Adds a registry to the provided service collection
        /// </summary>
        /// <param name="services">The services to configure</param>
        /// <param name="registry">The registry to add</param>
        /// <returns>The configured service collection</returns>
        public static IServiceCollection AddRegistry<TRegistry>(this IServiceCollection services, TRegistry registry) where TRegistry : IServiceRegistry
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            registry.ConfigureServices(services);
            return services;
        }
    }
}