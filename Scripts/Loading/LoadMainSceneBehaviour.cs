using System;
using UnityEngine;
using Zenject;

namespace Loading
{
    public class LoadMainSceneBehaviour : MonoBehaviour
    {
        [SerializeField] private string sceneName;
        private ISceneLoader _sceneLoader;

        [Inject]
        public void Construct(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        private void Awake()
        {
            _sceneLoader.LoadSceneAsync(sceneName);
        }
    }
}