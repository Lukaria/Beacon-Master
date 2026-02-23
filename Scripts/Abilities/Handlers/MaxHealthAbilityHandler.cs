using Abilities.Configs;
using Abilities.Interfaces;
using Common.Interfaces;
using Lighthouse.Lighthouses;
using Lighthouse.Pool;

namespace Abilities.Handlers
{
    public class MaxHealthAbilityHandler : IAbilityHandler
    {
        private readonly IPlayerHealth _playerHealth;
        private readonly MaxHealthAbilityConfig _config;

        public MaxHealthAbilityHandler(IPlayerHealth playerHealth, MaxHealthAbilityConfig config)
        {
            _playerHealth = playerHealth;
            _config = config;
        }
        
        public void ApplyEffect()
        {
            var value = _playerHealth.MaxHealth.CurrentValue * _config.MaxHealthPercent;
            _playerHealth.IncreaseMaxHealth(value);
            _playerHealth.Heal(value);
        }
    }
}