using System;
using Billboard;
using Common.Interfaces;
using Cysharp.Threading.Tasks;
using GameResources.Sprites;
using Lighthouse.Pool;
using Loading.Signals;
using Player;
using UnityEngine;
using Zenject;

namespace Loading
{
    public class Bootstrap : MonoBehaviour
    {
        private Location.LocationService _locationService;
        private PlayerDataService _playerData;
        private LighthouseService _lighthouseService;
        private SpriteAtlasManager _atlasManager;
        private ISceneLoader _sceneLoader;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(
            SignalBus signalBus,
            ISceneLoader sceneLoader,
            Location.LocationService locationService,
            PlayerDataService playerData,
            SpriteAtlasManager atlasManager,
            LighthouseService lighthouseService)
        {
            _signalBus = signalBus;
            _sceneLoader = sceneLoader;
            _locationService = locationService;
            _playerData = playerData;
            _lighthouseService = lighthouseService;
            _atlasManager = atlasManager;
        }

        private void Awake()
        {
            SetupGame();
            _sceneLoader.HideLoadingScreen();
        }

        private async UniTaskVoid SetupGame()
        {
            await LoadAtlasesAsync();
            _lighthouseService.EnableLighthouse(_playerData.UnlockedLighthouseId);
            _locationService.EnableLocation(_locationService.UnlockedLocationId);
            _signalBus.Fire<GameLoadedSignal>();
        }

        private async UniTask LoadAtlasesAsync()
        {
            _atlasManager.AddAtlasHandler<BillboardType>(new SpriteAtlasHandler<BillboardType>());
            await _atlasManager.LoadAtlasesAsync();
        }
    }
}