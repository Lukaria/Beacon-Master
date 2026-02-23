using UI.Windows.Common;
using UI.Windows.Interfaces;

namespace Game.Signals
{
    public struct GameRestartedSignal : IOpenWindowSignal
    {
        public WindowId Id { get; set; }
    }
}