using Lighthouse.Stats;
using Lighthouse.Types;
using Lightzone;
using Lightzone.Dto;
using UnityEngine;

namespace Lighthouse.Lighthouses
{
    public class StandardLighthouse : LighthouseBase
    {
        [SerializeField] private CylinderLightzone lightzone;

        public override LightzoneBase Lightzone => lightzone;
        public override LighthouseStatLevels StatLevels { get; set; }
        public override LighthouseId Id => LighthouseId.Standard;
        
        public override void Initialize()
        {
            var stats = new LightzoneDto()
            {
                Brightness = config.Stats[StatType.Brightness].GetCalculatedValue(StatLevels.Levels[StatType.Brightness]),
                Radius = config.Stats[StatType.Radius].GetCalculatedValue(StatLevels.Levels[StatType.Radius]),
                MinAngle = config.Stats[StatType.MinAngle].GetCalculatedValue(StatLevels.Levels[StatType.MinAngle]),
                MaxAngle = config.Stats[StatType.MaxAngle].GetCalculatedValue(StatLevels.Levels[StatType.MaxAngle]),
                Speed = config.Stats[StatType.Speed].GetCalculatedValue(StatLevels.Levels[StatType.Speed]),
            };
            EnableLightzone(stats);
        }

        public void EnableLightzone(LightzoneDto dto)
        {
            lightzone.Initialize(transform.position, dto);
            lightzone.gameObject.SetActive(true);
        }
    }
    
    
}