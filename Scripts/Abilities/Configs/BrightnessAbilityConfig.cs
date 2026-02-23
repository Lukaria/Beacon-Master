using System;
using Abilities.Handlers;
using Lighthouse.Lighthouses;
using UnityEngine;

namespace Abilities.Configs
{
    [CreateAssetMenu(fileName ="BrightnessAbilityConfig", menuName = "Configs/Abilities/Brightness")]
    public class BrightnessAbilityConfig : AbilityConfigBase
    {
        [SerializeField, Range(0, 1)] private float brightnessPercent;
        
        public float BrightnessPercent => brightnessPercent;
        
        public override Type HandlerType => typeof(BrightnessAbilityHandler);
    }
}