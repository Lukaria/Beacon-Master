using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Common.Extensions;
using Common.Interfaces;
using Obstacle.Obstacles;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Obstacle.ObstaclePool
{
    public class ObstaclePool : MonoBehaviour, IUpdateable
    {
        [SerializeField] private List<Transform> spawnPoints = new();
        [SerializeField, Range(0f, 10f)] private float minSpawnTime;
        
        //private ReadOnlyCollection<Vector3> spawnPoints = new(new List<Vector3>());
        
        private List<ObstaclePoolElement> _obstaclePrefabs;
        
        private int _obstaclesLimit;
        private readonly Dictionary<ObstacleBase, Transform> _usedSpawnPositions = new();
        private readonly List<Transform> _unusedSpawnPositions = new();
        
        private List<ObstacleBase> _activePool = new();

        private float _nextObstacleTimer;
        private float _maxSpawnTimer;
        private DiContainer _container;

        [Inject]
        private void Construct(DiContainer container)
        {
            _container = container;
            _nextObstacleTimer = minSpawnTime;
        }
        

        public void SetObstaclePrefabs(List<ObstaclePoolElement> obstacles) => _obstaclePrefabs = obstacles;
        public void SetObstaclesLimit(int limit) => _obstaclesLimit = limit;
        public void SetSpawnPoints(ReadOnlyCollection<Vector3> points){
            //spawnPoints = points;
            foreach (var spawnPoint in spawnPoints)
            {
                _unusedSpawnPositions.Add(spawnPoint);
            }
        }
        public void SetMaxObstacleTimer(float max) => _maxSpawnTimer = max;

        public bool TrySpawnRandomObstacle()
        {
            if (_usedSpawnPositions.Count >= _obstaclesLimit) return false;

            var obstaclePoolElement = _obstaclePrefabs.GetRandomElement();
            var randomPosition = _unusedSpawnPositions.GetRandomElement();
            
            var obstacle = _container.InstantiatePrefabForComponent<ObstacleBase>(
                obstaclePoolElement.obstacle, randomPosition);
            obstacle.SetStats(obstaclePoolElement.stats);
            obstacle.ResetCharge();
            _unusedSpawnPositions.Remove(randomPosition);
            _usedSpawnPositions.Add(obstacle,  randomPosition);
            obstacle.ObstacleDestroyed += OnObstacleDestroyed;
            _activePool.Add(obstacle);
            return true;
        }

        private void OnObstacleDestroyed(ObstacleBase obstacle)
        {
            _activePool.Remove(obstacle);
            obstacle.gameObject.SetActive(false);
            if (_usedSpawnPositions.Remove(obstacle, out var spawnPosition))
            {
                _unusedSpawnPositions.Add(spawnPosition);
                obstacle.ResetCharge();
            }
            else
            {
                Utils.Assertions.Assert("no such obstacle in usedSpawnPoints!");
            }
        }

        public void PerformUpdate(float dt)
        {
            PerformActiveObstaclesUpdate(dt);
            
            if (_usedSpawnPositions.Count >= _obstaclesLimit) return;

            _nextObstacleTimer -= dt;
            if (_nextObstacleTimer > 0f) return;
            
            _nextObstacleTimer = Random.Range(minSpawnTime, _maxSpawnTimer);
            TrySpawnRandomObstacle();
        }

        private void PerformActiveObstaclesUpdate(float dt)
        {
            foreach (var obstacle in _activePool)
            {
                obstacle.PerformUpdate(dt);
            }
        }

        public void Clear()
        {
            foreach (var obstacleBase in _activePool.ToList())
            {
                obstacleBase.DestroyObstacle();
            }
        }

        private void OnDrawGizmosSelected()
        {
            foreach (var point in spawnPoints)
            {
                Gizmos.color = Color.brown;
                Gizmos.DrawSphere(point.position, 0.2f);
            }
        }
    }
}