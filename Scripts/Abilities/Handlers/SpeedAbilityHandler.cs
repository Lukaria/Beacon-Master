using Abilities.Configs;
using Abilities.Interfaces;
using Lighthouse.Pool;

namespace Abilities.Handlers
{
    public class SpeedAbilityHandler : IAbilityHandler
    {
        private readonly SpeedAbilityConfig _config;
        private readonly LighthouseService _service;

        public SpeedAbilityHandler(SpeedAbilityConfig config, LighthouseService service)
        {
            _config = config;
            _service = service;
        }
        
        public void ApplyEffect()
        {
            var lighthouse = _service.Lighthouse;
            var stats = lighthouse.Lightzone.Stats;
            stats.Speed *= (1 + _config.SpeedPercent);
            lighthouse.Lightzone.Stats = stats;
        }
    }
}