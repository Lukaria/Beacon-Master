using Common.Interfaces;
using Cysharp.Threading.Tasks;
using Game;
using Game.Gameplay;
using Leaderboard;
using Lighthouse.Dto;
using Location;
using Player;
using Save.Interfaces;
using UI.BottomBar;
using UI.Windows.HUD;
using UnityEngine;
using Zenject;

namespace Save
{
    public class SaveManager : MonoBehaviour
    {
        private bool _isDirty = false;
        private float _autoSaveInterval = 60f;
        private float _timer; 
        
        private GameplayController _gameplayController;
        private BottomBarController _bottomBarController;
        private HUDWindow _hudWindow;
        private GameState _currentState;
        private IDataService<PlayerDataDto> _playerDataService;
        private IDataService<LighthouseDataDto> _lighthouseDataService;
        private IDataService<LeaderboardDataDto> _leaderboardDataService;
        private IDataService<LocationDataDto> _locationDataService;

        [Inject]
        public async UniTask Construct(
            SignalBus signalBus,
            IDataService<LocationDataDto> locationData,
            IDataService<LighthouseDataDto> lighthouseData,
            IDataService<LeaderboardDataDto> leaderboardData,
            PlayerDataService playerData
        )
        {
            _locationDataService = locationData;
            _playerDataService = playerData;
            _lighthouseDataService = lighthouseData;
            _leaderboardDataService = leaderboardData;

            signalBus.Subscribe<ISaveGameSignal>(_ => MarkDirty());
            
            await LoadGame();
        }

        public async UniTask LoadGame()
        {
            await _lighthouseDataService.LoadAsync();
            await _locationDataService.LoadAsync();
            await _leaderboardDataService.LoadAsync();
            await _playerDataService.LoadAsync();
        }
        
        public void FixedUpdate()
        {
            _timer += Time.unscaledDeltaTime;
            if (_timer >= _autoSaveInterval)
            {
                _timer = 0;
                if (_isDirty) SaveGame();
            }
        }
        
        public void MarkDirty() => _isDirty = true;

        public async UniTaskVoid SaveGame()
        {
            await _lighthouseDataService.SaveAsync();
            await _locationDataService.SaveAsync();
            await _leaderboardDataService.SaveAsync();
            await _playerDataService.SaveAsync();
            
            Debug.Log("Game Saved!");
            _isDirty = false;
            _timer = 0;
        }
        
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus && _isDirty)
            {
                Debug.Log("[SaveManager] App Paused. Saving...");
                SaveGame();
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus && _isDirty)
            {
                Debug.Log("[SaveManager] App Lost Focus. Saving...");
                SaveGame();
            }
        }
        
        private void OnApplicationQuit()
        {
            Debug.Log("[SaveManager] App Quit. Saving...");
            if(_isDirty) SaveGame();
        }
    }
}