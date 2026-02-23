using System;
using Input;
using Lighthouse.Types;
using TMPro;
using UnityEngine;
using Zenject;

namespace Lighthouse.Lighthouses
{
    public class CavemanLighthouse : StandardLighthouse
    {
        public override LighthouseId Id { get; } = LighthouseId.Caveman;
        
        [SerializeField] private Animator cavemanAnimator;
        [SerializeField] private string angleValueName = "Angle";
        [SerializeField] private GameObject lensGO;
        private InputManager _inputManager;

        [Inject]
        public void Construct(InputManager inputManager)
        {
            _inputManager = inputManager;
            
        }

        private void Update()
        {
            var t = Lightzone.gameObject.transform.rotation.eulerAngles;
            var inputPos = _inputManager.GetLastPosition().normalized;
            var vector = new Vector2(inputPos.x, t.y).normalized;
            var angle = Vector2.SignedAngle(vector, inputPos);
            HandleCavemanAnimation(angle);
            HandleLensRotation(angle);
        }
        
        private void HandleCavemanAnimation(float angle)
        {
            if (angle is > -10 and < 10) angle = 0;
            cavemanAnimator.SetFloat(angleValueName, angle);
        }

        private void HandleLensRotation(float angle)
        {
            lensGO.transform.eulerAngles = new Vector3(0, Lightzone.transform.rotation.eulerAngles.y, 0); 
        }

        public override void Death()
        {
            cavemanAnimator.SetTrigger("Death");
        }
        
        public override void Revive()
        {
            cavemanAnimator.SetTrigger("Revive");
        }
    }
}