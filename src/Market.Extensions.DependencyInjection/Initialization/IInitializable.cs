using System.Threading;
using System.Threading.Tasks;

namespace Market.Extensions.DependencyInjection
{
    public interface IInitializable
    {
        Task InitializeAsync(CancellationToken cancellationToken);
    }
}
