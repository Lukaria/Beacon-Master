using Ads.Signals;
using UnityEngine;
using Zenject;

namespace Ads
{
    public class AdsInstaller : MonoInstaller
    {
        [Header("Ads Initializer")]
        [SerializeField] string androidGameId;
        [SerializeField] string iOSGameId;
        [SerializeField] bool testMode = true;
        
        [Header("Ads Manager")]
        [SerializeField] private string androidAdUnitId = "Rewarded_Android";
        [SerializeField] private string iOSAdUnitId = "Rewarded_iOS";

        

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AdsInitializer>()
                .AsSingle()
                .WithArguments(androidGameId, iOSGameId, testMode)
                .NonLazy();
            
            Container.BindInterfacesAndSelfTo<AdsManager>()
                .AsSingle()
                .WithArguments(androidAdUnitId,  iOSAdUnitId)
                .NonLazy();
            
            Container.DeclareSignal<AdLoadedSignal>();
            Container.DeclareSignal<AdWatchedSignal>();
        }
    }
}