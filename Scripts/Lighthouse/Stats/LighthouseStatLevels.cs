using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Lighthouse.Types;
using MessagePack;

namespace Lighthouse.Stats
{
    [MessagePackObject]
    public struct LighthouseStatLevels
    {
        [Key(0)] public LighthouseId Id { get; private set; }

        [Key(1)]
        public Dictionary<StatType, int> Levels { get; set; }

        public LighthouseStatLevels(LighthouseId id)
        {
            Id = id;
            Levels = new Dictionary<StatType, int>
            {
                { StatType.Brightness, 1 },
                { StatType.Radius, 1 },
                { StatType.MinAngle, 1 },
                { StatType.MaxAngle, 1 },
                { StatType.Speed, 1 },
                { StatType.Health, 1 }
            };
        }

        public LighthouseStatLevels(LighthouseId id, SerializedDictionary<StatType, StatData> stats)
        {
            Id = id;
            Levels = new Dictionary<StatType, int>();
            foreach (var stat in stats)
            {
                Levels.Add(stat.Key, stat.Value.BaseLevel);
            }
        }
    }
}