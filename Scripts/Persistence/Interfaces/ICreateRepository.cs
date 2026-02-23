
using Cysharp.Threading.Tasks;

namespace Persistence.Interfaces
{
    public interface ICreateRepository<T>
    {
        public UniTask CreateAsync(T entity);
    } 
}
