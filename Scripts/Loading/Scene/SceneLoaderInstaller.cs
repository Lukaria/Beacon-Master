using UI.Windows.Loading;
using UnityEngine;
using Zenject;

namespace Loading.Scene
{
    public class SceneLoaderInstaller : MonoInstaller
    {
        [SerializeField] private LoadingScreenWindow loadingCurtainPrefab;
        
        public override void InstallBindings()
        {
            Container.Bind<ISceneLoader>()
                .To<SceneLoader>()
                .AsSingle()
                .WithArguments(Container,loadingCurtainPrefab)
                .NonLazy();
        }
    }
}