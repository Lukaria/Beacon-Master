using System;
using System.Collections.Generic;
using System.Linq;
using Lighthouse;
using Lighthouse.Stats;
using Lighthouse.Types;
using MessagePack;

namespace Player
{
    [MessagePackObject]
    public class PlayerDataDto
    {
        [Key(0)]
        public int CurrentUnlockedIndex = 0;
        
        [Key(1)]
        public List<LighthouseId> Unlocked = new() { LighthouseId.Standard };

        [Key(2)]
        public int Cash = 500;
        
        public PlayerDataDto() {}
    }
}