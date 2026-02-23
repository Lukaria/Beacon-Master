using Common.Interfaces;
using Lighthouse.Lighthouses;

namespace Abilities.Interfaces
{
    public interface IAbilityHandler
    {
        public void ApplyEffect(){}
        
        public void PerformUpdate(){}
        
        public void DiscardEffect(){}
    }
}