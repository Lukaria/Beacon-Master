using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UI.Windows.Interfaces;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace UI.Windows.Common
{
    public class WindowFactory : IWindowFactory
    {
        private DiContainer _container;
        
        private readonly Dictionary<WindowId, string> _prefabs = new()
        {
            { WindowId.Settings, "SettingsWindow" },
            { WindowId.LighthouseShop, "LighthouseShopWindow" },
            { WindowId.LocationShop, "LocationShopWindow" },
            { WindowId.MainMenu, "MainMenuWindow" },
            { WindowId.Leaderboard, "LeaderboardWindow" },
            { WindowId.GameHUD, "HUDWindow" },
            { WindowId.Pause, "PauseWindow" },
            { WindowId.GameOver, "GameOverWindow" },
            { WindowId.ChooseAbility, "ChooseAbilityWindow" },
            { WindowId.Revive, "ReviveWindow" },
        };

        [Inject]
        public void Construct(DiContainer container)
        {
            _container = container;
        }

        public async UniTask<WindowBase> CreateAsync(WindowId id, Transform parent)
        {
            if (!_prefabs.TryGetValue(id, out var path))
            {
                Utils.Assertions.Assert($"Path for window {id} not found");
            }

            try
            {
                var request = Addressables.LoadAssetAsync<GameObject>(path);
                await request.Task;
                if (request.Status != AsyncOperationStatus.Succeeded)
                    throw new Exception($"Failed to load window {id}");
                
                var window = _container.InstantiatePrefabForComponent<WindowBase>(request.Result, parent);
                window.Initialize();
                return window;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}