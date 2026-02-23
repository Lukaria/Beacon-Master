using System;
using Common;
using Common.Interfaces;
using Lighthouse.Pool;
using Settings;
using UnityEngine;
using Zenject;
using MoreMountains.NiceVibrations;
using R3;

namespace Haptic
{
    public class HapticManager : IInitializable, ILateDisposable
    {
        public bool IsHapticActive { get; private set; }
        
        
        private IPlayerHealth _playerHealth;
        private IDisposable _disposable;
        

        [Inject]
        public void Construct(IPlayerHealth playerHealth)
        {
            Setup();
            _playerHealth = playerHealth;
            _disposable = _playerHealth.CurrentHealth
                .Pairwise()
                .Subscribe(tuple =>
                {
                    if (tuple.Previous < tuple.Current) return;
                    HealthChanged(tuple.Current);
                });
        }

        public void Setup()
        {
            if(MMVibrationManager.HapticsSupported()) Debug.LogWarning("vibration is not available");
            else Debug.Log("vibration is available");
        }


        public void Initialize()
        {
            IsHapticActive = PlayerPrefs.GetInt(SettingsConstants.HAPTIC_ACTIVE, 1) == 1;
        }


        public void SetHaptic(bool flag)
        {
            IsHapticActive = flag;
            MMVibrationManager.SetHapticsActive(IsHapticActive);
            
            if(IsHapticActive) MMVibrationManager.Haptic (HapticTypes.Success);
        }
        

        public void LateDispose()
        {
            _disposable.Dispose();
            PlayerPrefs.SetInt(SettingsConstants.HAPTIC_ACTIVE, IsHapticActive ? 1 : 0);
        }

        private void HealthChanged(float currentHealth)
        {
            MMVibrationManager.Haptic(
                currentHealth <= 0
                ? HapticTypes.MediumImpact
                : HapticTypes.HeavyImpact);
        }
    }
}