using System;
using System.Collections.Generic;
using System.Linq;
using Camera;
using Common;  
using Common.Interfaces;
using Cysharp.Threading.Tasks;
using Lighthouse.Configs;
using Lighthouse.Dto;
using Lighthouse.Lighthouses;
using Lighthouse.Stats;
using Lighthouse.Types;
using R3;
using Sound;
using UnityEngine;
using Zenject;
using ZLinq;

namespace Lighthouse.Pool
{
    [RequireComponent(typeof(Collider))]
    public class LighthouseService : MonoBehaviour, IUpdateable
    {
        [SerializeField] private Transform spawnPoint;
        
        [SerializeField] private List<LighthouseBase> lighthouseBehaviours;

        [Header("Animation")] 
        [SerializeField] private int flickeringCount = 4;
        [SerializeField] private float flickeringDuration = 0.07f;
        
        private LighthouseBase _lighthouse;
        private IPlayerHealth _playerHealth;
        private SoundManager _soundManager;
        private IDataService<LighthouseDataDto> _dataService;
        private CameraController _cameraController;

        [Inject]
        public void Construct(
            IPlayerHealth playerHealth,
            IDataService<LighthouseDataDto> dataService,
            CameraController cameraController,
            SoundManager soundManager)
        {
            _dataService = dataService;
            _cameraController = cameraController;
            _playerHealth = playerHealth;
            _playerHealth.OnDamageTaken.SubscribeAwait(async (value, ct) =>
            {
                await LighthouseDamaged(value);
            }).AddTo(this);
            _soundManager = soundManager;
        }

        private async UniTask LighthouseDamaged(float _)
        {
            if (_playerHealth.CurrentHealth.CurrentValue > 0)
            {
                _cameraController.SmallShake();
                return;
            }
            
            _cameraController.BigShake();
            var lightzone = _lighthouse.Lightzone.gameObject;
            _soundManager.PlaySfx(_lighthouse.Config.ExplorationSound, transform.position);
            _lighthouse.Death();
            //flickering
            for (var i = 0; i < flickeringCount; ++i)
            { 
                await UniTask.WaitForSeconds(flickeringDuration, cancellationToken: this.GetCancellationTokenOnDestroy());
                lightzone.SetActive(true);
                await UniTask.WaitForSeconds(flickeringDuration, cancellationToken: this.GetCancellationTokenOnDestroy());
                lightzone.SetActive(false);
            }
        }
        
        
        public void Awake()
        {
            DisableAllLighthouses();
        }


        private void DisableAllLighthouses()
        {
            foreach (var beh in lighthouseBehaviours)
            {
                beh.gameObject.SetActive(false);
            }
        }

        public void EnableLighthouse(LighthouseId id)
        {
            DisableAllLighthouses();
            _lighthouse = lighthouseBehaviours
                .AsValueEnumerable()
                .First(x => x.Id == id);
            
            _lighthouse.StatLevels = _dataService.GetData().StatLevels
                .AsValueEnumerable()
                .First(x => x.Id == id);
            
            _lighthouse.gameObject.SetActive(true);
        }

        public void InitializeLighthouse(LighthouseId id)
        {
            _lighthouse.StatLevels = _dataService.GetData().StatLevels
                .AsValueEnumerable()
                .First(x => x.Id == id);
            _lighthouse.Initialize();
            
            _playerHealth.Set(_lighthouse.Config.Stats[StatType.Health]
                .GetCalculatedValue(_lighthouse.StatLevels.Levels[StatType.Health]));
        }

        public void DisableLighthouse()
        {
            _lighthouse.Lightzone.gameObject.SetActive(false);
        }
        
        public LighthouseBase Lighthouse => _lighthouse;
        
        public LighthouseStatLevels GetActiveLighthouseStatLevels() => _lighthouse.StatLevels;

        public LighthouseConfig GetActiveLighthouseConfig() => GetLighthouseConfig(_lighthouse.Config.Id);

        public LighthouseConfig GetLighthouseConfig(LighthouseId id)
        {
            return lighthouseBehaviours.First(x => x.Id == id).Config;
        }

        public void LevelupLighthouseStat(LighthouseId id, StatType statType)
        {
            _dataService.GetData().StatLevels
                .AsValueEnumerable()
                .First(x => x.Id == id).Levels[statType]++;
            _lighthouse.StatLevels = _dataService.GetData().StatLevels
                .AsValueEnumerable()
                .First(x => x.Id == id);
        }
        

        public void PerformUpdate(float dt)
        {
            _lighthouse.Lightzone.PerformUpdate(dt);
        }


        public void Revive()
        {
            _playerHealth.Restore();
            _lighthouse.Revive();
            _lighthouse.Lightzone.gameObject.SetActive(true);
        }
    }
}