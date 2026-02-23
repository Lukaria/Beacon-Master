using System.Collections.Generic;
using Abilities;
using Lighthouse.Types;
using Location;
using MessagePack;

namespace Leaderboard
{
    [MessagePackObject]
    public class LeaderboardDataDto
    {
        [Key(0)] 
        public LocationId Location;

        [Key(1)] public LighthouseId Lighthouse;
        
        [Key(2)]
        public float Score = 0.0f;
        
        [Key(3)]
        public float Time = 0.0f;
        
        [Key(4)]
        public Dictionary<AbilityId, int> Abilities  = new ();
    }
    
}