using Camera;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game;
using Loading.Signals;
using R3;
using UI.Windows.Common;
using UI.Windows.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.BottomBar
{
    public class BottomBarController : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button locationsButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button lighthouseButton;
        
        [Header("Button selection animation")]
        [SerializeField] private float animDuration = 0.2f;
        [SerializeField] private Vector2 originalButtonSize = new (150f, 150f);
        [SerializeField] private Vector2 selectedButtonSize = new (220f, 150f);
        
        
        [Header("Hide/show animation")]
        [SerializeField] private float bounceUpAmount;
        [SerializeField] private float bounceUpDuration;
        [SerializeField] private float hideDownAmount;
        [SerializeField] private float hideDownDuration;
        
        private Button _currentButton = null;
        
        private bool _isExecuting;
        private CameraController _cameraController;
        
        private Sequence _animationSequence;
        private IUiNavigation _navigation;

        [Inject]
        public void Construct(
            IUiNavigation navigation,
            GameManager gameManager,
            CameraController cameraController)
        {
            _navigation = navigation;
            _cameraController = cameraController;
            
            var rectTransform = GetComponent<RectTransform>();
            
            _animationSequence = DOTween.Sequence()
                .Append(rectTransform.DOAnchorPosY(bounceUpAmount, bounceUpDuration)
                .SetEase(Ease.OutQuad))
                
                .Append(rectTransform.DOAnchorPosY(hideDownAmount, hideDownDuration)
                .SetEase(Ease.InBack))
                
                .SetAutoKill(false)
                .Pause();

            SetupButtonActions();
            
            gameManager.GameStateUpdated.SubscribeAwait(async (value, ct) =>
            {
                if (value == GameState.MainMenu)
                {
                    await OnMainMenuButtonClicked();
                    Show();
                }
                else
                {
                    Hide();
                }
            }).AddTo(this);
        }

        private void SetupButtonActions()
        {
            locationsButton.OnClickAsObservable().SubscribeAwait(async (_, ct) =>
            {
                await OnLocationsButtonClicked();
            }).AddTo(this);
                
            mainMenuButton.OnClickAsObservable().SubscribeAwait(async (_, ct) =>
            {
                await OnMainMenuButtonClicked();
            }).AddTo(this);
            
            lighthouseButton.OnClickAsObservable().SubscribeAwait(async (_, ct) =>
            {
                await OnLighthouseButtonClicked();
            }).AddTo(this);
            
        }

        private async UniTask OnLighthouseButtonClicked()
        {
            SetSelectedButton(lighthouseButton);
            _cameraController.EnableShopCamera();
            await _navigation.OpenAsync(WindowId.LighthouseShop);
        }

        private async UniTask OnLocationsButtonClicked()
        {
            SetSelectedButton(locationsButton);
            _cameraController.EnableMainCamera();
            await _navigation.OpenAsync(WindowId.LocationShop);
        }


        private async UniTask OnMainMenuButtonClicked()
        {
            SetSelectedButton(mainMenuButton);
            _cameraController.EnableMainCamera(); 
            await _navigation.OpenAsync(WindowId.MainMenu);
        }

        public async UniTaskVoid Hide()
        {
            _animationSequence.PlayForward();
            await _animationSequence.AwaitForComplete();
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _animationSequence.PlayBackwards();
        }

        private void SetSelectedButton(Button button)
        {
            if (_currentButton == button) return;

            if (_currentButton != null)
            {
                _currentButton.interactable = true;
                _currentButton.GetComponent<RectTransform>().DOSizeDelta(originalButtonSize, animDuration);
            }
            _currentButton = button;
            button.GetComponent<RectTransform>().DOSizeDelta(selectedButtonSize, animDuration);
            button.interactable = false;
        }
        
        private void OnDestroy()
        {
            _animationSequence.Kill();
        }
    }
}