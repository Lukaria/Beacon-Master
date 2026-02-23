using Loading.Signals;
using UnityEngine;
using Zenject;

namespace Loading
{
    public class BootstrapInstaller  : MonoInstaller
    {
        [SerializeField] private Bootstrap bootstrap;

        public override void InstallBindings()
        {
            Container.Bind<Bootstrap>().FromInstance(bootstrap).AsSingle().NonLazy();

            Container.DeclareSignal<GameLoadedSignal>();
        }
    }
}