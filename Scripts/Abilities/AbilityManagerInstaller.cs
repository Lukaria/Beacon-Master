using Abilities.Interfaces;
using Abilities.Signals;
using DefaultNamespace;
using UnityEngine;
using Zenject;
 
namespace Abilities
{
    public class AbilityManagerInstaller : MonoInstaller
    {
        [SerializeField] private AbilityManager _abilityManager;

        public override void InstallBindings()
        {
            Container.Bind<IAbilityContainer>().To<AbilityContainer>().AsSingle().NonLazy();
            Container.Bind<AbilityManager>().FromInstance(_abilityManager).AsSingle().NonLazy();
            Container.Bind<IAbilityHandlerFactory>().To<AbilityHandlerFactory>().AsCached().NonLazy();

            BindSignals();
        }

        private void BindSignals()
        {
            Container.DeclareSignal<AbilityContainerUpdatedSignal>();
            Container.DeclareSignalWithInterfaces<LevelUpSignal>();
        }
    }
}