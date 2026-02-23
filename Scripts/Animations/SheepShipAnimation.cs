using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Animations
{
    public class SheepShipAnimation : MonoBehaviour
    {
        [SerializeField] private List<Transform> sheeps;
        [Header("Animation")]
        [SerializeField] private float scaleMultiplier = 1.2f;
        [SerializeField] private float duration = 0.5f;         
        [SerializeField] private Ease easeType = Ease.InOutSine;

        private Vector3 initialScale;

        void Start()
        {
            initialScale = sheeps[0].localScale;

            Vector3 endScale = initialScale * scaleMultiplier;

            foreach (var sheep in sheeps)
            {
                sheep.DOScale(endScale, duration)
                    .SetEase(easeType)        
                    .SetLoops(-1, LoopType.Yoyo) 
                    .SetLink(gameObject);
            }
        }
    }
}