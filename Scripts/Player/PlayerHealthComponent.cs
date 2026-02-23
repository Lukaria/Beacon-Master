using Common.Interfaces;
using R3;

namespace Common
{
    public class PlayerHealthComponent : IPlayerHealth
    {
        private HealthComponent _healthComponent = new();

        public ReadOnlyReactiveProperty<float> CurrentHealth => _healthComponent.CurrentHealth;
        public ReadOnlyReactiveProperty<float> MaxHealth => _healthComponent.MaxHealth;
        public Observable<float> OnDamageTaken => _healthComponent.OnDamageTaken;
        
        public void Heal(float amount) => _healthComponent.Heal(amount);
        public void IncreaseMaxHealth(float amount) => _healthComponent.IncreaseMaxHealth(amount);

        public void Restore() =>  _healthComponent.Restore();

        public void Set(float amount) => _healthComponent.Set(amount);

        public void TakeDamage(float amount) =>  _healthComponent.TakeDamage(amount);
    }
}