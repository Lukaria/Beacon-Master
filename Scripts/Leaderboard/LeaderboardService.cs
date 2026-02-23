using System;
using Abilities;
using Abilities.Interfaces;
using Common.Interfaces;
using Game;
using Game.Gameplay;
using Game.Score;
using Game.Signals;
using Lighthouse.Pool;
using R3;
using Zenject;
using LocationService = Location.LocationService;

namespace Leaderboard
{
    public class LeaderboardService  : IDisposable
    {
        public ReadOnlyReactiveProperty<bool> IsNewRecord => _isNewRecord;
        
        private ReactiveProperty<bool> _isNewRecord = new(false);
        
        private IAbilityContainer _abilities;
        private IScoreService _scoreService;
        private GameplayController _gameplayController;
        private LighthouseService _lighthouseService;
        private LocationService _locationService;
        private IDataService<LeaderboardDataDto> _dataService;
        private IDisposable _disposable;


        [Inject]
        public void Construct(
            IScoreService scoreService,
            GameManager gameManager,
            GameplayController gameplayController,
            IAbilityContainer abilities,
            LocationService locationService,
            LighthouseService lighthouseService,
            IDataService<LeaderboardDataDto> dataService)
        {
            _dataService = dataService;
            _scoreService = scoreService;
            _gameplayController = gameplayController;
            _abilities = abilities;
            _locationService = locationService;
            _lighthouseService = lighthouseService;

            _disposable = gameManager.GameStateUpdated.Subscribe(state =>
            {
                if (state == GameState.Playing)
                {
                    _isNewRecord.Value = false;  
                }
                else if (state == GameState.GameOver)
                {
                    TrySaveRecord();
                }
            });
        }

        private void TrySaveRecord()
        {
            if (!VerifyNewRecord()) return;
            SaveRecord();
        }

        private bool VerifyNewRecord()
        {
            _isNewRecord.Value = _scoreService.Score.CurrentValue > _dataService.GetData().Score;
            return _isNewRecord.Value;
        }

        public void SaveRecord()
        {
            var data = _dataService.GetData();
            data.Score = _scoreService.Score.CurrentValue;
            data.Time = _gameplayController.GetGameTime();
            data.Lighthouse = _lighthouseService.Lighthouse.Id;
            data.Location = _locationService.UnlockedLocationId;

            data.Abilities.Clear();
            
            foreach (var (id, count) in _abilities.GetAppliedAbilities())
            {
                Utils.Assertions.IsTrueAssert(data.Abilities.TryAdd(id, count));
            }
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}