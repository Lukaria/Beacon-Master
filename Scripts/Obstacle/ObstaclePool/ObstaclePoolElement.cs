using System;
using Obstacle.Obstacles;

namespace Obstacle.ObstaclePool
{
    [Serializable]
    public class ObstaclePoolElement
    {
        public ObstacleBase obstacle;
        public ObstacleStats stats;
    }
}