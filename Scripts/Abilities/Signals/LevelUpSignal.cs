using UI.Windows.Common;
using UI.Windows.Interfaces;

namespace Abilities.Signals
{
    public struct LevelUpSignal : IOpenWindowSignal
    {
        public WindowId Id { get; set; } 
    }
}