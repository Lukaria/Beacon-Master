using UnityEngine;
using Zenject;

namespace Boat.Pool
{
    public class BoatPoolInstaller : MonoInstaller
    {
        [SerializeField] private BoatPool boatPool;

        public override void InstallBindings()
        {
            Container.Bind<BoatPool>().FromInstance(boatPool).AsSingle().NonLazy();
        }
    }
}