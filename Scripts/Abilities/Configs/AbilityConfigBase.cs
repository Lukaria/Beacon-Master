using System;
using Lighthouse.Lighthouses;
using UnityEngine;

namespace Abilities.Configs
{
    public abstract class AbilityConfigBase : ScriptableObject
    {
        [SerializeField] private AbilityId id;
        [SerializeField] private string title;
        [SerializeField] private string description;
        [SerializeField] private Sprite sprite;
        
        public Sprite Sprite => sprite;
        public string Title => title;
        public string Description => description;
        public AbilityId Id => id;
        
        public abstract Type HandlerType { get; }
        
        protected int AbilityLevel = 0;
    }
    
}