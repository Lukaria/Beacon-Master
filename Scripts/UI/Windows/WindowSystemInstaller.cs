using UI.Windows.Common;
using UI.Windows.Interfaces;
using UI.Windows.Signals;
using UnityEngine;
using Zenject;

namespace UI.Windows
{
    public class WindowSystemInstaller : MonoInstaller
    {
        [SerializeField] private WindowManager windowManager;

        public override void InstallBindings()
        {
            Container.Bind<IWindowFactory>().To<WindowFactory>().AsSingle().NonLazy();
            Container.Bind<IUiNavigation>().FromInstance(windowManager).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<WindowsOrchestrator>()
                .AsSingle().NonLazy();

            BindSignals();

        }

        private void BindSignals()
        {
            Container.DeclareSignalWithInterfaces<OpenWindowSignal>();
        }
    }
}