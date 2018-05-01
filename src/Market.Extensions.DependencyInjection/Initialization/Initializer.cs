using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Extensions.DependencyInjection
{
    class Initializer<TService> : IInitializer,IDisposable
    {
        private IEnumerable<TService> _services;

        public Initializer(IEnumerable<TService> services)
        {
            _services = services;
        }

        public Type InitializedType => typeof(TService);

        public void Dispose()
        {
            _services = null;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            foreach(TService service in _services)
            {
                if(service is IInitializable initializable)
                {
                    await initializable.InitializeAsync(cancellationToken);
                }
            }
        }
    }
}
