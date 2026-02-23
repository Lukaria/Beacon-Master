using UnityEngine;
using Zenject;

namespace VFX
{
    public class ExplosionManagerInstaller : MonoInstaller
    {
        [SerializeField] private ExplosionManager explosionManager;

        public override void InstallBindings()
        {
            Container.Bind<ExplosionManager>().FromInstance(explosionManager).AsSingle().NonLazy();
        }
    }
}