using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameResources.Interfaces
{
    public interface ISpriteAtlasHandler
    {
        UniTask LoadSpriteAtlasAsync(string spriteAtlas);

        Sprite GetSprite(Enum sprite);

        bool IsLoaded { get; }
    }
}