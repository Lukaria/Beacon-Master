using DG.Tweening;
using UnityEngine;

namespace UI.Windows.HUD
{
    public class HealthSlider : MonoBehaviour
    {
        [SerializeField] private RectTransform frontImageRT;
        [SerializeField] private RectTransform backImageRT;
        
        [Header("Settings")]
        [SerializeField] private float chipSpeed = 2f;
        [SerializeField] private float chipDelay = 0.5f;

        private Tween _backBarTween;
        private float _lastTargetPercent;
        private const float MaxAnchorHeight = 1.0f;

        private void Awake()
        {
            if (frontImageRT != null)
                _lastTargetPercent = frontImageRT.anchorMax.x;
        }

        public void UpdateHealth(float targetPercent)
        {
            frontImageRT.anchorMax = new Vector2(targetPercent, MaxAnchorHeight);

            var isTakingDamage = targetPercent < _lastTargetPercent;
            
            HandleBackBar(targetPercent, isTakingDamage);

            _lastTargetPercent = targetPercent;
        }

        private void HandleBackBar(float targetPercent, bool isTakingDamage)
        {
            var fillAmount = backImageRT.anchorMax.x;
            
            if (fillAmount <= targetPercent) 
            {
                _backBarTween?.Kill();
                backImageRT.anchorMax = new Vector2(targetPercent, MaxAnchorHeight);
                return;
            }

            var distance = Mathf.Abs(fillAmount - targetPercent);
            var duration = distance / chipSpeed;
            
            if (isTakingDamage)
            {
                _backBarTween?.Kill();
                
                _backBarTween = backImageRT
                    .DOAnchorMax(new Vector2(targetPercent, MaxAnchorHeight), duration)
                    .SetDelay(chipDelay)
                    .SetEase(Ease.OutCubic)
                    .SetLink(gameObject);
            }
            else
            {
                _backBarTween?.Kill();

                _backBarTween = backImageRT
                    .DOAnchorMax(new Vector2(targetPercent, MaxAnchorHeight), duration)
                    .SetEase(Ease.Linear)
                    .SetLink(gameObject);
            }
        }
    }
}