using Common.Interfaces;
using R3;
using UnityEngine;

namespace Common
{
    public class HealthComponent : IHealth
    { 
        private readonly ReactiveProperty<float> _currentHealth = new(1.5f);
        private readonly ReactiveProperty<float> _maxHealth = new(1.5f);

        private readonly Subject<float> _damageSubject = new();
        
        
        public ReadOnlyReactiveProperty<float> CurrentHealth => _currentHealth;
        public ReadOnlyReactiveProperty<float> MaxHealth => _maxHealth;

        public Observable<float> OnDamageTaken => _damageSubject;
        

        public void Set(float amount)
        {
            _currentHealth.Value = _maxHealth.Value = amount;
        }

        public void TakeDamage(float amount)
        {
            _currentHealth.Value -= amount;
            _damageSubject.OnNext(amount);
        }

        public void Heal(float amount)
        {
            _currentHealth.Value = Mathf.Min(_currentHealth.Value + amount, _maxHealth.Value);
        }

        public void IncreaseMaxHealth(float amount)
        {
            if (amount == 0) return;
            
            _maxHealth.Value += amount;
        }

        public void Restore()
        {
            _currentHealth.Value = _maxHealth.Value;
        }
    }
}