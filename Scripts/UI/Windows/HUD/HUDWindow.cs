using Abilities;
using Common.Interfaces;
using Cysharp.Threading.Tasks;
using Game.Gameplay;
using Game.Score;
using R3;
using UI.Common;
using UI.Windows.Abilities;
using UI.Windows.Common;
using UI.Windows.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Windows.HUD
{
    public class HUDWindow : WindowBase
    {
        [Header("General")]
        [SerializeField] private TimeValueView timeView;
        [SerializeField] private FloatValueView scoreView;
        [SerializeField] private Button pauseButton;
        [Header("PlayerInfo")]
        [SerializeField] private HealthSlider healthSlider;
        [SerializeField] private Slider expSlider;
        [Header("Abilities")]
        [SerializeField] private AbilitiesGridBehaviour abilitiesGridBehaviour;
        
        
        public override WindowId Id => WindowId.GameHUD;
        
        private IPlayerHealth _playerHealth;
        private GameplayController _gameplayController;
        private IScoreService _scoreService;
        private float _maxHealth;
        private AbilityManager _abilityManager;
        
        private DisposableBag _disposableBag;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(
            IPlayerHealth playerHealth,
            GameplayController gameplayController,
            AbilityManager abilityManager,
            IScoreService score,
            SignalBus signalBus)
        {
            _playerHealth = playerHealth;
            _scoreService = score;
            _gameplayController = gameplayController;
            _signalBus = signalBus;
            _abilityManager = abilityManager;
        }

        private void OnEnable()
        {
            _disposableBag.Dispose();
            _disposableBag = new();
            _playerHealth.MaxHealth.Subscribe(OnMaxHealthUpdated).AddTo(ref _disposableBag);
            _playerHealth.CurrentHealth.Subscribe(OnCurrentHealthUpdated).AddTo(ref _disposableBag);
            _scoreService.Score.Subscribe(OnScoreUpdated).AddTo(ref _disposableBag);
            
            _maxHealth = _playerHealth.MaxHealth.CurrentValue;
            pauseButton.onClick.AddListener(OnSettingsButtonClicked);
            healthSlider.UpdateHealth(_playerHealth.CurrentHealth.CurrentValue / _maxHealth);
            abilitiesGridBehaviour.Show();
        }

        private void OnScoreUpdated(float value)
        {
            scoreView.Show(value);
        }

        private void OnSettingsButtonClicked()
        {
            _signalBus.AbstractFire(new OpenWindowSignal{ Id = WindowId.Pause });
        }

        private void OnDisable()
        {
            _disposableBag.Dispose();
            pauseButton.onClick.RemoveListener(OnSettingsButtonClicked);
            abilitiesGridBehaviour.Hide();
        }

        private void OnMaxHealthUpdated(float obj)
        {
            _maxHealth = obj;
            var value = _playerHealth.CurrentHealth.CurrentValue;
            healthSlider.UpdateHealth(value / _maxHealth);
        }


        private void OnCurrentHealthUpdated(float value)
        {
            healthSlider.UpdateHealth(value / _maxHealth);
        }

        private void Update()
        {
            if (_gameplayController.IsGamePaused.CurrentValue) return;
            
            timeView.Show(_gameplayController.GetGameTime());
            expSlider.value = _abilityManager.GetExperiencePercentage();
        }

    }
}