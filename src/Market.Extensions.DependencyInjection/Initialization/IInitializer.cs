using System;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Extensions.DependencyInjection
{
    interface IInitializer
    {
        Type InitializedType { get; }
        Task InitializeAsync(CancellationToken cancellationToken);
    }
}
