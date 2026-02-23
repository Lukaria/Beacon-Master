using UI.Windows.Common;
using UI.Windows.Interfaces;

namespace UI.Windows.Signals
{
    public class OpenWindowSignal : IOpenWindowSignal
    {
        public WindowId Id { get; set; }
    }
}