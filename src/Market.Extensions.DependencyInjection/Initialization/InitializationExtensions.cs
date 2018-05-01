using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Extensions.DependencyInjection
{
    public static class InitializationExtensions
    {
        public static IServiceCollection AddSingletonWithInit(this IServiceCollection services, Type serviceType)
        {
            return services.AddSingleton(serviceType)
                .AddSingleton(typeof(IInitializer), GetGenericType(serviceType));
        }

        public static IServiceCollection AddSingletonWithInit(this IServiceCollection services, Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            return services.AddSingleton(serviceType, implementationFactory)
                           .AddSingleton(typeof(IInitializer), GetGenericType(serviceType));
        }

        public static IServiceCollection AddSingletonWithInit(this IServiceCollection services, Type serviceType, Type implementationType)
        {
            return services.AddSingleton(serviceType,implementationType)
                        .AddSingleton(typeof(IInitializer), GetGenericType(serviceType));
        }

        public static IServiceCollection AddSingletonWithInit(this IServiceCollection services, Type serviceType, object implementationInstance)
        {
            
            return services.AddSingleton(serviceType,implementationInstance)
                        .AddSingleton(typeof(IInitializer), GetGenericType(serviceType));
        }

        public static IServiceCollection AddSingletonWithInit<TService>(this IServiceCollection services) where TService : class
        {
            return services.AddSingleton<TService>()
                           .AddSingleton<IInitializer, Initializer<TService>>();
        }

        public static IServiceCollection AddSingletonWithInit<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory) 
                                        where TService : class
        {
            return services.AddSingleton(implementationFactory)
                           .AddSingleton<IInitializer, Initializer<TService>>();
        }
        
        public static IServiceCollection AddSingletonWithInit<TService>(this IServiceCollection services, TService implementationInstance) where TService : class
        {
            return services.AddSingleton<TService>(implementationInstance)
                           .AddSingleton<IInitializer, Initializer<TService>>();

        }
        
        public static IServiceCollection AddSingletonWithInit<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {

            return services.AddSingleton<TService,TImplementation>()
                           .AddSingleton<IInitializer, Initializer<TService>>();
        }

        public static IServiceCollection AddSingletonWithInit<TService, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory)
            where TService : class
            where TImplementation : class, TService
        {
            return services.AddSingleton<TService,TImplementation>(implementationFactory)
                           .AddSingleton<IInitializer, Initializer<TService>>();
        }

        public static IServiceCollection AddSingletonWithInit<TService, TImplementation>(this IServiceCollection services, params object[] parameters)
            where TService : class
            where TImplementation : class, TService
        {
            return services.AddSingleton<TService, TImplementation>(parameters)
                           .AddSingleton<IInitializer, Initializer<TService>>();
        }


        public static void TryAddSingletonWithInit(this IServiceCollection collection,Type service)
        {
            collection.TryAddSingleton(service);
            collection.TryAddSingleton(typeof(IInitializer), GetGenericType(service));
        }

        public static void TryAddSingletonWithInit(this IServiceCollection collection,Type service,Type implementationType)
        {
            collection.TryAddSingleton(service, implementationType);
            collection.TryAddSingleton(typeof(IInitializer), GetGenericType(service));
        }

        public static void TryAddSingletonWithInit(this IServiceCollection collection,Type service,Func<IServiceProvider, object> implementationFactory)
        {
            collection.TryAddSingleton(service, implementationFactory);
            collection.TryAddSingleton(typeof(IInitializer), GetGenericType(service));
        }

        public static void TryAddSingletonWithInit<TService>(this IServiceCollection collection)where TService : class
        {
            collection.TryAddSingleton<TService>();
            collection.TryAddSingleton(typeof(IInitializer), GetGenericType(typeof(TService)));
        }

        public static void TryAddSingletonWithInit<TService, TImplementation>(this IServiceCollection collection)
                where TService : class
                where TImplementation : class, TService
        {
            collection.TryAddSingleton<TService, TImplementation>();
            collection.TryAddSingleton(typeof(IInitializer), GetGenericType(typeof(TService)));
        }

        public static void TryAddSingletonWithInit<TService>(this IServiceCollection collection, TService instance)
                        where TService : class
        {
            collection.TryAddSingleton<TService>(instance);
            collection.TryAddSingleton(typeof(IInitializer), GetGenericType(typeof(TService)));
        }

        public static void TryAddSingletonWithInit<TService>(this IServiceCollection services,Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            services.TryAddSingleton<TService>(implementationFactory);
            services.TryAddSingleton(typeof(IInitializer), GetGenericType(typeof(TService)));
        }

        private static Type GetGenericType(Type argumentType)
        {
            return typeof(Initializer<>).MakeGenericType(argumentType);
        }

        public static void Init(this IServiceProvider serviceProvider)
        {
            serviceProvider.InitAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public static async Task InitAsync(this IServiceProvider serviceProvider,CancellationToken cancellationToken = default)
        {
            var inits = serviceProvider.GetServices<IInitializer>();

            if (inits == null || inits.Count() <= 0)
            {
                return;
            }

            foreach (var init in inits)
            {
                await init.InitializeAsync(cancellationToken);
                Debug.WriteLine("Initialized type " + init.InitializedType.Name);
            }
        }
    }
}
