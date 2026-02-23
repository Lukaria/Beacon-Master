using System.Collections.Generic;
using Common.Constants;
using Lighthouse.Configs;
using Lighthouse.Pool;
using Lighthouse.Stats;
using Player;
using UnityEngine;
using Zenject;

namespace UI.Windows.LighthouseShop.Enhancements
{
    public class EnhancementsContainer : MonoBehaviour
    {
        [SerializeField] private List<Enhancement> enhancements;
        
        private PlayerDataService _playerData;
        private LighthouseService _lighthouseService;

        [Inject]
        public void Construct(LighthouseService service, PlayerDataService playerData)
        {
            _lighthouseService = service;
            _playerData = playerData;
        }
        
        
        public void Show()
        {
            var enhancementDtos = GetEnhancementDtos();
            gameObject.SetActive(true);
 
            for (var i = 0; i < enhancementDtos.Count; i++)
            {
                var dto = enhancementDtos[i];
                enhancements[i].Show(dto);
                var i1 = i;
                enhancements[i].EnhancementButtonClicked += () =>
                {
                    EnhancementButtonClicked(enhancements[i1], dto);
                };
            }
            
            
        }

        private List<EnhancementDto> GetEnhancementDtos()
        {
            var lighthouseConfig = _lighthouseService.GetActiveLighthouseConfig();
            var level = _lighthouseService.GetActiveLighthouseStatLevels();
            
            var enhancementDtos = new List<EnhancementDto>()
            {
                EvaluateEnhancement(lighthouseConfig, level, StatType.Health),
                EvaluateEnhancement(lighthouseConfig, level, StatType.Brightness),
                EvaluateEnhancement(lighthouseConfig, level, StatType.Radius),
                EvaluateEnhancement(lighthouseConfig, level, StatType.Speed),
                EvaluateEnhancement(lighthouseConfig, level, StatType.MinAngle),
                EvaluateEnhancement(lighthouseConfig, level, StatType.MaxAngle),
            };
            
            return enhancementDtos;
        }

        private EnhancementDto EvaluateEnhancement(LighthouseConfig config, LighthouseStatLevels statLevels, StatType type)
        {
            var level = statLevels.Levels[type];
            return new EnhancementDto()
            {
                Type = type,
                Level = level,
                MaxLevel = LighthouseConstants.MAX_LEVEL,
                Price = config.Stats[type].GetPrice(level)
            };
        }

        private void EnhancementButtonClicked(Enhancement enhancement, EnhancementDto dto)
        {
            if (!_playerData.TrySubtractCash(dto.Price)) return;
            
            _lighthouseService.LevelupLighthouseStat(_playerData.UnlockedLighthouseId, dto.Type);

            enhancement.Show(
                EvaluateEnhancement(_lighthouseService.GetActiveLighthouseConfig(),
                    _lighthouseService.GetActiveLighthouseStatLevels(),
                    dto.Type
                )
            );
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}