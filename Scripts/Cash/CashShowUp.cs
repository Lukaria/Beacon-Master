using System;
using System.Text;
using DG.Tweening;
using TMPro;
using UI.Common;
using UnityEngine;

namespace Cash
{
    public class CashShowUp : MonoBehaviour
    {
        [SerializeField] private IntValueView cashView;
        [SerializeField] private float appearanceHeight;
        [SerializeField] private float textAppearanceAnimationTime;
        [SerializeField] private float textVisibilityTime;
        [SerializeField] private float textDisappearanceAnimationTime;
        private RectTransform _rectTransform;
        private UnityEngine.Camera _mainCamera;
        private CanvasGroup _canvasGroup;
        private StringBuilder _stringBuilder;

        private void Awake()
        {
            _mainCamera = UnityEngine.Camera.main;
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;
            _stringBuilder = new StringBuilder();
            cashView.SetAdditionalConversion(text =>
            {
                _stringBuilder.Clear();
                _stringBuilder.Append("+").Append(text);
                return _stringBuilder.ToString();
            });
        }

        public void Show(Vector3 position, int value)
        {
            _rectTransform.position = _mainCamera.WorldToScreenPoint(position);
            cashView.Show(value);

            DOTween.Sequence()
                .Append(
                    _canvasGroup.DOFade(1f, textAppearanceAnimationTime).SetEase(Ease.Linear))
                .Insert(0,
                    gameObject.transform.DOLocalMoveY(appearanceHeight, textAppearanceAnimationTime)
                        .SetRelative()
                        .SetEase(Ease.OutSine)
                )
                .AppendInterval(textVisibilityTime)
                .Append(
                    _canvasGroup.DOFade(0f, textDisappearanceAnimationTime)).SetEase(Ease.Linear)
                .Insert(textAppearanceAnimationTime + textVisibilityTime,
                    gameObject.transform.DOLocalMoveY(appearanceHeight, textDisappearanceAnimationTime)
                        .SetRelative()
                        .SetEase(Ease.OutSine)
                );
        }
    }
}