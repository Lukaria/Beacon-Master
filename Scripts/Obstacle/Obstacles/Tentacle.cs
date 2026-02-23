using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Obstacle.Obstacles
{
    public class Tentacle : ObstacleBase
    {
        [SerializeField] private TentacleHitTriggerCollider hitCollider;
        [Header("Animation")]
        [SerializeField] private Vector3 centerOffset; //offest for Y so origin of object become shifted
        [SerializeField] private Transform visualRoot;
        [SerializeField] private Vector3 initialVisualPosition;
        [SerializeField] private float appearanceAnimationDuration;

        private void OnEnable()
        {
            hitCollider.SetDamage(obstacleStats.damage);
            transform.rotation = Quaternion.Euler(0, Random.Range(-120, 60) + 30, 0);
            GetComponent<Collider>().enabled = false;
            AnimateShow();
        }
        
        private void AnimateShow()
        {
            var visualCenterPosition = visualRoot.localPosition + centerOffset;

            var pos = visualRoot.localPosition;
            pos.y = visualCenterPosition.y;

            //appearance animation
            visualRoot.localPosition = initialVisualPosition;
            visualRoot
                .DOLocalMove(pos, appearanceAnimationDuration)
                .OnComplete(() => { EnableCollider(); });
        }

        private void EnableCollider()
        {
            GetComponent<Collider>().enabled = true;
        }


        protected override void OnCharged()
        {
            visualRoot
                .DOLocalMove(initialVisualPosition, appearanceAnimationDuration)
                .OnComplete(() => { DestroyObstacle(); });
        }
    }
}