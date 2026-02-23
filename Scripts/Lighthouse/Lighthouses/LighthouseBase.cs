using Input.Interfaces;
using Lighthouse.Configs;
using Lighthouse.Stats;
using Lighthouse.Types;
using Lightzone;
using UnityEngine;

namespace Lighthouse.Lighthouses
{
    public abstract class LighthouseBase : MonoBehaviour, IInteractable
    {
        [SerializeField] protected LighthouseConfig config;

        public LighthouseConfig Config => config;
        public abstract LightzoneBase Lightzone { get; }
        
        public abstract LighthouseStatLevels StatLevels { get; set; }
        
        public abstract LighthouseId Id { get; }


        public abstract void Initialize();
        
        public virtual void OnInteract()
        {
        }

        public void OnValidate()
        {
            Utils.Assertions.IsTrueAssert(config.Id == Id,
                "Lighthouse Id and Config Id does not match!");
        }
        
        public virtual void Death(){}
        public virtual void Revive(){}
    }
}
