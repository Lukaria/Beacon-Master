using Cysharp.Threading.Tasks;
using UI.Windows.Common;

namespace UI.Windows.Interfaces
{
    public interface IUiNavigation
    {
        public UniTask<WindowBase> OpenAsync(WindowId id);
        public UniTask GoBackAsync();
    }
}