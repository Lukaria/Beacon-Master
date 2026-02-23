#nullable enable
using Cysharp.Threading.Tasks;

namespace Persistence.Interfaces
{
    public interface IReadRepository<T>
    {
        public UniTask<T?> ReadAsync();
    }
}