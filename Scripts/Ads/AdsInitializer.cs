using System;
using UnityEngine;
using UnityEngine.Advertisements;
using Zenject;

namespace Ads
{
    public class AdsInitializer : IAdsInitializer, IInitializable,
        IUnityAdsInitializationListener
    {
        public event Action OnInitialized;
        public bool IsInitialized => Advertisement.isInitialized;
        
        private readonly string _androidGameId;
        private readonly string _iOSGameId;
        private readonly bool _testMode = true;
        private string _gameId;

        public AdsInitializer(string androidGameId, string iOSGameId, bool testMode)
        {
            _androidGameId = androidGameId;
            _iOSGameId = iOSGameId;
            _testMode = testMode;
        }
        
        public void Initialize()
        {
            InitializeAds();
        }
        
        public void InitializeAds()
        {
#if UNITY_IOS
            _gameId = _iOSGameId;
#elif UNITY_ANDROID
            _gameId = _androidGameId;
#elif UNITY_EDITOR
            _gameId = _androidGameId;
#endif
            
            if (!Advertisement.isInitialized && Advertisement.isSupported)
            {
                Advertisement.Initialize(_gameId, _testMode,  this);
            }
        }

        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialization complete.");
            OnInitialized?.Invoke();
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }

    }
}