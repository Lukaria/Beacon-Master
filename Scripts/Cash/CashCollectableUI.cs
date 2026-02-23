using System;
using DG.Tweening;
using UI.Common;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Cash
{
    public class CashCollectableUI : MonoBehaviour
    {
        [SerializeField] private Button collectButton;
        [SerializeField] private float appearanceAnimationTime;
        [SerializeField] private float disappearanceAnimationTime;

        public Action<CashCollectableUI> OnCollected;
        
        private int _containingCash;
        private UnityEngine.Camera _mainCamera;
        private RectTransform _rectTransform;

        public void Awake()
        {
            _mainCamera = UnityEngine.Camera.main;
            _rectTransform = GetComponent<RectTransform>();
        }

        public int GetContainingCash() => _containingCash;

        public void OnEnable()
        {
            collectButton.onClick.AddListener(CollectButtonClicked);
        }

        public void OnDisable()
        {
            collectButton.onClick.RemoveListener(CollectButtonClicked);
        }

        private void CollectButtonClicked()
        {
            OnCollected?.Invoke(this);
        }

        public void Bind(Vector3 position, int cashAmount)
        {
            var screenPos = _mainCamera.WorldToScreenPoint(position);

            _rectTransform.position = screenPos;
            
            _containingCash = cashAmount;
        }

        private void AnimateText()
        {
            
        }
    }
}