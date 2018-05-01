using Microsoft.Extensions.DependencyInjection;

namespace Market.Extensions.DependencyInjection
{

    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddSingleton<TService,TImplementation>(this IServiceCollection serviceCollection,params object[] parameters)
            where TService : class
            where TImplementation : class, TService
        {
            return serviceCollection.AddSingleton(typeof(TService), (sp) =>
             {
                 return ActivatorUtilities.CreateInstance<TImplementation>(sp, parameters);
             });
        }

        public static IServiceCollection Clone(this IServiceCollection servicesCollection)
        {
            var forked = new ServiceCollection();

            for (int i = 0; i < servicesCollection.Count; i++)
            {
                forked.Insert(i, servicesCollection[i]);
            }

            return forked;
        } 
    }
}