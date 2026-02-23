using System;
using Abilities.Handlers;
using Lighthouse.Lighthouses;
using Lighthouse.Pool;
using UnityEngine;
using Zenject;

namespace Abilities.Configs
{
    [CreateAssetMenu(fileName ="HealAbilityConfig", menuName = "Configs/Abilities/Heal")]
    public class HealAbilityConfig : AbilityConfigBase
    {
        [SerializeField, Range(0, 1)] private float healPercent;
        
        public float HealPercent => healPercent;
        
        public override Type HandlerType =>  typeof(HealAbilityHandler);
    }
}