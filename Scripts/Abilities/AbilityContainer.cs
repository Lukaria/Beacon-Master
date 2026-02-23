using System.Collections.Generic;
using Abilities;
using Abilities.Configs;
using Abilities.Interfaces;
using Abilities.Signals;
using Common;
using ObservableCollections;
using Zenject;

namespace DefaultNamespace
{
    public class AbilityContainer : IAbilityContainer
    {
        private Dictionary<AbilityId, IAbilityHandler> _activeAbilityHandlers = new();
        private WeightedList<AbilityConfigBase> _weightedAbilities = new();
        
        private readonly Dictionary<AbilityId, int> _appliedAbilities = new();
        private IAbilityHandlerFactory _factory;
        private AbilityRegistryData _registry;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(
            SignalBus signalBus,
            IAbilityHandlerFactory factory,
            AbilityRegistryData registryData)
        {
            _signalBus = signalBus;
            _factory = factory;
            _registry = registryData;

            foreach (var (_,weightedAbility) in _registry.Abilities)
            {
                _weightedAbilities.AddDirty(weightedAbility.Ability, weightedAbility.Weight);
            }
            
            _weightedAbilities.Recalculate();
        }
        
        public void AddAbility(AbilityConfigBase ability)
        {
            var id = ability.Id;
            if(_appliedAbilities.TryGetValue(id, out _))
            {
                _appliedAbilities[id] += 1;
                _activeAbilityHandlers[id].ApplyEffect();
            }
            else
            {
                _appliedAbilities.Add(id, 1);
                var handler = _factory.Create(ability);
                handler.ApplyEffect();
                _activeAbilityHandlers.Add(id, handler);
            }
           
            _signalBus.Fire<AbilityContainerUpdatedSignal>();
        }

        public AbilityConfigBase GetRandomUniqueAbility(HashSet<AbilityId> abilities)
        {
            while (true)
            {
                var ability = GetRandomAbility();
                if(!abilities.Contains(ability.Id)) return ability;
            }
        }

        public AbilityConfigBase GetRandomAbility()
        {
            return _weightedAbilities.Next();
        }

        public void Reset()
        {
            foreach (var (id, abilityHandler) in _activeAbilityHandlers)
            {
                var count = _appliedAbilities[id];
                for (var i = 0; i < count; i++)
                {
                    abilityHandler.DiscardEffect();
                }
            }
            
            _appliedAbilities.Clear();
            _activeAbilityHandlers.Clear();
        }

        public AbilityConfigBase GetAbilityConfig(AbilityId id)
        {
            if (_registry.Abilities.TryGetValue(id, out var weightedAbility))
            {
                return weightedAbility.Ability;
            }
            Utils.Assertions.Assert();
            return null;
        }

        public Dictionary<AbilityId, int> GetAppliedAbilities() => _appliedAbilities;
        
        public void PerformUpdate(float dt)
        {
            foreach (var (id, abilityHandler) in _activeAbilityHandlers)
            {
                var count = _appliedAbilities[id];
                for (var i = 0; i < count; i++)
                {
                    abilityHandler.PerformUpdate();
                }
            }   
        }
    }
}