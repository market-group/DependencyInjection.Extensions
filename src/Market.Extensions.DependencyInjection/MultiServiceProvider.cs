using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market.Extensions.DependencyInjection
{
    public class MultiServiceProvider : IServiceProvider
    {
        private IServiceProvider[] _providers;

        public MultiServiceProvider(params IServiceProvider[] providers)
        {
            if (providers == null || providers.Length <= 0) throw new ArgumentNullException(nameof(providers));

            _providers = providers;
        }
        public object GetService(Type serviceType)
        {
            foreach (var sp in _providers)
            {
                var obj = sp.GetService(serviceType);

                if (obj != null)
                    return obj;
            }

            return null;
        }
    }
}
