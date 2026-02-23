using Common;
using Common.Interfaces;
using UnityEngine;
using Zenject;

namespace Lighthouse.Pool
{
    public class LighthouseServiceInstaller : MonoInstaller
    {
        [SerializeField] private LighthouseService lighthouseService;

        public override void InstallBindings()
        {
            Container.Bind<LighthouseService>().FromInstance(lighthouseService).AsSingle().NonLazy();
        }
    }
}