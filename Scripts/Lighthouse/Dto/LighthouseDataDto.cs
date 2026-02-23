using System;
using System.Collections.Generic;
using System.Linq;
using Lighthouse.Configs;
using Lighthouse.Stats;
using Lighthouse.Types;
using MessagePack;

namespace Lighthouse.Dto
{
    
    [MessagePackObject]
    public class LighthouseDataDto
    {
        [Key(0)]
        public List<LighthouseStatLevels> StatLevels { get; set; }

        public LighthouseDataDto()
        {
            var ids = Enum.GetValues(typeof(LighthouseId)).Cast<LighthouseId>().ToList();
            StatLevels = new List<LighthouseStatLevels>();
            foreach (var id in ids)
            {
                StatLevels.Add(new LighthouseStatLevels(id));
            }
        }
        
        public LighthouseDataDto(LighthouseRepositoryData repository)
        {
            var ids = Enum.GetValues(typeof(LighthouseId)).Cast<LighthouseId>().ToList();
            StatLevels = new List<LighthouseStatLevels>();
            foreach (var id in ids)
            {
                StatLevels.Add(new LighthouseStatLevels(id, repository.configs[id].Stats));
            }
        }
    }
}