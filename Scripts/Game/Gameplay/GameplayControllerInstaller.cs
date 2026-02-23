using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public class GameplayControllerInstaller : MonoInstaller
    {
        [SerializeField] private GameplayController gameplayController;

        public override void InstallBindings()
        {
            Container.Bind<GameplayController>().FromInstance(gameplayController).AsSingle().NonLazy();
        }
    }
}