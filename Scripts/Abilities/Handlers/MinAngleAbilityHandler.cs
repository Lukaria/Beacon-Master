using Abilities.Configs;
using Abilities.Interfaces;
using Lighthouse.Lighthouses;
using Lighthouse.Pool;

namespace Abilities.Handlers
{
    public class MinAngleAbilityHandler : IAbilityHandler
    {
        private readonly MinAngleAbilityConfig _config;
        private readonly LighthouseBase _lighthouse;

        public MinAngleAbilityHandler(LighthouseService service, MinAngleAbilityConfig config)
        {
            _config = config;
            _lighthouse = service.Lighthouse;
        }

        
        public void ApplyEffect()
        {
            var stats = _lighthouse.Lightzone.Stats;
            stats.MinAngle *= (1 - _config.MinAnglePercent);
            _lighthouse.Lightzone.Initialize(_lighthouse.gameObject.transform.position, stats);
        }
    }
}