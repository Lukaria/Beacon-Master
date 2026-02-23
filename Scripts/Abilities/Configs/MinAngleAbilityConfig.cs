using System;
using Abilities.Handlers;
using UnityEngine;

namespace Abilities.Configs
{
    [CreateAssetMenu(fileName ="MinAngleAbilityConfig", menuName = "Configs/Abilities/MinAngle")]
    public class MinAngleAbilityConfig : AbilityConfigBase
    {
        [SerializeField, Range(0, 1)] private float minAnglePercent;

        public float MinAnglePercent => minAnglePercent;
        
        public override Type HandlerType =>  typeof(MinAngleAbilityHandler);
    }
}