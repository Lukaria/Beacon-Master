using System;
using Chargeable;
using UnityEngine;

namespace Obstacle
{
    [Serializable]
    public class ObstacleStats
    {
        public ChargeableStats chargeable;

        [Header("Obstacle")]
        public float health;
        public float speed;
        public float damage = 1.0f;
    }
}