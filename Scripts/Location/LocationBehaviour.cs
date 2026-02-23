using Game.Configs;
using Location.Configs;
using UnityEngine;

namespace Location
{
    public class LocationBehaviour : MonoBehaviour
    {
        [SerializeField] private LocationConfig config;
        [SerializeField] private DifficultyConfig  difficultyConfig;

        public DifficultyConfig DifficultyConfig => difficultyConfig;
        public LocationConfig LocationConfig => config;
    }
}