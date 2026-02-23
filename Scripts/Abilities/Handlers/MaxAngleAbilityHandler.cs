using Abilities.Configs;
using Abilities.Interfaces;
using Lighthouse.Lighthouses;
using Lighthouse.Pool;

namespace Abilities.Handlers
{
    public class MaxAngleAbilityHandler : IAbilityHandler
    {
        private readonly MaxAngleAbilityConfig _config;
        private readonly LighthouseBase _lighthouse;

        public MaxAngleAbilityHandler(LighthouseService service, MaxAngleAbilityConfig config)
        {
            _config = config;
            _lighthouse = service.Lighthouse;
        }
        
        public void ApplyEffect()
        {
            var stats = _lighthouse.Lightzone.Stats;
            stats.MaxAngle *= (1 + _config.MaxAnglePercent);
            _lighthouse.Lightzone.Initialize(_lighthouse.gameObject.transform.position, stats);
        }
    }
}