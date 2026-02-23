using System;
using Ads;
using Ads.Signals;
using DG.Tweening;
using Game;
using Game.Gameplay;
using Common;
using Common.Interfaces;
using Cysharp.Threading.Tasks;
using Player;
using R3;
using R3.Triggers;
using UI.Common;
using UI.Windows.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Sequence = DG.Tweening.Sequence;

namespace UI.Windows.Revive
{
    public class ReviveWindow : WindowBase
    {
        [SerializeField] private Button showAdButton;
        [SerializeField] private Button payCashButton;
        [SerializeField] private GameObject disableCashButtonImage;
        [SerializeField] private IntValueView cashButtonText;
        [SerializeField] private Button closeButton;
        [SerializeField] private int reviveBasePrice = 100;
        [SerializeField] private int countdownFrom = 5;
        [Header("countdown animation")]
        [SerializeField] private IntValueView countdownText;
        [SerializeField] private float textDelayTime = 1.2f;
        [SerializeField] private float textAppearanceTime = 0.2f;


        private int _cashNeeded;
        private GameplayController _gameplayController;
        private GameManager _gameManager;
        private PlayerDataService _playerDataManager;
        private Sequence _countdownSequence;
        private AdsManager _adsManager;
        private Vector3 _countdownTextScale = Vector3.one;

        public override WindowId Id => WindowId.Revive;

        [Inject]
        public void Construct(
            SignalBus signalBus,
            GameplayController gc, 
            GameManager gameManager, 
            AdsManager adsManager,
            PlayerDataService playerDataManager)
        {
            _gameplayController = gc;
            _gameManager = gameManager;
            _adsManager = adsManager;
            _playerDataManager = playerDataManager;
            _countdownTextScale = countdownText.transform.localScale;

            adsManager.IsAdLoaded
                .Where(value => value is true)
                .Subscribe(_ => EnableWatchAdButton()).AddTo(this);
            
            signalBus.Subscribe<AdWatchedSignal>(_ => Revive());
            
        }

        protected override void Awake()
        {
            base.Awake();
            showAdButton.OnPointerClickAsObservable().Subscribe(_ => OnAdButtonClick()).AddTo(this);
            payCashButton.OnPointerClickAsObservable().Subscribe(_ => OnPayCashButtonClick()).AddTo(this);
            showAdButton.OnPointerClickAsObservable().Subscribe(_ => OnAdButtonClick()).AddTo(this);
            closeButton.OnClickAsObservable()
                .SubscribeAwait(async (_, ct) => await OnCloseButtonClick())
                .AddTo(this);
        }

        protected override async UniTask AnimateShowAsync()
        {
            _canvasGroup.alpha = 1;
            gameObject.transform.localScale = Vector3.zero;
            await gameObject.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutQuad);
        }
        
        protected override async UniTask AnimateHideAsync()
        {
            await gameObject.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutQuad);
            _canvasGroup.alpha = 0;
        }

        private void EnableWatchAdButton()
        {
            showAdButton.interactable = true;
        }

        private void SetupPayCashButton()
        {
            var revives = _gameplayController.GetReviveCounter();
            var price = reviveBasePrice * (1 << revives); //fast 2^n
            _cashNeeded = price;
            cashButtonText.Show(_cashNeeded);
            
            if (_playerDataManager.GetCash() < _cashNeeded)
            {
                disableCashButtonImage.SetActive(true);
                payCashButton.interactable = false;
            }
            else
            {
                disableCashButtonImage.SetActive(false);
                payCashButton.interactable = true;
            }
        }
        

        private void OnAdButtonClick()
        {
            _adsManager.ShowAd();
        }

        private void OnPayCashButtonClick()
        {
            if (!_playerDataManager.TrySubtractCash(_cashNeeded))
            {
                Utils.Assertions.Assert("trying to subtract more cash than Player has!");
            }
            Revive();
        }

        private async UniTask OnCloseButtonClick()
        {
            _countdownSequence.Kill();
            await CloseAsync();
            _gameManager.GameOver();
        }

        public async UniTaskVoid Revive()
        {
            _countdownSequence.Kill();
            await CloseAsync();
            _gameManager.Revive();
        }

        private void StartCountdown()
        {
            _countdownSequence.Kill();
            _countdownSequence = CreateCountdownSequence();
            countdownText.transform.localScale = Vector3.zero;
            _countdownSequence.Play();
        }

        private Sequence CreateCountdownSequence()
        {
            var seq = DOTween.Sequence();
            seq.Pause();

            var countdownTransform = countdownText.transform;
            var originalScale = _countdownTextScale;
            
            for (var i = countdownFrom; i >= 0; --i)
            {
                var i1 = i;
                seq
                    .AppendCallback(() =>
                    {
                        countdownText.Show(i1);
                    })
                    .Append(countdownTransform.DOScale(originalScale, textAppearanceTime).SetEase(Ease.OutQuad))
                    .AppendInterval(textDelayTime)
                    .Append(countdownTransform.DOScale(Vector3.zero, textAppearanceTime).SetEase(Ease.OutQuad));
            }
            seq.OnComplete(() =>
            {
                OnCloseButtonClick().Forget();
            });
            return seq;
        }
        
        
        private void OnEnable()
        {
            countdownText.Hide();
            SetupPayCashButton();
            StartCountdown();
        }   
        
        private void OnDisable()
        {
            countdownText.transform.localScale = _countdownTextScale;
        }
    }
}