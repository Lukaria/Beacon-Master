using System;
using System.Collections.Generic;
using Abilities.Interfaces;
using AYellowpaper.SerializedCollections;
using Camera;
using UnityEngine;

namespace Abilities.Configs
{
    [CreateAssetMenu(fileName ="AbilityRegistry", menuName = "Configs/Abilities/AbilityRegistry")]
    public class AbilityRegistry : Zenject.ScriptableObjectInstaller
    {
        public AbilityRegistryData RegistryData;

        public override void InstallBindings()
        {
            Container.BindInstance(RegistryData).AsSingle();
        }

        public void OnValidate()
        {
            foreach (var (typeId, config) in RegistryData.Abilities)
            {
                Utils.Assertions.IsTrueAssert(typeId == config.Ability.Id, "AbilityId of Key and Config does not match!");
            }
            
            var types = new HashSet<Type>();
            foreach (var (_, config) in RegistryData.Abilities)
            {
                if(types.TryGetValue(config.Ability.HandlerType, out var type))
                {
                    Utils.Assertions.IsTrueAssert(type == config.Ability.HandlerType,
                        "Registry contains abilities with the same handler type [" + config.Ability.HandlerType +"]");
                }
                else
                {
                    types.Add(config.Ability.HandlerType);
                }
                
            }
        }
    }

    [Serializable]
    public class WeightedAbility
    {
        [SerializeField] private AbilityConfigBase ability;
        [SerializeField] private int weight;
        
        public AbilityConfigBase Ability => ability;
        public int Weight => weight;
    }

    [Serializable]
    public class AbilityRegistryData
    {
        [SerializedDictionary] public SerializedDictionary<AbilityId, WeightedAbility> Abilities;
    }
}