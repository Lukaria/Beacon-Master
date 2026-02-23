namespace Common.Interfaces
{
    public interface IDeepCopy
    {
        public T DeepCopy<T>(T source);
    }
}