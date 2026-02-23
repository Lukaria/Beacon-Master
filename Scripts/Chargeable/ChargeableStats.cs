using System;
using UnityEngine;

namespace Chargeable
{
    [Serializable]
    public class ChargeableStats
    {
        public bool accumulativeTrigger;
        public bool accumulativeUpdate;
        public float timeToCharge;
        public float experience;
        public float points;
    }
}