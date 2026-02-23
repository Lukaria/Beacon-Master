using Cysharp.Threading.Tasks;
using Game;
using Game.Gameplay;
using Haptic;
using Settings;
using UI.Windows.Abilities;
using UI.Windows.Common;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using MoreMountains.NiceVibrations;
using R3;

namespace UI.Pause
{
    public class PauseWindow : WindowBase
    {
        public override WindowId Id => WindowId.Pause;

        [SerializeField] private Button closeButton;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider soundSlider;
        [SerializeField] private Button vibrationButton;
        [SerializeField] private Toggle vibrationToggle;
        [SerializeField] private Button homeButton;
        [SerializeField] private AbilitiesGridBehaviour abilitiesGridBehaviour;
        
        private GameplayController _gameplayController;
        private GameManager _gameManager;
        private SettingsService _settings;
        private HapticManager _haptic;

        [Inject]
        public void Construct(
            GameplayController gameplayController,
            GameManager gameManager, 
            SettingsService settings,
            HapticManager haptic)
        {
            _gameplayController = gameplayController;
            _gameManager = gameManager;
            _settings = settings;
            _haptic = haptic;
            
            closeButton.OnClickAsObservable()
                .SubscribeAwait(async (_, ct) => await OnCloseButtonClicked())
                .AddTo(this);
        }

        private void MusicSliderChanged(float arg0)
        {
            _settings.SetMusicVolume(arg0);
        }
        
        private void SoundSliderChanged(float arg0)
        {
            _settings.SetSfxVolume(arg0);
        }

        protected override void OnBeforeShow()
        {
            _gameplayController.PauseGame();
        }
        
        private void OnHomeButtonClicked()
        {
            _gameManager.GameOver();
        }

        private void OnEnable()
        {
            Setup();
            
            vibrationButton.onClick.AddListener(OnVibrationButtonClicked);
            musicSlider.onValueChanged.AddListener(MusicSliderChanged);
            soundSlider.onValueChanged.AddListener(SoundSliderChanged);
            abilitiesGridBehaviour.Show();
            abilitiesGridBehaviour.RedrawItems();
            homeButton.onClick.AddListener(OnHomeButtonClicked);
        }

        private void Setup()
        {
            musicSlider.value = _settings.MusicVolume;
            soundSlider.value = _settings.SfxVolume;
            
            vibrationToggle.isOn = _haptic.IsHapticActive;
        }

        private void OnDisable()
        {
            vibrationButton.onClick.RemoveListener(OnVibrationButtonClicked);
            musicSlider.onValueChanged.RemoveListener(MusicSliderChanged);
            soundSlider.onValueChanged.RemoveListener(SoundSliderChanged);
            abilitiesGridBehaviour.Hide();
            homeButton.onClick.RemoveListener(OnHomeButtonClicked);
        }

        private void OnVibrationButtonClicked()
        {            
            var haptic = !_haptic.IsHapticActive;
            _haptic.SetHaptic(haptic);
            vibrationToggle.isOn = haptic;
        }

        private async UniTask OnCloseButtonClicked()
        {
            await CloseAsync();
            _gameplayController.UnpauseGame();
        }
    }
}