using System;
using AYellowpaper.SerializedCollections;
using Lighthouse.Types;
using UnityEngine;

namespace Lighthouse.Configs
{
    [CreateAssetMenu(fileName = "LighthouseRepository", menuName = "Configs/LighthouseRepository")]
    public class LighthouseRepository : Zenject.ScriptableObjectInstaller
    {
        public LighthouseRepositoryData configData;

        public override void InstallBindings()
        {
            Container.BindInstance(configData).AsSingle();
        }
        
        public void OnValidate()
        {
            foreach (var (key, config) in configData.configs)
            {
                Utils.Assertions.IsTrueAssert(key == config.Id);
            }
        }
    }

    [Serializable]
    public class LighthouseRepositoryData
    {
        [SerializedDictionary] 
        public SerializedDictionary<LighthouseId, LighthouseConfig> configs;
    }
}