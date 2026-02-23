using System;

namespace Ads
{
    public interface IAdsInitializer
    {
        event Action OnInitialized;
        bool IsInitialized { get; }
    }
}