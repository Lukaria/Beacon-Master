using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Configs
{
    [CreateAssetMenu(fileName ="DifficultyConfig", menuName = "Configs/DifficultyConfig")]
    public class DifficultyConfig : ScriptableObject
    {
        [FormerlySerializedAs("_levels")] [SerializeField] 
        private TimeConfigEntry[] levels;

        public TimeConfigEntry[] LevelConfigs => levels;
    }

    [Serializable]
    public struct TimeConfigEntry
    {
        public float TimeThreshold;
        public LevelConfig LevelConfig;
    }
}