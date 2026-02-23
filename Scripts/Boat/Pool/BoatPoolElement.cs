using System;
using Boat.Boats;

namespace Boat.Pool
{
    [Serializable]
    public class BoatPoolElement
    {
        public BoatBase boat;
        public BoatStats stats;
    }
}