using R3;

namespace Common.Interfaces
{
    public interface IHealth : IDamageable
    { 
        ReadOnlyReactiveProperty<float> CurrentHealth { get; } 
        ReadOnlyReactiveProperty<float> MaxHealth { get; } 
        
        void Heal(float amount);
        void IncreaseMaxHealth(float amount);
        void Restore();
        public void Set(float amount);

    }
}