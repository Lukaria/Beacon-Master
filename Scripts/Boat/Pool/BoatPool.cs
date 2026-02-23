using System;
using System.Collections.Generic;
using System.Linq;
using Boat.Boats;
using Common.Extensions;
using Common.Interfaces;
using ObjectPath;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Boat.Pool
{
    public class BoatPool : MonoBehaviour, IUpdateable
    {
        [SerializeField] private int initialPoolSize = 20;
        [SerializeField] private List<Transform> boatSpawnPoints;
        [SerializeField, Range(0f, 10f)] private float minSpawnTime;
        
        
        private int _boatsLimit;
        private List<BoatPoolElement> _boatPrefabs = new();
        
        private Queue<BoatBase> _pool = new();
        private List<BoatBase> _activePool = new();
        
        private float _nextBoatTimer;
        private float _maxSpawnTimer;
        private PathGenerator _pathGenerator;
        private DiContainer _container;

        public void SetBoatList(List<BoatPoolElement> prefabs) => _boatPrefabs = prefabs;
        public void SetBoatsLimit(int limit) => _boatsLimit = limit;
        public void SetMaxBoatsTimer(float max) => _maxSpawnTimer = max;

        [Inject]
        private void Construct(DiContainer container, PathGenerator pathGenerator)
        {
            _container = container;
            _pathGenerator = pathGenerator;
        }

        public void Initialize()
        {
            _pool = new Queue<BoatBase>(initialPoolSize);
            for (var i = 0; i < initialPoolSize; i++)
            {
                var boat = SpawnBoat(_boatPrefabs[Random.Range(0, _boatPrefabs.Count)]);
                _pool.Enqueue(boat);
            }
        }


        private BoatBase SpawnBoat(BoatPoolElement element)
        {
            var go = _container.InstantiatePrefabForComponent<BoatBase>(element.boat,
                boatSpawnPoints.GetRandomElement());
            go.SetStats(element.stats);
            var points = _pathGenerator.GetRandomPoints(0);
            go.SetTarget(points[0]);
            go.gameObject.SetActive(false);
            return go;
        }

        public BoatBase RequestBoat()
        {
            var boat = _pool.Count > 0 ? _pool.Dequeue() : SpawnBoat(_boatPrefabs[Random.Range(0, _boatPrefabs.Count)]);
            boat.ResetStats();
            boat.gameObject.transform.position = boatSpawnPoints.GetRandomElement().position;
            boat.BoatDestroyed += ReleaseBoat;
            _activePool.Add(boat);
            boat.gameObject.SetActive(true);
            return boat;
        }
        
        
        public void ReleaseBoat(BoatBase boat)
        {
            if (!_activePool.Contains(boat)) return;
            _activePool.Remove(boat);
            boat.gameObject.SetActive(false);
            boat.ResetStats();
            boat.BoatDestroyed -= ReleaseBoat;
            _pool.Enqueue(boat);
        }
        
        private void OnDrawGizmos()
        { 
            Gizmos.color = Color.chartreuse;
            foreach (var spawnPoint in boatSpawnPoints)
            {
                Gizmos.DrawWireSphere(spawnPoint.position, 0.5f);
            } 
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
        }

        public void PerformUpdate(float dt)
        {
            PerformActiveBoatsUpdate(dt);
            if (_activePool.Count >= _boatsLimit) return;

            _nextBoatTimer -= dt;
            if (_nextBoatTimer > 0f) return;
            
            _nextBoatTimer = Random.Range(minSpawnTime, _maxSpawnTimer);
            RequestBoat();
        }

        private void PerformActiveBoatsUpdate(float dt)
        {
            foreach (var boat in _activePool)
            {
                boat.PerformUpdate(dt);
            }
        }

        public void Clear()
        {
            foreach (var boat in _activePool.ToList())
            {
                boat.DestroyBoat();
            }
        }
    }
}