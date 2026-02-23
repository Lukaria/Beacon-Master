using Abilities.Configs;

namespace Abilities.Interfaces
{
    public interface IAbilityHandlerFactory
    {
        public IAbilityHandler Create(AbilityConfigBase abilityConfig);
    }
}