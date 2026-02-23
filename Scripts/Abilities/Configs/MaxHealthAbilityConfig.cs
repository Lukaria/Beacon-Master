using System;
using Abilities.Handlers;
using Lighthouse.Lighthouses;
using Lighthouse.Pool;
using UnityEngine;
using Zenject;

namespace Abilities.Configs
{
    [CreateAssetMenu(fileName ="MaxHealthAbilityConfig", menuName = "Configs/Abilities/MaxHealth")]
    public class MaxHealthAbilityConfig : AbilityConfigBase
    {
        [SerializeField, Range(0, 1)] private float maxHealthPercent;

        public float MaxHealthPercent => maxHealthPercent;
        
        public override Type HandlerType =>  typeof(MaxHealthAbilityHandler);
    }
}