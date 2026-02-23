using Cysharp.Threading.Tasks;
using Haptic;
using R3;
using Settings;
using UI.BottomBar;
using UI.Windows.Common;
using UI.Windows.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Windows.Settings
{
    public class SettingsWindow : WindowBase
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider soundSlider; 
        
        [SerializeField] private Button vibrationButton;
        [SerializeField] private Toggle vibrationToggle;

        [SerializeField] private Button languageButton;
        [SerializeField] private Button rateUsButton;
        [SerializeField] private Button contactsButton;
        [SerializeField] private Button disableAddButton;
        [SerializeField] private Button restorePurchasesButton;
        [SerializeField] private Button quitGameButton;
        
        private SettingsService _settings;
        private HapticManager _haptic;
        private IUiNavigation _navigation;
        private BottomBarController _bottomBar;

        public override WindowId Id => WindowId.Settings;

        [Inject]
        public void Construct(
            IUiNavigation navigation,
            SettingsService settings,
            BottomBarController bottomBar,
            HapticManager haptic)
        {
            _settings = settings;
            _haptic = haptic;
            _navigation = navigation;
            _bottomBar = bottomBar;
            soundSlider.value = _settings.SfxVolume;
            musicSlider.value = _settings.MusicVolume;
            vibrationToggle.isOn = _haptic.IsHapticActive;
            
            closeButton.OnClickAsObservable().SubscribeAwait(async (_, ct) =>
            {
                await OnCloseButtonClicked();
            }).AddTo(this);
        }

        private void OnEnable()
        {
            Setup();
            vibrationButton.onClick.AddListener(OnVibrationButtonClicked);
            quitGameButton.onClick.AddListener(OnQuitGameButtonClicked);
            soundSlider.onValueChanged.AddListener(OnSoundSliderChanged);
            musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);

            _bottomBar.Hide();
        }

        private async UniTask OnCloseButtonClicked()
        {
            await _navigation.GoBackAsync();
        }

        private void Setup()
        {
            musicSlider.value = _settings.MusicVolume;
            soundSlider.value = _settings.SfxVolume;
            
            vibrationToggle.isOn = _haptic.IsHapticActive;
        }

        private void OnDisable()
        {
            _bottomBar.Show();
            vibrationButton.onClick.RemoveListener(OnVibrationButtonClicked);
            quitGameButton.onClick.RemoveListener(OnQuitGameButtonClicked);
            soundSlider.onValueChanged.RemoveListener(OnSoundSliderChanged);
            musicSlider.onValueChanged.RemoveListener(OnMusicSliderChanged);
        }

        private void OnQuitGameButtonClicked()
        {
            Application.Quit();
        }

        private void OnVibrationButtonClicked()
        {
            var haptic = !_haptic.IsHapticActive;
            _haptic.SetHaptic(haptic);
            vibrationToggle.isOn = haptic;
        }


        private void OnMusicSliderChanged(float value)
        {
            _settings.SetMusicVolume(value);
        }

        private void OnSoundSliderChanged(float value)
        {
            _settings.SetSfxVolume(value);
        }
    }
}