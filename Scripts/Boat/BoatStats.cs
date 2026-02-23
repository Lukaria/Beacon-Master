using System;
using Chargeable;
using UnityEngine;

namespace Boat
{
    [Serializable]
    public struct BoatStats
    {
        public ChargeableStats chargeable;

        public float damage;
        public float health;
        public float speed;
        public int cash;
    }
}