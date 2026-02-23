using System;
using Chargeable;
using Common.Interfaces;
using Lightzone.Dto;
using Lightzone.Interfaces;
using UnityEngine;

namespace Lightzone
{
    public abstract class LightzoneBase : MonoBehaviour, ILightzone, IUpdateable
    {
        
        public LightzoneDto Stats { get; set; }
        public abstract ILightzoneThreshold Threshold { get; }
        
        public Action<ChargeableBase> OnChargeableEntered { get; set; }
        public Action<ChargeableBase> OnChargeableExited { get; set; }

        public abstract void Initialize(Vector3 center, LightzoneDto stats);

        private void Update(){}
        
        public virtual void PerformUpdate(float dt){}

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<ChargeableBase>(out var chargeable))
                OnChargeableEntered?.Invoke(chargeable);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent<ChargeableBase>(out var chargeable))
                OnChargeableExited?.Invoke(chargeable);
        }
    }
}