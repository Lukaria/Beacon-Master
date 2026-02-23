using Abilities.Configs;
using Abilities.Interfaces;
using Lighthouse.Pool;

namespace Abilities.Handlers
{
    public class BrightnessAbilityHandler : IAbilityHandler
    {
        private readonly BrightnessAbilityConfig _config;
        private readonly LighthouseService _service;

        public BrightnessAbilityHandler(BrightnessAbilityConfig config, LighthouseService service)
        {
            _config = config;
            _service = service;
        }
        
        public void ApplyEffect()
        {
            var lighthouse = _service.Lighthouse;
            var stats = lighthouse.Lightzone.Stats;
            stats.Brightness *= (1 + _config.BrightnessPercent);
            lighthouse.Lightzone.Stats = stats;
        }
    }
}