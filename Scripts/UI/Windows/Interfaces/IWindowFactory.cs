using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UI.Windows.Common;
using UnityEngine;

namespace UI.Windows.Interfaces
{
    public interface IWindowFactory
    {
        UniTask<WindowBase> CreateAsync(WindowId id, Transform parent);
    }
}