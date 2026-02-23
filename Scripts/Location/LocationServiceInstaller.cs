using UnityEngine;
using Zenject;

namespace Location
{
    public class LocationServiceInstaller : MonoInstaller
    {
        [SerializeField] private LocationService locationService;

        public override void InstallBindings()
        {
            Container.Bind<LocationService>().FromInstance(locationService).AsSingle().NonLazy();
        }
    }
}