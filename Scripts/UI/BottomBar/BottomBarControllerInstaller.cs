using UnityEngine;
using Zenject;

namespace UI.BottomBar
{
    //todo remove
    public class BottomBarControllerInstaller : MonoInstaller
    {
        [SerializeField] private BottomBarController controller;

        public override void InstallBindings()
        {
            Container.Bind<BottomBarController>().FromInstance(controller).AsSingle().NonLazy();
        }
    }
}