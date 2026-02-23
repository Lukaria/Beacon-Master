using Abilities.Configs;
using Abilities.Interfaces;
using Common.Interfaces;
namespace Abilities.Handlers
{
    public class HealAbilityHandler : IAbilityHandler
    {
        private readonly IPlayerHealth _playerHealth;
        private readonly HealAbilityConfig _config;

        public HealAbilityHandler(IPlayerHealth playerHealth, HealAbilityConfig config)
        {
            _playerHealth = playerHealth;
            _config = config;
        }
        
        public void ApplyEffect()
        {
            var value = _playerHealth.MaxHealth.CurrentValue * _config.HealPercent;
            _playerHealth.Heal(value);
        }
    }
}