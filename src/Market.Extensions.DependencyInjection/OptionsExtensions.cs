using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Market.Extensions.DependencyInjection
{
    public static class OptionsExtensions
    {
        private static IServiceCollection Add<TService, TImplementation, TOptions>(IServiceCollection services,ServiceLifetime serviceLifetime, Action<TOptions> configure)
                        where TService : class
                        where TOptions : class, new()
                        where TImplementation : class, TService
        {

            if (configure != null)
                services.Configure(configure);

            services.Add(new ServiceDescriptor(typeof(TService), (sp) =>
              {
                  TOptions options = sp.GetRequiredService<IOptionsSnapshot<TOptions>>().Value;
                  return ActivatorUtilities.CreateInstance<TImplementation>(sp, options);
              }, serviceLifetime));

            return services;
        }


        private static IServiceCollection AddScoped<TService, TImplementation, TOptions>(this IServiceCollection services,Action<TOptions> configure)
                        where TService : class
                        where TOptions : class, new()
                        where TImplementation : class, TService
        {

            return Add<TService,TImplementation,TOptions>(services, ServiceLifetime.Scoped, configure);
        }

        private static IServiceCollection AddScoped<TService, TImplementation, TOptions>(this IServiceCollection services)
                        where TService : class
                        where TOptions : class, new()
                        where TImplementation : class, TService
        {

            return Add<TService, TImplementation, TOptions>(services, ServiceLifetime.Scoped, null);
        }


        private static IServiceCollection AddSingleton<TService, TImplementation, TOptions>(this IServiceCollection services, Action<TOptions> configure)
                        where TService : class
                        where TOptions : class, new()
                        where TImplementation : class, TService
        {

            return Add<TService, TImplementation, TOptions>(services, ServiceLifetime.Singleton, configure);
        }

        private static IServiceCollection AddSingleton<TService, TImplementation, TOptions>(this IServiceCollection services)
                        where TService : class
                        where TOptions : class, new()
                        where TImplementation : class, TService
        {

            return Add<TService, TImplementation, TOptions>(services, ServiceLifetime.Singleton, null);
        }


        private static IServiceCollection AddTransient<TService, TImplementation, TOptions>(this IServiceCollection services, Action<TOptions> configure)
                where TService : class
                where TOptions : class, new()
                where TImplementation : class, TService
        {

            return Add<TService, TImplementation, TOptions>(services, ServiceLifetime.Transient, configure);
        }

        private static IServiceCollection AddTransient<TService, TImplementation, TOptions>(this IServiceCollection services)
                        where TService : class
                        where TOptions : class, new()
                        where TImplementation : class, TService
        {

            return Add<TService, TImplementation, TOptions>(services, ServiceLifetime.Transient, null);
        }

    }
}