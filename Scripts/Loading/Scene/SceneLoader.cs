using Cysharp.Threading.Tasks;
using UI.Windows.Loading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Loading.Scene
{
    public class SceneLoader : ISceneLoader
    {
        private readonly LoadingScreenWindow _loadingWindow;

        public SceneLoader(DiContainer container, LoadingScreenWindow loadingWindow)
        {
            _loadingWindow = container.InstantiatePrefabForComponent<LoadingScreenWindow>(loadingWindow);
        }
        
        public async UniTask LoadSceneAsync(string sceneName)
        {
            Debug.Log($"Loading scene {sceneName}");
            await _loadingWindow.ShowAsync();
            var loadHandle  = await Addressables.LoadSceneAsync(sceneName);
            loadHandle.ActivateAsync();
        }
        
        public async UniTaskVoid HideLoadingScreen()
        {
            await _loadingWindow.HideAsync();
        }
    }
}