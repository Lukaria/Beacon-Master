using System.Collections.Generic;
using Abilities.Configs;
using Common.Interfaces;
using ObservableCollections;

namespace Abilities.Interfaces
{
    public interface IAbilityContainer : IUpdateable    
    {
        public void AddAbility(AbilityConfigBase ability);

        public AbilityConfigBase GetRandomUniqueAbility(HashSet<AbilityId> abilities);
        
        public AbilityConfigBase GetRandomAbility();
        
        public Dictionary<AbilityId, int> GetAppliedAbilities();

        public void Reset();

        public AbilityConfigBase GetAbilityConfig(AbilityId id);
    }
}