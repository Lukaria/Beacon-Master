using Cysharp.Threading.Tasks;

namespace Loading
{
    public interface ISceneLoader
    {
        public UniTask LoadSceneAsync(string sceneName);
        public UniTaskVoid HideLoadingScreen();
    }
}