using System;
using Game.Signals;
using R3;
using Zenject;

namespace Game.Score
{
    public class ScoreService : IScoreService
    {
        public ReadOnlyReactiveProperty<float> Score => _score;
        
        private ReactiveProperty<float> _score = new(0);

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            signalBus.Subscribe<GameRestartedSignal>(_ => Reset());
        }
        
        public void AddScore(float value)
        {
             _score.Value += value;
        }

        public void SubtractScore(float value)
        {
            _score.Value = Math.Max(_score.Value - value, 0);
        }

        public void SetScore(float value)
        {
            _score.Value = value;
        }

        public void Reset()
        {
            _score.Value = 0;
        }
    }
}