using System;
using System.Threading;
using Common.Interfaces;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Lighthouse.Types;
using Location;
using Player;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Windows.Loading
{
    [RequireComponent(typeof(CanvasGroup)), RequireComponent(typeof(GraphicRaycaster))]
    public class LoadingScreenWindow : MonoBehaviour
    {
        [SerializeField] private Image lighthouseImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private string dissolvePropertyId = "_DissolvePercent";
        [SerializeField] private float animationDuration;

        private int _dissolveProperty;
    
        private Material _lighthouseMaterial;
        private Material _backgroundMaterial;
        
        private CancellationTokenSource _cts;
        private CanvasGroup _canvasGroup;
        private GraphicRaycaster _graphicRaycaster;
        private PlayerDataService _playerData;
        private IDataService<LocationDataDto> _locationData;

        [Inject]
        public void Construct(PlayerDataService data, IDataService<LocationDataDto> locationData)
        {
            _playerData = data;
            _locationData = locationData;
            _dissolveProperty = Shader.PropertyToID(dissolvePropertyId);

            _canvasGroup = GetComponent<CanvasGroup>();
            CopyMaterials();
        }

        private void CopyMaterials()
        {
            _backgroundMaterial = new Material(backgroundImage.material);
            backgroundImage.material = _backgroundMaterial;
            
            _lighthouseMaterial = new Material(lighthouseImage.material);
            lighthouseImage.material = _lighthouseMaterial;
        }

        public async UniTask ShowAsync()
        {
            gameObject.SetActive(true);
            _backgroundMaterial.SetFloat(_dissolveProperty, 0);
            _lighthouseMaterial.SetFloat(_dissolveProperty, 0);
            
            _canvasGroup.alpha = 1;
        }

        public async UniTask HideAsync()
        {
            await PlayDissolveAnimation(0f, 1.0f);
            _canvasGroup.alpha = 0;
            gameObject.SetActive(false);
        }


        private async UniTask PlayDissolveAnimation(float startValue, float endValue)
        {
            var tween = _backgroundMaterial
                .DOFloat(endValue, _dissolveProperty, animationDuration).From(startValue);


            var locations = _locationData.GetData();
            if (_playerData.ChosenLighthouseId != LighthouseId.Standard ||
                locations.Unlocked[locations.CurrentLocationIndex] == LocationId.Noir ||
                locations.Unlocked[locations.CurrentLocationIndex] == LocationId.Foggy)
            {
                _lighthouseMaterial.DOFloat(endValue, _dissolveProperty, animationDuration).From(startValue);
            }

            await tween.ToUniTask(cancellationToken: this.GetCancellationTokenOnDestroy());
        }
        
    }
}