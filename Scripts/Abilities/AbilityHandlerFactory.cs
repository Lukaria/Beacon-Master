using Abilities.Configs;
using Abilities.Interfaces;
using Zenject;

namespace Abilities
{
    public class AbilityHandlerFactory : IAbilityHandlerFactory 
    {
        private IInstantiator _instantiator;

        [Inject]
        public void Construct(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }
        
        public IAbilityHandler Create(AbilityConfigBase abilityConfig)
        {
            return (IAbilityHandler)_instantiator.Instantiate(abilityConfig.HandlerType,
                new object[] { abilityConfig });
        }
    }
}