using R3;

namespace Game.Score
{
    public interface IScoreService
    {
        ReadOnlyReactiveProperty<float> Score { get; }
        
        public void AddScore(float value);
        public void SubtractScore(float value);
        public void SetScore(float value);
        public void Reset();
    }
}