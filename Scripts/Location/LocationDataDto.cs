using System.Collections.Generic;
using MessagePack;

namespace Location
{
    [MessagePackObject]
    public class LocationDataDto
    {
        [Key(0)]
        public int CurrentLocationIndex = 0;
        
        [Key(1)]
        public List<LocationId> Unlocked = new() { LocationId.Standard };
    }
}