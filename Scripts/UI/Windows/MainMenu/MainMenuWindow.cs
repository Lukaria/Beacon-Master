using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game;
using UI.Windows.Common;
using UI.Windows.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Windows.MainMenu
{
    public class MainMenuWindow : WindowBase
    {
        [SerializeField] private GameObject title;
        [SerializeField] private GameObject description;
        [SerializeField] private Button leaderboardButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button playButton;
        
        [Header("Hide/show animation")]
        [SerializeField] private float animDuration;
        [SerializeField] private float hideButtonsXPosition;
        [SerializeField] private float hideTitleYPosition;

        public override WindowId Id =>  WindowId.MainMenu;
        
        private GameManager _gameManager;
        
        private Sequence _hideAnimationSequence;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(GameManager gameManager, SignalBus signalBus)
        {
            _signalBus = signalBus;
            _gameManager = gameManager;
            
            CreateHideAnimationSequence();
        }

        private void CreateHideAnimationSequence()
        {
            _hideAnimationSequence = DOTween.Sequence()
                .Append(title.GetComponent<RectTransform>().DOAnchorPosY(hideTitleYPosition, animDuration)
                    .SetEase(Ease.OutQuad))
                
                .Join(leaderboardButton.GetComponent<RectTransform>().DOAnchorPosX(-hideButtonsXPosition, animDuration)
                    .SetEase(Ease.OutQuad))
                
                .Join(settingsButton.GetComponent<RectTransform>().DOAnchorPosX(hideButtonsXPosition, animDuration)
                    .SetEase(Ease.OutQuad))
                
                .SetAutoKill(false)
                .Pause();
        }


        private void OnLeaderboardButtonClicked()
        {
            _signalBus.AbstractFire(new OpenWindowSignal{ Id = WindowId.Leaderboard });
        }
        
        private void OnSettingsButtonClicked()
        {
            _signalBus.AbstractFire(new OpenWindowSignal{ Id = WindowId.Settings });
        }

        public void OnEnable()
        {
            leaderboardButton.onClick.AddListener(OnLeaderboardButtonClicked);
            settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }

        public void OnDisable()
        {
            leaderboardButton.onClick.RemoveListener(OnLeaderboardButtonClicked);
            settingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
            playButton.onClick.RemoveListener(OnPlayButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            _gameManager.RestartGame();
        }

        protected override async UniTask AnimateShowAsync()
        {
            _canvasGroup.alpha = 1;
            playButton.gameObject.SetActive(true);
            _hideAnimationSequence.PlayBackwards();
            await Task.CompletedTask;
        }
        
        protected override async UniTask AnimateHideAsync()
        {
            playButton.gameObject.SetActive(false);
            _hideAnimationSequence.PlayForward();
            await _hideAnimationSequence.AwaitForComplete();
            _canvasGroup.alpha = 0;
        }
        
        private void OnDestroy()
        {
            _hideAnimationSequence.Kill();
        }
    }
}