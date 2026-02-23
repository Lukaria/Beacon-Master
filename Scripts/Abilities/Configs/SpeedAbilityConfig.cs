using System;
using Abilities.Handlers;
using UnityEngine;

namespace Abilities.Configs
{
    [CreateAssetMenu(fileName ="SpeedAbilityConfig", menuName = "Configs/Abilities/Speed")]
    public class SpeedAbilityConfig : AbilityConfigBase
    {
        [SerializeField, Range(0, 1)] private float speedPercent;
        
        public float SpeedPercent => speedPercent;
        
        public override Type HandlerType =>  typeof(SpeedAbilityHandler);
    }
}