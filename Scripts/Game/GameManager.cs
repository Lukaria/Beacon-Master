using System.Threading;
using Common.Interfaces;
using Cysharp.Threading.Tasks;
using Game.Gameplay;
using Game.Signals;
using Loading.Signals;
using R3;
using UI.Windows.Common;
using UI.Windows.Signals;
using Zenject;

namespace Game
{
    public class GameManager : ILateDisposable
    {
        public ReadOnlyReactiveProperty<GameState> GameStateUpdated => _gameState;
        
        private ReactiveProperty<GameState> _gameState = new();
        
        private GameplayController _gameplayController;
        
        private DisposableBag _disposableBag;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(
            SignalBus signalBus,
            IPlayerHealth playerHealth,
            GameplayController gameplayController)
        {
            _gameState.Value = GameState.NotStarted;
            _signalBus = signalBus;
            _gameplayController = gameplayController;

            playerHealth.CurrentHealth
                .Where(value => value <= 0 && _gameState.Value == GameState.Playing)
                .SubscribeAwait(async (_, ct) => { await OfferRevivePlayer(ct); })
                .AddTo(ref _disposableBag);

            signalBus.Subscribe<GameLoadedSignal>(_ => _gameState.Value = GameState.MainMenu);
        }


        public void RestartGame()
        {
            _gameplayController.RestartGame();
            _signalBus.AbstractFire(new GameRestartedSignal{ Id = WindowId.GameHUD });
            _gameState.Value = GameState.Playing;
        }

        public void ToMainWindow()
        {
            _gameState.Value = GameState.MainMenu;
            _gameplayController.GameOver();
        }

        public void LateDispose()
        {
            _disposableBag.Dispose();
        }

        private async UniTask OfferRevivePlayer(CancellationToken ct)
        {
            await UniTask.Delay(1500, true).AttachExternalCancellation(ct);
            _signalBus.AbstractFire(new OpenWindowSignal{ Id = WindowId.Revive });
        }

        public void Revive()
        {
            _gameplayController.Revive();
            _gameplayController.UnpauseGame();
        } 

        public void GameOver()
        {
            _signalBus.AbstractFire<GameOverSignal>();
            _gameplayController.GameOver();
            
            _signalBus.AbstractFire(new OpenWindowSignal{ Id = WindowId.GameOver });
            _gameState.Value = GameState.GameOver;
        }
    }
}