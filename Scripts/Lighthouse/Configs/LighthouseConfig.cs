using AYellowpaper.SerializedCollections;
using Lighthouse.Stats;
using Lighthouse.Types;
using UnityEngine;

namespace Lighthouse.Configs
{
    [CreateAssetMenu(fileName = "LighthouseConfig", menuName = "Configs/LighthouseConfig")]
    public class LighthouseConfig : ScriptableObject
    {
        [Header("General")]
        [SerializeField] private int price;
        [SerializeField] private bool isLocked;

        [SerializeField] private LighthouseId id;
        
        [Header("Text")]
        [SerializeField] private string title;
        [SerializeField] private string description;

        [Header("Stats")] [SerializedDictionary]
        public SerializedDictionary<StatType, StatData> Stats;
        
        [Header("Sfx")]
        [SerializeField] private AudioClip explorationSound;

        public int Price => price;
        public LighthouseId Id => id;
        public bool IsLocked => isLocked;
        
        public string Title => title;
        public string Description => description;
        
        public AudioClip ExplorationSound => explorationSound;
    }
}