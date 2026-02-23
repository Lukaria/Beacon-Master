using System;
using Game;
using Lighthouse.Pool;
using R3;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Location
{
    public class FogShaderMaskBehaviour : MonoBehaviour
    {
        [SerializeField] private string maskPositionVariable = "_MaskPosition";
        [SerializeField] private string maskRadiusVariable = "_MaskRadius";
        [SerializeField] private LayerMask layerMask;
        private Material _fogMaterial; 
    
        private LighthouseService _lighthouse;
        private GameManager _gameManager;
        private Transform _targetObject;
        private bool _isPlaying;

        [Inject]
        public void Construct(LighthouseService lighthouseService, GameManager gameManager)
        {
            _lighthouse =  lighthouseService;
            _fogMaterial = GetComponent<MeshRenderer>().material;
            gameManager.
                GameStateUpdated
                .Subscribe(OnGameStateChanged)
                .AddTo(this);
        }

        private void OnGameStateChanged(GameState obj)
        {
            if (obj != GameState.Playing)
            {
                _isPlaying = false;
                SetMaskData(Vector3.zero, 0);
                return;
            };
            _isPlaying = true;
            _targetObject = _lighthouse.Lighthouse.Lightzone.transform;
        }

        void FixedUpdate()
        {
            if (!_isPlaying) return;
            if (!_fogMaterial) return;
            
            var ray = new Ray(_targetObject.position, _targetObject.forward);
            
            if (!Physics.Raycast(ray, out var hit, 100, layerMask)) return;
            
           SetMaskData(new Vector3(hit.point.x, 1.0f, hit.point.z), _lighthouse.Lighthouse.Lightzone.Stats.Radius);
        }

        private void SetMaskData(Vector3 position, float radius)
        {
            _fogMaterial.SetVector(maskPositionVariable, position);
            _fogMaterial.SetFloat(maskRadiusVariable, radius);
        }

        private void OnDrawGizmosSelected()
        {
            if (!_targetObject) return;
            Gizmos.DrawRay(_targetObject.position, _targetObject.forward * 100);
        }
    }
}