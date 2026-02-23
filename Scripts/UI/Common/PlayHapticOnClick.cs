using System;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;
using UnityEngine.AddressableAssets;

namespace UI.Common
{
    public class PlayHapticOnClick : MonoBehaviour
    {
        [SerializeField] public HapticType type;

        private Button _button;

        public void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(Vibrate);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Vibrate);
        }
        
        private void Vibrate()
        {
            switch (type)
            {
                case HapticType.Pop: MMVibrationManager.Haptic (HapticTypes.SoftImpact);
                    break;
                case HapticType.Peek: MMVibrationManager.Haptic (HapticTypes.MediumImpact);
                    break;
                case HapticType.Vibrate: MMVibrationManager.Haptic (HapticTypes.Warning);
                    break;
                case HapticType.Nope: MMVibrationManager.Haptic (HapticTypes.RigidImpact);
                    break;
                default: Debug.LogWarning("vibration not implemented");
                    break;
            }
        }
    }

    public enum HapticType
    {
        Pop = 0,
        Peek = 1,
        Vibrate = 2,
        Nope = 3
    }
}