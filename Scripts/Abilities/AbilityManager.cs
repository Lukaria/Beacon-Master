using Abilities.Configs;
using Abilities.Interfaces;
using Abilities.Signals;
using Common.Interfaces;
using R3;
using UI.Windows.Common;
using UnityEngine;
using Zenject;

namespace Abilities
{
    public class AbilityManager : MonoBehaviour, IUpdateable
    {
        
        [SerializeField] private AnimationCurve experienceCurve;
        [SerializeField] private float baseXP = 100f;
        [SerializeField] private float expPerSecond = 0f;
        [SerializeField] private int maxLevel = 100;
        [SerializeField] private float curveWeight = 1f;

        public ReadOnlyReactiveProperty<int> Level => _currentLevel;
        private float _experience;
        private ReactiveProperty<int> _currentLevel = new();
        private float _nextAbilityExperience;
        private IAbilityContainer _abilityContainer;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(
            SignalBus signalBus,
            IAbilityContainer abilityContainer)
        {
            _signalBus = signalBus;
            _abilityContainer = abilityContainer;
        }

        public void Clear()
        {
            _experience = 0;
            _currentLevel.Value = 1;
            _nextAbilityExperience = EvaluateNextAbilityExperience();
            
            _abilityContainer.Reset();
        }
        
        public void AddExperience(float value)
        {
            _experience += value;

            if (_experience < _nextAbilityExperience) return;
            
            _experience = 0;
            _currentLevel.Value++;
            _signalBus.AbstractFire(new LevelUpSignal{ Id = WindowId.ChooseAbility });
            _nextAbilityExperience = EvaluateNextAbilityExperience();
        }

        private float EvaluateNextAbilityExperience()
        {
            var t = Mathf.Clamp01(_currentLevel.Value / (float)maxLevel);
            return baseXP * _currentLevel.Value * (1 + experienceCurve.Evaluate(t) * curveWeight);
        }

        public void PerformUpdate(float dt)
        {
            AddExperience(expPerSecond * dt);
            _abilityContainer.PerformUpdate(dt);
        }
        
        public float GetExperiencePercentage() => _experience / _nextAbilityExperience;
    }
}