using System;
using Chargeable;
using Common;
using CustomEditor;
using UnityEngine;

namespace Obstacle.Obstacles
{
    public class ObstacleBase : ChargeableBase
    {
        [SerializeField, Readonly] protected ObstacleStats obstacleStats; 
        public Action<ObstacleBase> ObstacleDestroyed;
        private readonly HealthComponent _health = new();

        public override void Awake()
        {
            base.Awake();
            chargeStats = obstacleStats.chargeable;
        }

        public void SetStats(ObstacleStats stats)
        {
            obstacleStats = stats;
            _health.Set(stats.health);
            chargeStats = stats.chargeable;
        }
        
        protected override void OnCharged()
        {
            throw new System.NotImplementedException();
        }

        public void DestroyObstacle()
        {
            ReleaseWheel();
            ObstacleDestroyed?.Invoke(this);
        }
    }
}