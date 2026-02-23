using UnityEngine;
using Zenject;

namespace Cash
{
    public class CashScreenUIManagerInstaller : MonoInstaller
    {
        [SerializeField] private CashScreenUIManager manager;

        public override void InstallBindings()
        {
            Container.Bind<CashScreenUIManager>().FromInstance(manager).AsSingle().NonLazy();
        }
    }
}