using System;
using Abilities;
using Boat.Pool;
using Common.Interfaces;
using Game.Configs;
using Game.Score;
using Grid;
using Lighthouse.Pool;
using ModestTree;
using ObjectPath;
using Obstacle.ObstaclePool;
using Player;
using R3;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public class GameplayController : MonoBehaviour
    {
        [SerializeField, Range(0f, 20f)] private float maxObstacleSpawnTimer;
        [SerializeField, Range(0f, 20f)] private float maxBoatSpawnTimer;
        [SerializeField] private Vector3 lighthouseTransform;
        
        public Action GameOverCallback;
        public Action GameRestartedCallback;
        public ReadOnlyReactiveProperty<bool> IsGamePaused => _isGamePaused;
        
        private TimeConfigEntry[] _configs;
        private LevelConfig _currentConfig;
        private int _nextConfigIndex = 0;

        private ReactiveProperty<bool> _isGamePaused = new();
        private bool _lastConfigApplied;
        private float _obstacleSpawnTime;
        private float _boatSpawnTime;
        private ObstaclePool _obstaclePool;
        private GridManager _gridManager;
        private PathGenerator _pathGenerator;
        private BoatPool _boatPool;
        
        private float _gameTime;
        private LighthouseService _lighthouseService;
        private PlayerDataService _playerData;
        private Location.LocationService _locationService;
        private AbilityManager _abilityManager;
        private int _reviveCounter;


        //todo fix god object
        [Inject]
        public void Construct(
            LighthouseService lighthouseService,
            Location.LocationService locationService,
            ObstaclePool obstaclePool,
            BoatPool boatPool,
            GridManager gridManager,
            PathGenerator pathGenerator,
            AbilityManager abilityManager,
            IPlayerHealth playerHealth,
            PlayerDataService playerData)
        {
            _locationService = locationService;
            _obstaclePool = obstaclePool;
            _boatPool = boatPool;
            _gridManager = gridManager;
            _pathGenerator = pathGenerator;
            _lighthouseService = lighthouseService;
            _playerData = playerData;
            _abilityManager =  abilityManager;
            
            playerHealth.CurrentHealth
                .Where(value => value <= 0).Subscribe(OnPlayerDeath).AddTo(this);
        }

        private void Awake()
        {
            PauseGame();
            ConfigureObstacleSpawnPositions();
        }

        private void OnPlayerDeath(float _)
        {
            PauseGame();
        }

        public void GameOver()
        {
            _lighthouseService.DisableLighthouse();
            PauseGame();
            GameOverCallback?.Invoke();
            ClearEnemies();
        }

        public void Clear()
        {
            _gameTime = 0.0f;
            _lastConfigApplied = false;
            _nextConfigIndex = 0;
            _reviveCounter = 0;
            
            ClearEnemies();
            _abilityManager.Clear();
        }

        public void ClearEnemies()
        {
            _boatPool.Clear();
            _obstaclePool.Clear();
        }

        private void ConfigureLevelFeatures()
        {
            ConfigurePathGenerator();
            ConfigureObstaclePool();
            ConfigureBoatPool();
        }

        private void ConfigureBoatPool()
        {
            if (_currentConfig.Boats.IsEmpty()) Utils.Assertions.Assert("Config's boats pool is empty!");
            
            _boatPool.SetBoatList(_currentConfig.Boats);
            _boatPool.SetBoatsLimit(_currentConfig.BoatsSpawnLimit);
            _boatPool.SetMaxBoatsTimer(maxBoatSpawnTimer/_currentConfig.BoatsSpawnRate);
            _boatPool.Initialize();
        }

        private void ConfigureObstacleSpawnPositions()
        {
            var obstacleSpawnPositions = _gridManager.GetPoints();
            _obstaclePool.SetSpawnPoints(obstacleSpawnPositions);
            _pathGenerator.SetEndPoints(obstacleSpawnPositions);
        }
        
        private void ConfigurePathGenerator()
        {
            _pathGenerator.SetUniqueEndPoint(lighthouseTransform);
        }

        private void ConfigureObstaclePool()
        {
            if (_currentConfig.Obstacles.IsEmpty()) return;
            
            _obstaclePool.SetObstaclePrefabs(_currentConfig.Obstacles);
            _obstaclePool.SetObstaclesLimit(_currentConfig.ObstaclesSpawnLimit);
            _obstaclePool.SetMaxObstacleTimer(maxObstacleSpawnTimer/_currentConfig.ObstaclesSpawnRate);
        }

        public void PauseGame()
        {
            _isGamePaused.Value = true;
        }
        
        public void UnpauseGame()
        {
            _isGamePaused.Value = false;
        }
        public void RestartGame()
        {
            Clear();
            _gameTime = 0.0f;
            _lastConfigApplied = false;
            _nextConfigIndex = 0;
            
            _lighthouseService.InitializeLighthouse(_playerData.UnlockedLighthouseId);

            _configs = _locationService.GetLocationDifficulty.LevelConfigs;
            ProcessDifficultyConfig();
            _isGamePaused.Value = false;
            GameRestartedCallback?.Invoke();
        }

        private void ProcessDifficultyConfig()
        {
            if (_lastConfigApplied) return;

            if (_nextConfigIndex >= _configs.Length || !(_gameTime >= _configs[_nextConfigIndex].TimeThreshold)) return;
            
            ApplyLevelConfig(_configs[_nextConfigIndex].LevelConfig);
            _nextConfigIndex++;
            _lastConfigApplied = _nextConfigIndex == _configs.Length;
        }

        private void ApplyLevelConfig(LevelConfig levelConfig)
        {
            _currentConfig = levelConfig;
            ConfigureLevelFeatures();
        }

        private void Update()
        {
            if (_isGamePaused.CurrentValue) return;

            
            ProcessDifficultyConfig();
            
            var dt = Time.deltaTime;
            
            _lighthouseService.PerformUpdate(dt);
            _obstaclePool.PerformUpdate(dt);
            _boatPool.PerformUpdate(dt);
            _abilityManager.PerformUpdate(dt);
        }

        private void FixedUpdate()
        {
            if (_isGamePaused.CurrentValue) return;
            _gameTime += Time.deltaTime;
        }

        public float GetGameTime() => _gameTime;

        public float GetUpdateConfigTimePercents()
        {
            if (_configs.Length <= 1) return _gameTime / _configs[0].TimeThreshold;

            return _gameTime / (_configs[_nextConfigIndex].TimeThreshold -
                                (_nextConfigIndex == 0
                                    ? 0
                                    : _configs[_nextConfigIndex - 1].TimeThreshold)
                );
        }
        
        public int GetReviveCounter() => _reviveCounter;

        public void Revive()
        {
            ++_reviveCounter;
            _lighthouseService.Revive();
            ClearEnemies();
        }
    }
}