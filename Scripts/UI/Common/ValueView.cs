using System;
using TMPro;
using UnityEngine;

namespace UI.Common
{
    [RequireComponent(typeof(TMP_Text))]
    public class ValueView<T> : MonoBehaviour where T : IConvertible
    {
        [SerializeField] private TMP_Text _text;

        public delegate string DelegateConversion(string text);

        private DelegateConversion _delegateConversion = (string text) => text;

        private void Awake()
        {
            _text ??= GetComponent<TMP_Text>();
        }

        public void Show(T value)
        {
            Set(value);
            gameObject.SetActive(true);
        }
        
        public void Set(T value)
        {
            var text = Convert(value);
            _text.text = AdditionalConversion(text);
        }

        protected virtual string Convert(T value)
        {
            return value.ToString();
        }

        private string AdditionalConversion(string text)
        {
            return _delegateConversion is not null ? _delegateConversion?.Invoke(text) : text;
        }

        public void SetAdditionalConversion(DelegateConversion delegateConversion)
        {
            _delegateConversion = delegateConversion;
        }

        public void Hide() => gameObject.SetActive(false);
    }
}