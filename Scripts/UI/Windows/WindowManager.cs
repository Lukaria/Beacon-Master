using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Gameplay;
using UI.Windows.Common;
using UI.Windows.Interfaces;
using UnityEngine;
using Zenject;

namespace UI.Windows
{
    public sealed class WindowManager : MonoBehaviour, IUiNavigation
    {
        // Ссылки на родительские трансформации для слоев (назначить в инспекторе)
        [SerializeField] private Transform layerScreen;
        [SerializeField] private Transform layerPopup;
        [SerializeField] private Transform layerOverlay;

        private IWindowFactory _windowFactory;

        private readonly Dictionary<WindowId, WindowBase> _windowCache = new();

        // Блокировщик инпута во время анимаций переходов
        private bool _isTransitioning;
        private readonly Stack<WindowBase> _navigationStack = new();
        private GameplayController _gameplayController;

        [Inject]
        public void Construct(IWindowFactory windowFactory)
        {
            _windowFactory = windowFactory;
        }

        public async UniTask<WindowBase> OpenAsync(WindowId id)
        {
            if (_isTransitioning) return null;
            _isTransitioning = true;

            try
            {
                var window = await GetOrCreateWindow(id);
                var layer = GetLayerForWindow(id);

                // Логика стека: если это ПОЛНОЭКРАННОЕ окно, скрываем предыдущее
                if (layer == WindowLayer.Screen && _navigationStack.Count > 0)
                {
                    var top = _navigationStack.Peek();
                    if (top.Id != id) // Не скрываем сами себя
                    {
                        // Не await, чтобы новое окно начало открываться параллельно
                        _ = top.CloseAsync();
                    }
                }

                // Добавляем в стек, если это не оверлей и не HUD
                if (ShouldAddToStack(layer))
                {
                    // Если окно уже в стеке, вынимаем его (перемещаем наверх)
                    // Упрощенная логика, можно доработать под конкретные нужды
                    _navigationStack.Push(window);
                }

                window.transform.SetAsLastSibling(); // Наверх внутри слоя
                await window.OpenAsync();
                return window;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                _isTransitioning = false;
            }
        }


        public async UniTask GoBackAsync()
        {
            if (_isTransitioning || _navigationStack.Count <= 1) return;
            _isTransitioning = true;

            try
            {
                var current = _navigationStack.Pop();
                await current.CloseAsync();

                if (_navigationStack.Count > 0)
                {
                    var previous = _navigationStack.Peek();
                    await previous.OpenAsync(); // Re-open (или просто Show, если не уничтожали)
                }
            }
            finally
            {
                _isTransitioning = false;
            }
        }

        private async UniTask<WindowBase> GetOrCreateWindow(WindowId id)
        {
            if (_windowCache.TryGetValue(id, out var window))
                return window;

            var layerTr = GetLayerTransform(GetLayerForWindow(id));
            window = await _windowFactory.CreateAsync(id, layerTr);
            _windowCache[id] = window;
            return window;
        }

        // Конфигурация: какое окно куда относится
        private WindowLayer GetLayerForWindow(WindowId id)
        {
            return id switch
            {
                WindowId.MainMenu => WindowLayer.Screen,
                WindowId.GameHUD => WindowLayer.Screen,
                WindowId.RewardPopup => WindowLayer.Popup,
                WindowId.Pause => WindowLayer.Popup,
                WindowId.LoadingScreen => WindowLayer.Overlay,
                WindowId.ChooseAbility => WindowLayer.Popup,
                WindowId.Revive => WindowLayer.Popup,
                _ => WindowLayer.Screen
            };
        }

        private Transform GetLayerTransform(WindowLayer layer)
        { 
            return layer switch
            {
                WindowLayer.Popup => layerPopup,
                WindowLayer.Overlay => layerOverlay,
                _ => layerScreen
            };
        }

        private bool ShouldAddToStack(WindowLayer layer)
        {
            return layer == WindowLayer.Screen || layer == WindowLayer.Popup;
        }

        // Обработка хардварной кнопки Back (для Android)
        /*private void Update()
       {
          if (Input.GetKeyDown(KeyCode.Escape))
           {
               // Здесь можно добавить проверку: если открыт Popup - закрыть его,
               // если главный экран - показать диалог выхода.
               if (_navigationStack.Count > 1)
               {
                   GoBack();
               }
           }
       }*/
    }
}