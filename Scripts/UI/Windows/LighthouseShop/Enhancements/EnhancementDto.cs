using Common.Constants;
using Lighthouse.Stats;

namespace UI.Windows.LighthouseShop.Enhancements
{
    public struct EnhancementDto
    {
        public StatType Type { get; init; }
        public int Level { get; init; }
        public int MaxLevel { get; init; }
        public int Price { get; init; }
    }
}