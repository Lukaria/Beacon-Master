using System;
using Abilities.Handlers;
using UnityEngine;

namespace Abilities.Configs
{
    [CreateAssetMenu(fileName ="RegenerationAbilityConfig", menuName = "Configs/Abilities/Regeneration")]
    public class RegenerationAbilityConfig : AbilityConfigBase
    {
        [SerializeField, Range(0, 1)] private float regenerationPercent;

        public float RegenerationPercent => regenerationPercent;
        
        public override Type HandlerType =>  typeof(RegenerationAbilityHandler);
    }
}