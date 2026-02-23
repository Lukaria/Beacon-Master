using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameResources.Interfaces;
using UnityEngine;

namespace GameResources.Sprites
{
    public class SpriteAtlasManager
    {
        private Dictionary<string, ISpriteAtlasHandler> _atlasHandlers = new();

        public void AddAtlasHandler(string name, in ISpriteAtlasHandler handler)
        {
            _atlasHandlers.TryAdd(name, handler);
        }
        
        public void AddAtlasHandler<TEnum>(in ISpriteAtlasHandler handler)
        {
            _atlasHandlers.TryAdd(typeof(TEnum).Name, handler);
        }

        public async UniTask LoadAtlasesAsync()
        {
            var tasks = Enumerable.Select(_atlasHandlers, kvp => kvp.Value.LoadSpriteAtlasAsync(kvp.Key));
            await UniTask.WhenAll(tasks);
        }

        public ISpriteAtlasHandler GetAtlasHandler(string atlasName)
        {
            _atlasHandlers.TryGetValue(atlasName, out var atlasHandler);
            if (atlasHandler is not null)
            {
                return atlasHandler;
            }
            Utils.Assertions.Assert("no such atlas handler!");
            return null;
        }
    }
}