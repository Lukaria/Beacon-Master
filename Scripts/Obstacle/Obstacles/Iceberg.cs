using System;
using DG.Tweening;
using UnityEngine;

namespace Obstacle.Obstacles
{
    public class Iceberg : ObstacleBase
    {
        [SerializeField] private float waveHeight;
        [SerializeField] private Vector3 centerOffset; //offest for Y so origin of object become shifted
        [SerializeField] private float animationDuration;
        [SerializeField] private float appearanceAnimationDuration;
        [SerializeField] private Transform visualRoot;
        [SerializeField] private Vector3 initialVisualPosition;
        
        private Sequence _animationSequence;
        private int _blendShapeIndex;
        private SkinnedMeshRenderer _icebergMesh;

        public override void Awake()
        {
            base.Awake();
            _icebergMesh = visualRoot.gameObject.GetComponent<SkinnedMeshRenderer>();
            _blendShapeIndex = _icebergMesh.sharedMesh.GetBlendShapeIndex("Melted");
        }

        private void OnEnable()
        {
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
                        animationDuration)
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

        public override void PerformUpdate(float dt)
        {
            base.PerformUpdate(dt);

            if (InZone)
            {
                _icebergMesh.SetBlendShapeWeight(_blendShapeIndex, _chargePercent * 100);
            }
        }
    }
}