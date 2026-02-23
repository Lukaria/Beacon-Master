using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Game;
using Game.Gameplay;
using Game.Score;
using Leaderboard;
using R3;
using TMPro;
using UI.Common;
using UI.Windows.Abilities;
using UI.Windows.Common;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Windows.GameOver
{
    public class GameOverWindow : WindowBase
    {
        public override WindowId Id =>  WindowId.GameOver;
        
        [SerializeField] private Button playAgainButton; 
        [SerializeField] private Button toMainMenuButton;
        [SerializeField] private FloatValueView scoreValueView;
        [SerializeField] private TimeValueView timeValueView;
        [SerializeField] private AbilitiesGridBehaviour abilitiesGridBehaviour;
        [SerializeField] private TMP_Text newRecordText;
        [SerializeField] private float recordTextScale = 2.0f;
        [SerializeField] private float recordTextAnimDuration = 0.5f;
        
        private GameManager _gameManager;
        private GameplayController _gameplayController;
        private IScoreService _scoreService;
        private LeaderboardService _leaderboardService;
        private TweenerCore<Vector3, Vector3, VectorOptions> _textTween;
        private Vector3 _textLocalScale = Vector3.one;

        [Inject]
        public void Construct(
            GameManager gameManager,
            LeaderboardService leaderboardService,
            GameplayController gameplayController, IScoreService scoreService)
        {
            _leaderboardService = leaderboardService;
            _gameManager = gameManager;
            _gameplayController = gameplayController;
            _scoreService = scoreService;
            
            _textLocalScale = newRecordText.transform.localScale;
            _leaderboardService.IsNewRecord.Subscribe(value =>
            {
                if (value)
                {
                    ShowNewRecord();
                }
                else
                {
                    newRecordText.gameObject.SetActive(false);
                }
            }).AddTo(this);
        }

        private void OnEnable()
        {
            scoreValueView.Show(_scoreService.Score.CurrentValue);
            timeValueView.Show(_gameplayController.GetGameTime());
            abilitiesGridBehaviour.Show();
            abilitiesGridBehaviour.RedrawItems();
            playAgainButton.onClick.AddListener(PlayAgainButtonClicked);
            toMainMenuButton.onClick.AddListener(ToMainWindowButtonClicked);
        }

        private void ShowNewRecord()
        {
            _textTween.Kill();
            newRecordText.gameObject.SetActive(true);
            newRecordText.transform.localScale = _textLocalScale;
            _textTween = newRecordText.transform.DOScale(newRecordText.transform.localScale * recordTextScale, 
                    recordTextAnimDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(newRecordText.gameObject);
        }

        private void OnDisable()
        {
            playAgainButton.onClick.RemoveListener(PlayAgainButtonClicked);
            toMainMenuButton.onClick.RemoveListener(ToMainWindowButtonClicked);
            abilitiesGridBehaviour.Hide();
        }

        private void PlayAgainButtonClicked()
        {
            _gameManager.RestartGame();
        }
        
        private void ToMainWindowButtonClicked()
        {
            _gameManager.ToMainWindow();
        }
    }
}