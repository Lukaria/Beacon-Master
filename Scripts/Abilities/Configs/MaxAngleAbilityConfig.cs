using System;
using Abilities.Handlers;
using UnityEngine;

namespace Abilities.Configs
{
    [CreateAssetMenu(fileName ="MaxAngleAbilityConfig", menuName = "Configs/Abilities/MaxAngle")]
    public class MaxAngleAbilityConfig : AbilityConfigBase
    {
        [SerializeField, Range(0, 1)] private float maxAnglePercent;

        public float MaxAnglePercent => maxAnglePercent;
        
        public override Type HandlerType =>  typeof(MaxAngleAbilityHandler);
    }
}