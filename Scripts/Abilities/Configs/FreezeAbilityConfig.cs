using System;
using Abilities.Handlers;
using UnityEngine;

namespace Abilities.Configs
{
    [CreateAssetMenu(fileName ="FreezeAbilityConfig", menuName = "Configs/Abilities/Freeze")]
    public class FreezeAbilityConfig : AbilityConfigBase
    {
        [SerializeField, Range(0, 1)] private float freezePercent;

        public float FreezePercent => freezePercent;
        
        public override Type HandlerType =>  typeof(FreezeAbilityHandler);
    }
}