using System.Collections.Generic;
using Boat.Pool;
using Obstacle.ObstaclePool;
using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName ="LevelConfig", menuName = "Configs/LevelConfig")]
    public class LevelConfig : ScriptableObject
    { 
        [Header("Boats")]
        [SerializeField] 
        private List<BoatPoolElement> boats;
        
        [SerializeField, Range(1f, 10), Tooltip("Divisor for MaxBoatSpawnTimer value")] 
        private int boatsSpawnRate = 1;
        
        [SerializeField, Range(1f, 100f)] 
        private float speedScaler = 1;
        
        [SerializeField, Range(1f, 100f), Tooltip("Max active boats on the screen")] 
        private int boatsSpawnLimit = 1;

        [Header("Obstacles")]
        [SerializeField] 
        private List<ObstaclePoolElement> obstacles;
        
        [SerializeField, Range(1f, 10), Tooltip("Divisor for MaxObstacleSpawnTimer value")] 
        private int obstaclesSpawnRate = 1;
        
        [SerializeField, Range(0f, 10), Tooltip("Max active obstacles on the screen")] 
        private int obstaclesSpawnLimit;
        
        
    
        public List<BoatPoolElement> Boats => boats;
        public List<ObstaclePoolElement> Obstacles => obstacles;
        public float SpeedScaler => speedScaler;
        
        public int BoatsSpawnLimit => boatsSpawnLimit;

        public int BoatsSpawnRate => boatsSpawnRate;
        public int ObstaclesSpawnRate => obstaclesSpawnRate;
        public int ObstaclesSpawnLimit => obstaclesSpawnLimit;

        private void OnValidate()
        {
            if (Boats.Count == 0) Utils.Assertions.Assert("Config's boats pool is empty!");
        }
    }
}