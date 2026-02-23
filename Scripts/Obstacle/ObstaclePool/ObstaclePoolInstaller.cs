using UnityEngine;
using Zenject;

namespace Obstacle.ObstaclePool
{
    public class ObstaclePoolInstaller : MonoInstaller
    {
        [SerializeField] private ObstaclePool obstaclePool;

        public override void InstallBindings()
        {
            Container.Bind<ObstaclePool>().FromInstance(obstaclePool).AsSingle().NonLazy();
        }
    }
}