using System;
using Abilities.Signals;
using Cysharp.Threading.Tasks;
using Game;
using R3;
using UI.Windows.Common;
using UI.Windows.HUD;
using UI.Windows.Interfaces;
using Zenject;

namespace UI.Windows
{
    //todo rework
    public class WindowsOrchestrator : IDisposable
    {
        private SignalBus _signalBus;
        private DisposableBag _disposableBag;
        private HUDWindow _hudWindow;
        private IUiNavigation _navigation;

        [Inject]
        public void Construct(
            SignalBus signalBus, 
            IUiNavigation navigation,
            GameManager gameManager)
        {
            _signalBus = signalBus;
            _navigation = navigation;
            
            gameManager.GameStateUpdated
                .SubscribeAwait(async (x, ct) => { await OnGameStateChanged(x);})
                .AddTo(ref _disposableBag);
            
            

            SubscribeForSignals();
        }

        private async UniTask OnGameStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.MainMenu:
                case GameState.GameOver:
                    if (_hudWindow) await _hudWindow.CloseAsync();
                    break;
                
            }
        }

        private void SubscribeForSignals()
        {
            _signalBus.Subscribe<IOpenWindowSignal>(async x=> await OpenWindow(x));
        }

        private async UniTask OpenWindow(IOpenWindowSignal openWindowSignal)
        {
            var window = await _navigation.OpenAsync(openWindowSignal.Id);

            switch (window)
            {
                case HUDWindow hudWindow:
                    if(!_hudWindow) _hudWindow = hudWindow;
                    break;
            }
        }

        public void Dispose()
        {
            _disposableBag.Dispose();
        }
    }
}