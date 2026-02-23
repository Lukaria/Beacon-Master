using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Windows.Common
{
    [RequireComponent(typeof(Canvas)), RequireComponent(typeof(CanvasGroup))]
    public abstract class WindowBase : MonoBehaviour
    {
        public abstract WindowId Id { get; }
    
        protected Canvas _canvas;
        protected CanvasGroup _canvasGroup;

        protected virtual void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }


        public virtual void Initialize()
        {
            _canvasGroup.blocksRaycasts = false; 
            _canvasGroup.alpha = 0;
        
            gameObject.SetActive(false); 
        }

        public async UniTask OpenAsync()
        {
            gameObject.SetActive(true);
            OnBeforeShow();
        
            await AnimateShowAsync();
        
            _canvasGroup.blocksRaycasts = true;
            OnShown();
        }

        public async UniTask CloseAsync()
        {
            _canvasGroup.blocksRaycasts = false;
            OnBeforeHide();
        
            await AnimateHideAsync();
        
            gameObject.SetActive(false);
            OnHidden();
        }
        
        protected virtual void OnBeforeShow() { }
        protected virtual void OnShown() { }
        protected virtual void OnBeforeHide() { }
        protected virtual void OnHidden() { }

        protected virtual async UniTask AnimateShowAsync()
        {
            _canvasGroup.alpha = 1;
            await Task.CompletedTask; 
        }

        protected virtual async UniTask AnimateHideAsync()
        {
            _canvasGroup.alpha = 0;
            await Task.CompletedTask;
        }
    }
}