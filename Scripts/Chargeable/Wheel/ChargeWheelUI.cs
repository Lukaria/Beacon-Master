using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Math = Common.Math;

namespace Chargeable.Wheel
{
    public class ChargeWheelUI : MonoBehaviour
    {
        [SerializeField] private Image greenWheel;
        [Header("Anmiation")] 
        [SerializeField, Tooltip("fade in seconds")] private float fadeDelay = 0.4f; 
        [SerializeField, Tooltip("duration in seconds")] private float fadeDuration = 0.2f; 
        
        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private Transform _target;
        private Vector3 _offset;
        private UnityEngine.Camera _mainCamera;
        private Color _greenColor;

        private void Awake()
        {
            greenWheel.fillAmount = 0.0f;
            _greenColor = greenWheel.color;
            _mainCamera = UnityEngine.Camera.main;
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            SetVisible(true);
        }

        public void Bind(Transform unitTransform, Vector3 worldOffset)
        {
            _target = unitTransform;
            _offset = worldOffset;
            greenWheel.fillAmount = 0.0f;
            gameObject.SetActive(true);
        }
    
        public void Unbind()
        {
            _target = null;
            gameObject.SetActive(false);
        }

        public void UpdateFill(float normalizedValue)
        {
            //greenWheel.fillAmount = normalizedValue;
            if (normalizedValue <= 0) return;
            greenWheel.fillAmount = Mathf.Exp(Math.E * Mathf.Log(normalizedValue));
        }
        
        public void UpdatePosition()
        {
            if (!_target) return;
        
            var worldPos = _target.position + _offset;
            var screenPos = _mainCamera.WorldToScreenPoint(worldPos);
        
            // Скрываем если за камерой
            if (screenPos.z < 0)
            {
                _canvasGroup.alpha = 0f;
                return;
            }
        
            _rectTransform.position = screenPos;
        }
        
        public void SetVisible(bool visible)
        {
            _canvasGroup.alpha = visible ? 1f : 0f;
        }

        public async UniTask PlayChargedAnimation()
        {
            greenWheel.fillAmount = 1.0f;
            greenWheel.color = Color.white;
            await UniTask.Delay(TimeSpan.FromSeconds(fadeDelay), ignoreTimeScale: false,
                cancellationToken: this.GetCancellationTokenOnDestroy());
            await DOVirtual.Color(greenWheel.color, _greenColor, fadeDuration, value =>
            {
                greenWheel.color = value;
            });
            await _canvasGroup.DOFade(0f, fadeDuration);
        }
    }
}