using System.Diagnostics.CodeAnalysis;

namespace Damaris.Kafka.Consumer
{
    // We can safely exclude from coverage since this is basically copy-paste code of another library. 
    [ExcludeFromCodeCoverage]
    public static class NamedServiceCollection
    {
        private static Dictionary<Type, Dictionary<string, Func<IServiceProvider, object>>> TypesMapping { get; }
          = new Dictionary<Type, Dictionary<string, Func<IServiceProvider, object>>>();

        public static void AddService<TService, TImplementation>(string name) where TService : class where TImplementation : class, TService
        {
            AddServiceDescriptor(typeof(TService), typeof(TImplementation), name);
        }

        public static void AddService(Type serviceType, Type implementation, string name)
        {
            AddServiceDescriptor(serviceType, implementation, name);
        }

        private static void AddServiceDescriptor(Type serviceType, Type implementation, string name)
        {
            if (TypesMapping.TryGetValue(serviceType, out var dict))
            {
                if (!dict.TryAdd(name, sp => sp.GetRequiredService(implementation)))
                {
                    throw new ArgumentException($"The Named Service {name} already exists for the type {serviceType.Name}");
                }

                return;
            }

            dict = new Dictionary<string, Func<IServiceProvider, object>>();
            dict.TryAdd(name, sp => sp.GetRequiredService(implementation));
            TypesMapping.Add(serviceType, dict);
        }

        public static TService GetService<TService>(string name, IServiceProvider serviceProvider) where TService : class
        {
            if (TypesMapping.TryGetValue(typeof(TService), out var dict))
            {
                if (dict.TryGetValue(name, out Func<IServiceProvider, object> service))
                {
                    return (TService)service(serviceProvider);
                }
            }

            throw new TypeAccessException($"No service named '{name}' for the type {typeof(TService)} was found.");
        }

        /// <summary>
        /// Extension to have many implementations being resolved by name convention
        /// </summary>
        /// <typeparam name="TService">Interface</typeparam>
        /// <typeparam name="TImplementation">Class that implements the TService</typeparam>
        /// <param name="services"></param>    
        /// <param name="name">Service name</param>
        /// <param name="serviceLifetime">Service life cycle</param>
        /// <returns></returns>
        public static IServiceCollection AddNamedService<TService, TImplementation>(this IServiceCollection services, string name, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
          where TService : class where TImplementation : class, TService
        {
            return services.AddNamedService<TService>(typeof(TImplementation), name, serviceLifetime);
        }

        /// <summary>
        /// Extension to have many implementations being resolved by name convention
        /// </summary>
        /// <typeparam name="TService">Interface</typeparam>    
        /// <param name="services"></param>    
        /// <param name="implementation">Class that implements the TService</param>
        /// <param name="name">Service name</param>
        /// <param name="serviceLifetime">Service life cycle</param>
        /// <returns></returns>
        public static IServiceCollection AddNamedService<TService>(
          this IServiceCollection services,
          Type implementation,
          string name,
          ServiceLifetime serviceLifetime = ServiceLifetime.Transient) where TService : class
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Transient:
                    {
                        services.AddTransient(implementation);
                        break;
                    }
                case ServiceLifetime.Singleton:
                    {
                        services.AddSingleton(implementation);
                        break;
                    }
                case ServiceLifetime.Scoped:
                    {
                        services.AddScoped(implementation);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, "Invalid Service Lifetime.");
            }

            // Registering how the service will be retrieved.
            AddService(typeof(TService), implementation, name);
            services.AddSingleton<INamedServiceResolver<TService>>(sp => serviceName => GetService<TService>(serviceName, sp));

            return services;
        }
    }

    public delegate TResult INamedServiceResolver<TResult>(dynamic serviceName) where TResult : class;
}
