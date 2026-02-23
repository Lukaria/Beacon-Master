using System;
using Ads.Signals;
using R3;
using UnityEngine;
using UnityEngine.Advertisements;
using Zenject;

namespace Ads
{
    public class AdsManager : IInitializable, IDisposable,
        IUnityAdsLoadListener, IUnityAdsShowListener
    {
        public ReadOnlyReactiveProperty<bool> IsAdLoaded => _isAdLoaded;
        
        private readonly string _androidAdUnitId;
        private readonly string _iOSAdUnitId;
        
        
        private string _adUnitId = null;
        private ReactiveProperty<bool> _isAdLoaded = new();
        private IAdsInitializer _adsInitializer;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus, IAdsInitializer adsInitializer)
        {
            _signalBus = signalBus;
            _adsInitializer = adsInitializer;
        }

        public AdsManager(string androidAdUnitId, string iOSAdUnitId)
        {
            _androidAdUnitId = androidAdUnitId;
            _iOSAdUnitId = iOSAdUnitId;
        }
        
        public void Initialize()
        {
            // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
            _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
            _adUnitId = _androidAdUnitId;
#endif
            
            if (_adsInitializer.IsInitialized)
            {
                LoadAd();
            }
            else
            {
                // Подписываемся и ждем
                _adsInitializer.OnInitialized += LoadAd;
            }
        }
        
        // Call this public method when you want to get an ad ready to show.
        private void LoadAd()
        {
            // IMPORTANT! Only load content AFTER initialization
            // (in this example, initialization is handled in a different script).
            Debug.Log("Loading Ad: " + _adUnitId);
            Advertisement.Load(_adUnitId, this);
        }
        
        // Implement a method to execute when the user clicks the button:
        public void ShowAd()
        {
            // Then show the ad:
            Advertisement.Show(_adUnitId, this);
        }
        
        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            Debug.Log("Ad Loaded: " + adUnitId);

            if (!adUnitId.Equals(_adUnitId)) return;
            
            _isAdLoaded.Value = true;
            _signalBus.Fire<AdLoadedSignal>();
        }
        
        // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            if (!adUnitId.Equals(_adUnitId) ||
                !showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED)) return;
            
            Debug.Log("Unity Ads Rewarded Ad Completed");
            _signalBus.Fire<AdWatchedSignal>();
            
            _isAdLoaded.Value = false;
            LoadAd();
        }
        
        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Use the error details to determine whether to try to load another ad.
        }

        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
            
            _isAdLoaded.Value = false;
            LoadAd();
        }

        public void OnUnityAdsShowStart(string adUnitId)
        {
            _isAdLoaded.Value = false;
        }

        public void OnUnityAdsShowClick(string adUnitId)
        {
        }

        public void Dispose()
        {
            _adsInitializer.OnInitialized -= LoadAd;
        }
    }
}