using System;
using Abilities.Configs;
using Abilities.Interfaces;
using Boat.Boats;
using Chargeable;
using Common;
using Lighthouse.Pool;
using Lightzone;

namespace Abilities.Handlers
{
    
    //todo rework
    public class FreezeAbilityHandler : IAbilityHandler
    {
        private readonly LighthouseService _service;
        private float _freezePercent;

        public FreezeAbilityHandler(LighthouseService service, FreezeAbilityConfig config)
        {
            _service = service;
            _freezePercent =  config.FreezePercent;
        }
        
        public void ApplyEffect()
        {
            _service.Lighthouse.Lightzone.OnChargeableEntered += OnChargeableEntered;
            _service.Lighthouse.Lightzone.OnChargeableExited += OnChargeableExited;
        }

        public void DiscardEffect()
        {
            _service.Lighthouse.Lightzone.OnChargeableEntered -= OnChargeableEntered;
            _service.Lighthouse.Lightzone.OnChargeableExited -= OnChargeableExited;
        }

        private void OnChargeableEntered(ChargeableBase obj)
        {
            if (!obj.gameObject.TryGetComponent<BoatBase>(out var boat)) return;
            
            var originalSpeed = boat.GetSpeed();
            boat.SetSpeed(originalSpeed * (1 - _freezePercent));
        }
        
        private void OnChargeableExited(ChargeableBase obj)
        {
            if (!obj.gameObject.TryGetComponent<BoatBase>(out var boat)) return;
            
            var oldSpeed = boat.GetSpeed();
            if (_freezePercent == 0) boat.SetSpeed(oldSpeed);
            else boat.SetSpeed(oldSpeed * (1 / (1 - _freezePercent)));
        }
    }
}