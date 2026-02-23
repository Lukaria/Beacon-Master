using Cysharp.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IDataService<T>
    {
        public T GetData();
        public UniTask LoadAsync();
        public UniTask SaveAsync();
    }
}