using System;
using Abilities.Configs;
using Abilities.Interfaces;
using Common.Interfaces;
using R3;
using UnityEngine;

namespace Abilities.Handlers
{
    public class RegenerationAbilityHandler : IAbilityHandler, IDisposable
    {
        private readonly IPlayerHealth _playerHealth;
        private readonly RegenerationAbilityConfig _config;
        private float _maxHealth;
        private IDisposable _disposable;

        public RegenerationAbilityHandler(IPlayerHealth playerHealth, RegenerationAbilityConfig config)
        {
            _playerHealth = playerHealth;
            _config = config;
        }
        
        public void ApplyEffect()
        { 
            _disposable = _playerHealth.MaxHealth
                .Pairwise()
                .Subscribe(tuple => OnMaxHealthChanged(tuple.Current));
            
            _maxHealth = _playerHealth.MaxHealth.CurrentValue;
        }

        public void PerformUpdate()
        {
            _playerHealth.Heal(_maxHealth * _config.RegenerationPercent * Time.deltaTime);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }

        private void OnMaxHealthChanged(float obj)
        {
            _maxHealth = obj;
        }
    }
}