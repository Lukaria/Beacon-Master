using DG.Tweening;
using UnityEngine;

namespace Obstacle.Obstacles
{
    public class Thundercloud : ObstacleBase
    {
        [SerializeField] private ThundercloudHitTriggerCollider hitCollider;
        [Header("Animation")]
        [SerializeField] private float waveHeight;
        [SerializeField] private Vector3 centerOffset; //offest for Y so origin of object become shifted
        [SerializeField] private Transform visualRoot;
        [SerializeField] private Vector3 initialVisualPosition;
        [SerializeField] private float appearanceAnimationDuration;
        
        private Sequence _animationSequence;
        
        
        private void OnEnable()
        {
            hitCollider.SetDamage(obstacleStats.damage);
            GetComponent<Collider>().enabled = false;
            AnimateShow();
        }
        
        private void AnimateShow()
        {
            var visualCenterPosition = visualRoot.localPosition + centerOffset;

            var pos = visualRoot.localPosition;
            pos.y = visualCenterPosition.y - waveHeight;

            //floating animation
            _animationSequence = DOTween.Sequence()
                .Append(visualRoot.DOLocalMove(
                        new Vector3(pos.x, pos.y + waveHeight, pos.z),
                        appearanceAnimationDuration)
                    .SetEase(Ease.InOutSine)
                )
                .SetLoops(-1, LoopType.Yoyo)
                .Pause();
            
            //appearance animation
            visualRoot.localPosition = initialVisualPosition;
            visualRoot
                .DOLocalMove(pos, appearanceAnimationDuration)
                .OnComplete(() =>
                {
                    EnableCollider();
                    _animationSequence.Play();
                });
        }
        
        private void EnableCollider()
        {
            GetComponent<Collider>().enabled = true;
        }


        protected override void OnCharged()
        {
            visualRoot
                .DOLocalMove(initialVisualPosition, appearanceAnimationDuration)
                .OnComplete(() =>
                {
                    _animationSequence.Kill();
                    DestroyObstacle();
                });
        }
    }
}