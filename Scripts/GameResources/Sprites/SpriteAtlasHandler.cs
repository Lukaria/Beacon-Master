using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameResources.Interfaces;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;

namespace GameResources.Sprites
{
    public class SpriteAtlasHandler<TEnum> : ISpriteAtlasHandler where TEnum : struct, Enum
    {
        private Dictionary<TEnum, Sprite> _sprites = new();
        

        public bool IsLoaded { get; protected set; }
        
        public virtual async UniTask LoadSpriteAtlasAsync([CanBeNull] string spriteAtlas)
        {
            var atlasName  = spriteAtlas ?? typeof(TEnum).Name;
            var task = Addressables.LoadAssetAsync<SpriteAtlas>(atlasName);
            await task.Task;
            var atlas = task.Result;

            if (atlas is null)
            {
                Utils.Assertions.Assert("no sprite atlas found!");
            }

            foreach (TEnum enumValue in Enum.GetValues(typeof(TEnum)))
            {
                var sprite = atlas!.GetSprite(enumValue.ToString());
                if (sprite is not null)
                {
                    _sprites.Add(enumValue, sprite);
                }
            }
            IsLoaded = true;
        }
        
        
        public virtual Sprite GetSprite(Enum key)
        {
            if (key is TEnum typedKey)
            {
                return GetSpriteTyped(typedKey);
            }
            
            Utils.Assertions.Assert(nameof(Enum) + " is not " + nameof(TEnum) + "!");
            return null;
        }
        
        protected virtual Sprite GetSpriteTyped(TEnum type)
        {
            if (!IsLoaded)
            {
                Utils.Assertions.Assert("sprite atlas not loaded!");
                return null;
            }
            
            if (_sprites.TryGetValue(type, out var sprite))
            {
                return sprite;
            }
            
            Utils.Assertions.Assert("no sprite named " + type + "!");
            return null;
        }

        
    }
}